using HealthHup.API.Model.Models.Hospital.Patient;
using HealthHup.API.Service.AccountService;
using HealthHup.API.Service.MlService;

namespace HealthHup.API.Service.ModelService.HospitalService.DrugModelService
{
    public class DrugModelApiService:BaseService<Drug>, IDrugModelApiService
    {
        private readonly IMLApiService _MLApiService;
        private readonly IPatientInfoService _PatientInfoService;
        private readonly IBaseService<interaction> _interactionService;
        private readonly IBaseService<Disease> _diseaseService;
        private readonly IAuthService _Authservice;
        public DrugModelApiService(ApplicatoinDataBaseContext db, IMLApiService mLApiService, IPatientInfoService patientInfoService, IBaseService<interaction> interactionService,
            IBaseService<Disease> diseaseService, IAuthService Authservice) : base(db)
        {
            _MLApiService = mLApiService;
            _PatientInfoService = patientInfoService;
            _interactionService = interactionService;
            _diseaseService = diseaseService;
            _Authservice = Authservice;
        }

        public async Task<InteractivitiyDto> CheackListDrugs(string PatientEmail,List<Guid> DrugIds)
        {
            var Result = new List<Drug>();
            foreach(Guid d in DrugIds)
            {
                string id=d.ToString();
                var r = await findAsync(d=>d.Id== id);
                if (r == null)
                    return new InteractivitiyDto(new List<string>(),false);
                Result.Add(r);
            }
            return await SearchingForInteractivitiy(PatientEmail, Result);
        }

        private async Task<InteractivitiyDto> SearchingForInteractivitiy(string PatientEmail, List<Drug> input)
        {
            //Get Patient
            var Patient=await _Authservice.GetUserAsync(PatientEmail);
            //Get Diseas
            var Diseas = await _diseaseService.findByAsync(d=>d.PatientId== Patient.Id);
            //Get Current Drug
            var Drugs= await _PatientInfoService.GetCurrentDrugs(PatientEmail);
            //Add New Drugs
            Drugs.AddRange(input);
            Drugs=Drugs.Distinct().ToList();

            List<string> Result=new List<string>();
            for (int i = 0; i < Drugs.Count; i++)
                for (int j = i + 1; j < Drugs.Count; j++)
                {
                   var r= await _MLApiService.GetTypeInteraction(Drugs[i].smiles, Drugs[j].smiles);
                    if (r.InteractionResult)
                    {
                        var result= await _interactionService.findAsNotTrakingync(a=>a.Id==(r.InteractionNameIndex+1),AsNotTraking:true);
                        Result.Add($"Drug:{Drugs[i].name} And Drug:{Drugs[j].name}\nThey May Cause {result?.Reason}");
                    }
                }

            Result.ForEach(d =>
            {
                foreach (var i in Diseas)
                    if (d.Contains(i.Name))
                        d = $"{d} ,Patient has the disease";
            });
            return new InteractivitiyDto(Result,Result.Count>0);

        }
    }
}
