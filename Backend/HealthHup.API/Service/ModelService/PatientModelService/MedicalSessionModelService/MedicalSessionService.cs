using Hangfire.Storage.Monitoring;
using HealthHup.API.Service.AccountService;
using Microsoft.EntityFrameworkCore;
namespace HealthHup.API.Service.ModelService.PatientModelService.MedicalSessionModelService
{
    public class MedicalSessionService:BaseService<MedicalSession>, IMedicalSessionService
    {
        private readonly IAuthService _authService;

        private readonly IDoctorService _dctorService;

        private readonly IBaseService<Drug> _drugService;
        private readonly IBaseService<Repentance> _repentanceService;
        private readonly IBaseService<Disease> _diseaseService;
        private readonly IBaseService<PatientDates> _patientDate;
        public MedicalSessionService(ApplicatoinDataBaseContext db, IAuthService authService, IDoctorService dctorService, IBaseService<Drug> drugService, IBaseService<Repentance> repentanceService, IBaseService<Disease> diseaseService, IBaseService<PatientDates> patientDate) : base(db)
        {
            _authService = authService;
            _dctorService = dctorService;
            _drugService = drugService;
            _repentanceService = repentanceService;
            _diseaseService = diseaseService;
            _patientDate = patientDate;

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

            //PatientGivePermision
            var PatientDates = await _patientDate.findByAsync(d => d.patientId == PatientAuth.Id && d.doctorId == Doctor.Id && d.date.Date == DateTime.UtcNow.Date);
            
            if (PatientDates.Count == 0)
                return "The Patient Must Make A Pre-Bookin";

            //Cheack IF MedicalSession Create
            var MedicNow = await findAsync(m => m.DoctorId == Doctor.Id && m.date.Date == DateTime.UtcNow.Date && m.PatientId == PatientAuth.Id);
            if (MedicNow != null)
                return "Added Before";


            //cheack Drugs
            if (input?.repentances?.Count > 0)
                foreach (var r in input?.repentances)
                    if ((await _drugService.findAsync(d => d.Id == r.drugId)) == null)
                        return "Must Select Correct Drug";
            //Add|Update State Disease
            if (!(await DiseaseAsync(input, PatientAuth.Id, Doctor.Id)))
                return "Try Agin";
            
            await CreateNewMedicalSession(Doctor.Id, PatientAuth.Id, input, input?.repentances);
            await _patientDate.RemoveAsync(PatientDates[0]);

            return "Done";
        }

        //Put
        public async Task<string> AddMedicalSessionNewDrugsAsync(string emailDoctor,string emailPaient,List<RepentanceDto> newRepentances)
        {
            //Get Doctor
            var DoctorAuth = await _authService.GetUserAsync(emailDoctor);
            if (DoctorAuth == null)
                return "Must Login";

            var Doctor = await _dctorService.findAsync(d => d.doctorId == DoctorAuth.Id);
            if (Doctor == null)
                return "Not Your Role";

            //Get Patient
            var PatientAuth = await _authService.GetUserAsync(emailPaient);
            if (PatientAuth == null)
                return "Must Select Patient";

            //Cheack If Have MedicalSession In Day
            var MedicNow = await findAsync(m => m.DoctorId == Doctor.Id && m.date.Date == DateTime.UtcNow.Date && m.PatientId == PatientAuth.Id, new string[] { "repentances" });
            if (MedicNow == null)
                return "Is No Medical Session Today";

            if (newRepentances?.Count > 0)
                foreach (var r in newRepentances)
                    if ((await _drugService.findAsync(d => d.Id == r.drugId)) == null)
                        return "Must Select Correct Drug";
            
            foreach(var r in newRepentances)
            {
                if (MedicNow.repentances.Find(d => d.drugId == r.drugId) != null)
                    return "The Medicine was Previously Found in The PreScription";
                Repentance NewRepentance = r;
                NewRepentance.drugId = r.drugId;
                MedicNow.repentances.Add(NewRepentance);
            }
            await UpdateAsync(MedicNow);
            return "Done";
        }
        //Delete 
        public async Task<string> RemoveMedicalSessionNewDrugsAsync(string emailDoctor, string emailPaient, List<string> RemovedRepentances)
        {
            //Get Doctor
            var DoctorAuth = await _authService.GetUserAsync(emailDoctor);
            if (DoctorAuth == null)
                return "Must Login";

            var Doctor = await _dctorService.findAsync(d => d.doctorId == DoctorAuth.Id);
            if (Doctor == null)
                return "Not Your Role";

            //Get Patient
            var PatientAuth = await _authService.GetUserAsync(emailPaient);
            if (PatientAuth == null)
                return "Must Select Patient";

            //Cheack If Have MedicalSession In Day
            var MedicNow = await findAsync(m => m.DoctorId == Doctor.Id && m.date.Date == DateTime.UtcNow.Date && m.PatientId == PatientAuth.Id,new string[] { "repentances" });
            if (MedicNow == null)
                return "Is No Medical Session Today";

            if (RemovedRepentances?.Count > 0)
                RemovedRepentances.ForEach(r =>
                {
                    var Drug= MedicNow.repentances.Find(d => d.drugId == r);
                    if (Drug != null)
                        MedicNow.repentances.Remove(Drug);
                });
            await UpdateAsync(MedicNow);
            return "The Medicine Has Been Removed";
        }
        
        
        //Get
        //Get With Id
        public async Task<OMedical?> GetWithId(Guid Id,string email)
        {
            //Get User 
            var User = await _authService.GetUserAsync(email);
            if (User == null)
                return new OMedical();

            var medical = await _db.MedicalSessions
                .Include(m => m.Doctor).ThenInclude(m => m.doctor)
                .Include(m => m.repentances)
                .SingleOrDefaultAsync(m => m.Id == Id);
            
            if(medical == null)
                return new OMedical();
            
            //Conver From MedicalSession To OMedical
            OMedical result = medical;
            if(medical.repentances.Count==0)
                return result;
            medical.repentances.ForEach(r => result.repentances.Add(r));
            return result;
        }
        //GetList<MedicalSession>With Doctor
        public async Task<List<OutMedicalList>?> GetMedicalSessionsWithDoctorAsync(string email,Guid DoctorId)
        {
            var User=await _authService.GetUserAsync(email);
            if (User == null)
                return null;

            var MedicalSessionDoctors 
                = await findByAsync(m=>m.PatientId==User.Id&&m.DoctorId==DoctorId,new string[] { "repentances","Doctor" });
            if(MedicalSessionDoctors.Count==0)
                return new List<OutMedicalList>();
            List<OutMedicalList> result = new List<OutMedicalList>();
            foreach (var m in MedicalSessionDoctors)
            {
                result.Add(m);
            }
            return result;
        }

        //GetList<MedicalSession>With Patient
        public async Task<List<OutMedicalList>?> GetMedicalSessionsWithPatientAsync(string emailDoctor, string emailPatient)
        {
            var DoctorAuth = await _authService.GetUserAsync(emailDoctor);
            if (DoctorAuth == null)
                return null;

            var PatientAuth = await _authService.GetUserAsync(emailPatient);
            if (PatientAuth == null)
                return null;

            //Get Doctor
            var Doctor = await _dctorService.findAsync(d => d.doctorId == DoctorAuth.Id);
            if (Doctor == null)
                return null;



            var MedicalSessionDoctors
                = await findByAsync(m => m.PatientId == PatientAuth.Id && m.DoctorId == Doctor.Id, new string[] { "repentances", "Doctor" });
            if (MedicalSessionDoctors.Count == 0)
                return new List<OutMedicalList>();
            List<OutMedicalList> result = new List<OutMedicalList>();
            foreach (var m in MedicalSessionDoctors)
            {
                result.Add(m);
            }
            return result;
        }
        //GetList<MedicalSession>With Disease
        public async Task<List<OutMedicalList>?> GetMedicalSessionsWithDiseaseAsync(string Email,string Disease)
        {
            var PatientAuth= await _authService.GetUserAsync(Email);
            if (PatientAuth == null||string.IsNullOrEmpty(Disease))
                return null;
            
            var MedicalSessionDoctors
                = await findByAsync(m=>m.PatientId== PatientAuth.Id&&m.DiseaseName.ToUpper().Contains(Disease), new string[] { "repentances", "Doctor" });
            if(MedicalSessionDoctors.Count==0)
                return new List<OutMedicalList>();
            List<OutMedicalList> result = new List<OutMedicalList>();
            foreach (var m in MedicalSessionDoctors)
            {
                result.Add(m);
            }
            return result;
        }



        //Create MedicalSession
        private async Task<bool> CreateNewMedicalSession(Guid DoctorId, string PatientId, MedicalSession input, List<RepentanceDto>? repentances = null)
        {
            
            input.DoctorId = DoctorId;
            input.PatientId = PatientId;
            if (repentances != null)
            repentances.ForEach(r =>
            {
                Repentance NewRepentance = r;
                NewRepentance.drugId = r.drugId;
                input.repentances.Add(NewRepentance);
            });
            await AddAsync(input);
            return true;
        }
        //Disease Switch
        public async Task<bool> DiseaseAsync(Disease input, string PatientId, Guid DoctorId)
        {
            //Cheack If Have old Disease
            var disease = await _diseaseService.findByAsync(d => d.PatientId == PatientId && d.Name.ToUpper()==input.Name.ToUpper());
            if(disease.Count>0)
                input.Id = disease[0].Id;
            return disease.Count != 0 ? await UpdateDiseaseAsync(input, disease[0], PatientId, DoctorId) :
                await AddNewDiseaseAsync(input, PatientId, DoctorId);
        }
        private async Task<bool> AddNewDiseaseAsync(Disease input, string PatientId, Guid DoctorId)
        {
            if (input.Cured)
                return true;

            input.responsibledDoctorId = DoctorId;
            input.PatientId = PatientId;
            input.Notes = string.IsNullOrEmpty(input.Notes) ?
                "" : $"[{DateOnly.FromDateTime(DateTime.UtcNow)}]:{input.Notes}";
            return await _diseaseService.AddAsync(input);
        }
        private async  Task<bool> UpdateDiseaseAsync(Disease input,Disease disea, string PatientId, Guid DoctorId)
        {
            if (input.Cured)
                return await _diseaseService.RemoveAsync(disea);
            
            disea.Notes = string.IsNullOrEmpty(input?.Notes ?? "")
                ? disea.Notes : $"{disea.Notes} \n\n[{DateOnly.FromDateTime(DateTime.UtcNow)}]:{input.Notes}";
            
            disea.persistent = input.persistent;
            
            disea.responsibledDoctorId = DoctorId;
            _db.Update(disea);
            return true;
        }
    }
}
