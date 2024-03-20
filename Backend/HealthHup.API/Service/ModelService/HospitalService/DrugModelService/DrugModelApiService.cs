using HealthHup.API.Service.MlService;

namespace HealthHup.API.Service.ModelService.HospitalService.DrugModelService
{
    public class DrugModelApiService:BaseService<Drug>, IDrugModelApiService
    {
        private readonly IMLDrugApiService _MLDrugApiService;
        private readonly IPatientInfoService _PatientInfoService;
        private readonly IBaseService<interaction> _interactionService;
        public DrugModelApiService(ApplicatoinDataBaseContext db, IMLDrugApiService mLDrugApiService, IPatientInfoService patientInfoService, IBaseService<interaction> interactionService) : base(db)
        {
            _MLDrugApiService = mLDrugApiService;
            _PatientInfoService = patientInfoService;
            _interactionService = interactionService;
        }

        public async Task<string> CheackListDrugs(string PatientEmail,List<Guid> DrugIds)
        {
            var Result = new List<Drug>();
            foreach(Guid d in DrugIds)
            {
                string id=d.ToString();
                var r = await findAsync(d=>d.Id== id);
                if (r == null)
                    return "Chrack Drugs Input";
                Result.Add(r);
            }
            return await SearchingForInteractivitiy(PatientEmail, Result);
        }

        private async Task<string> SearchingForInteractivitiy(string PatientEmail, List<Drug> input)
        {
            //Get Current Drug
            var Drugs= await _PatientInfoService.GetCurrentDrugs(PatientEmail);
            Drugs.AddRange(input);
            Drugs=Drugs.Distinct().ToList();
            for (int i = 0; i < Drugs.Count; i++)
                for (int j = i + 1; j < Drugs.Count; j++)
                {
                   var r= await _MLDrugApiService.GetTypeInteraction(Drugs[i].smiles, Drugs[j].smiles);
                    if (r.InteractionResult)
                    {
                        var result= await _interactionService.findAsNotTrakingync(a=>a.Id==(r.InteractionNameIndex+1),AsNotTraking:true);
                        return $"Drug:{Drugs[i].name} And Drug:{Drugs[j].name}\nThey May Cause {result?.Reason}";
                    }
                }
            return "Done";

        }
    }
}
