using HealthHup.API.Model.Extion.Hospital.RateModelDto;
using HealthHup.API.Service.AccountService;
using Microsoft.EntityFrameworkCore;

namespace HealthHup.API.Service.ModelService.PatientModelService.RateModelService
{
    public class RateService:BaseService<Review>, IRateService
    {
        private readonly IAuthService _authService;
        private readonly IMedicalSessionService _sessionService;
        private readonly IDoctorService _doctorService;
        public RateService(ApplicatoinDataBaseContext db, IMedicalSessionService sessionService, IAuthService authService, IDoctorService doctorService) : base(db)
        {
            _sessionService = sessionService;
            _authService=authService;
            _doctorService = doctorService;
        }

        public async Task<string> PushRate(string PatientEmail,Guid DoctorId,RateDTO input)
        {
            //Get Patient
            var PatientAuth = await _authService.GetUserAsync(PatientEmail);
            if (PatientAuth == null)
                return "Need To Login";
            //Get Doctor
            var Doctor = await _doctorService.findAsNotTrakingync(d=>d.Id==DoctorId,AsNotTraking:true);
            if (Doctor == null)
                return "Choose A Right Doctor";
            
            
            //Cheack If Patient Is Doctor
            if (Doctor.doctorId == PatientAuth.Id)
                return "Can't Add Rate";
            //CheackGoToDoctor
            var LastMedical = await CheackGoToDoctor(PatientAuth.Id, DoctorId);
            if (LastMedical==null)
                return "You Should Visit a Doctor Previously";
            if (DateOnly.FromDateTime(DateTime.UtcNow.Date) > DateOnly.FromDateTime(LastMedical.date.Date.AddDays(7)))
                return "You Must Rate within a Week";
            
            return await CheackAddOrUpdate(PatientAuth.Id, DoctorId, input) 
                ? "Done":"Try Time Late";
        }


        

        private async Task<bool> AddNewRate(RateDTO input,string PatientId,Guid DoctorId)
        {
            Review review = new Review() { 
            doctorId= DoctorId,
            PatientId= PatientId,
            Message=input?.message,
            rate=input.rate,
            Id= Guid.NewGuid(),
            SendTime=DateTime.UtcNow
            };
            return await AddAsync(review);
        }
        private async Task<bool> UpdateRate(RateDTO input, string PatientId, Guid DoctorId)
        {
            var NewRate = await findAsync(r => r.PatientId == PatientId && r.doctorId == DoctorId);
            NewRate.rate= input.rate;
            NewRate.Message = input?.message;
            return await UpdateAsync(NewRate);
        } 
        private async Task<bool> CheackAddOrUpdate(string PatientId, Guid DoctorId, RateDTO input)
        {
            var rate =await findAsNotTrakingync(r=>r.PatientId==PatientId&&r.doctorId==DoctorId,AsNotTraking:true);
            return rate == null ?await AddNewRate(input,PatientId,DoctorId) :await UpdateRate(input, PatientId, DoctorId);
        }

        private async Task<MedicalSession?> CheackGoToDoctor(string PatientId, Guid DoctorId)
        {
            var MedicalSessions = await _sessionService.findByAsync(s => s.DoctorId == DoctorId && s.PatientId == PatientId,OrderBy:m=>m.date);
            return MedicalSessions.Count > 0?MedicalSessions[MedicalSessions.Count-1]:null;
        }
    }
}
