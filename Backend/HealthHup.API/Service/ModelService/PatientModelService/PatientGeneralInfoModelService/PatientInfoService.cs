using HealthHup.API.Service.AccountService;
using Microsoft.EntityFrameworkCore;

namespace HealthHup.API.Service.ModelService.PatientModelService.PatientGeneralInfoModelService
{
    public class PatientInfoService:BaseService<ApplicationUser>, IPatientInfoService
    {
        private readonly IAuthService _authService;
        private readonly IMedicalSessionService _medicalSessionService;
        private readonly IBaseService<Disease> _diseaseService;
        public PatientInfoService(ApplicatoinDataBaseContext db,IAuthService authService,IMedicalSessionService medicalSessionService, IBaseService<Disease> diseaseService) :base(db)
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
        public async Task<List<PatientRepentanceDTO>?> GetRepentanceAsync(string email)
        {
            var Patient = await _authService.GetUserAsync(email);
            if (Patient == null)
                return null;

            var MedicalSessions
                =await _db.MedicalSessions.Where(m => m.PatientId == Patient.Id)
                .Include(m => m.repentances).ThenInclude(r => r.drug).ToListAsync();
            if (MedicalSessions?.Count == 0)
                return null;

            var Result = new List<PatientRepentanceDTO>();
            MedicalSessions.ForEach(m =>Result.Add(m));
            return Result;
        }
    }
}
