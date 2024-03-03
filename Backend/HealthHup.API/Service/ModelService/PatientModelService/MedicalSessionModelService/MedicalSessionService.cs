using HealthHup.API.Service.AccountService;
namespace HealthHup.API.Service.ModelService.PatientModelService.MedicalSessionModelService
{
    public class MedicalSessionService:BaseService<MedicalSession>, IMedicalSessionService
    {
        private readonly IAuthService _authService;

        private readonly IDoctorService _dctorService;

        private readonly IBaseService<Drug> _drugService;
        private readonly IBaseService<Repentance> _repentanceService;
        private readonly IBaseService<Disease> _diseaseService;
        public MedicalSessionService(ApplicatoinDataBaseContext db, IAuthService authService, IDoctorService dctorService, IBaseService<Drug> drugService, IBaseService<Repentance> repentanceService, IBaseService<Disease> diseaseService) : base(db)
        {
            _authService = authService;
            _dctorService = dctorService;
            _drugService = drugService;
            _repentanceService = repentanceService;
            _diseaseService = diseaseService;
        }

        //post
        public async Task<string> AddMedicalSessionAsync(MedicalSessionDTO input, string EmailDoctor)
        {
            //Get Doctor
            var DoctorAuth = await _authService.GetUserAsync(EmailDoctor);
            if (DoctorAuth == null)
                return "Must Login";
            var Doctor = await _dctorService.findAsync(d => d.doctorId == DoctorAuth.Id);
            if (Doctor == null)
                return "Not Your Role";
            //Get Patient
            var PatientAuth = await _authService.GetUserAsync(input?.patientEmail);
            if (PatientAuth == null)
                return "Must Select Patient";
            //cheack Drugs
            if (input?.repentances?.Count > 0)
                foreach (var r in input?.repentances)
                    if ((await _drugService.findAsync(d => d.Id == r.drugId)) == null)
                        return "Must Select Correct Drug";
            //Add|Update State Disease
            if (!(await DiseaseAsync(input, PatientAuth.Id, Doctor.Id)))
                return "Try Agin";


            //Create MedicalSession
            MedicalSession NewSession = input;
            NewSession.DoctorId = Doctor.Id;
            NewSession.PatientId= PatientAuth.Id;

            //Add New Drugs
            input?.repentances.ForEach(
                 r => {
                    Repentance NewRepentance = r;
                    NewRepentance.drugId = r.drugId;
                    NewSession.repentances.Add(NewRepentance);
                }
                );

            await AddAsync(NewSession);
            return "Done";
        }



        //Disease Switch
        public async Task<bool> DiseaseAsync(Disease input, string PatientId, Guid DoctorId)
        {
            //Cheack If Have old Disease
            var disease = await _diseaseService.findByAsync(d => d.ApplicationUserId == PatientId && d.Name.ToUpper() == input.Name.ToUpper());
           
            
            return disease != null ? await UpdateDiseaseAsync(input, PatientId, DoctorId) :
                await AddNewDiseaseAsync(input, PatientId, DoctorId);
        }

        private async Task<bool> AddNewDiseaseAsync(Disease input, string PatientId, Guid DoctorId)
        {
            input.Id = Guid.NewGuid();
            input.responsibledDoctorId = DoctorId;
            input.ApplicationUserId = PatientId;
            return await _diseaseService.AddAsync(input);
        }

        private async Task<bool> UpdateDiseaseAsync(Disease input, string PatientId, Guid DoctorId)
        {
            var disea=await _diseaseService.findAsync(d => d.Id==input.Id);
            disea.Cured = input.Cured;
            disea.Notes = input?.Notes??disea?.Notes;
            disea.persistent = input.persistent;
            disea.responsibledDoctorId = DoctorId;
            return await _diseaseService.UpdateAsync(input);
        }
    }
}
