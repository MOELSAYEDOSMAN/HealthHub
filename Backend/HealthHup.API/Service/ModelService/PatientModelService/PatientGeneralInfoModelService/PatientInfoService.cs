using HealthHup.API.Service.AccountService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HealthHup.API.Service.ModelService.PatientModelService.PatientGeneralInfoModelService
{
    public class PatientInfoService:BaseService<ApplicationUser>, IPatientInfoService
    {
        private readonly IAuthService _authService;
        private readonly IMedicalSessionService _medicalSessionService;
        private readonly IBaseService<Disease> _diseaseService;
        
        public PatientInfoService(ApplicatoinDataBaseContext db,IAuthService authService,IMedicalSessionService medicalSessionService, IBaseService<Disease> diseaseService, IBaseService<Repentance> repentanceService) :base(db)
        {
            _authService = authService;
            _medicalSessionService=medicalSessionService;
            _diseaseService= diseaseService;
        }

        //Get Disease
        public async Task<List<Disease>?> GetDiseasesAsync(string Email)
        {
            var Patient=await _authService.GetUserAsync(Email);
            if (Patient == null)
                return null;
            var diseases = await _diseaseService.findByAsync(d=>d.PatientId==Patient.Id);
            if(diseases.Count>0)
            {

                var result = diseases.Select(d =>new Disease()
                {
                    Name=d.Name,
                    Cured=d.Cured,
                    Notes=d.Notes,
                    persistent=d.persistent
                } );
                return result.ToList();
            }
            return null;
        }

        //Get Repentance
        public async Task<IEnumerable<PatientRepentanceDTO>?> GetRepentanceAsync(string email)
        {
            var Patient = await _authService.GetUserAsync(email);
            if (Patient == null)
                return null;

            var MedicalSessions
                =await _db.MedicalSessions.Where(m => m.PatientId == Patient.Id)
                .Include(m=>m.Doctor).ThenInclude(d=>d.doctor)
                .Include(m => m.repentances).ThenInclude(r => r.drug).ToListAsync()
                ;
            if (MedicalSessions?.Count == 0)
                return null;

            
            return PatientRepentanceDTO.ConvertFromMedicalSession(MedicalSessions);
        }
        public async Task<List<Drug>?> GetCurrentDrugs(string email)
        {
            var patient = await _authService.GetUserAsync(email);
            if (patient == null)
                return new();
            var reps =await _db.MedicalSessions.AsNoTracking()
                .Include(m=>m.repentances).ThenInclude(r=>r.drug)
                .Where(m=>m.PatientId==patient.Id).SelectMany(m=>m.repentances)
                .Where(r=>(r.EndDate??DateTime.MaxValue.Date)>DateTime.Now.Date)
                .ToListAsync()
                ;
            if(reps.Count==0)
                return new();
            return reps.Select(r => r.drug).ToList();
        }
        //Get=> responsibled Doctor
        public async Task<List<ODoctor>> GetResponsibledDoctorAsync(string Email)
        {
            var Patient = await _authService.GetUserAsync(Email);
            if (Patient == null)
                return new();
            var Diseases = _db.Entry(Patient).
                Collection(p => p.Diseases).Query()
                .Include(d => d.responsibledDoctor).ThenInclude(u => u.doctor)
                .Select(d=>d.responsibledDoctor).ToList();
            if (Diseases.Count() == 0)
                return new();
            var Result = ODoctor.Doctors(Diseases.DistinctBy(d=>d.Id).ToList());
            return Result.ToList();
        }
    }
}
