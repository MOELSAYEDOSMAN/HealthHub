using HealthHup.API.Service.AccountService;

namespace HealthHup.API.Service.ModelService.Admin.Alerts
{
    public class GlobalAlertService:BaseService<AlertBlcok>
    {
        private readonly ISendMessageService _MessegService;
        private readonly IAuthService _authService;
        public GlobalAlertService(ApplicatoinDataBaseContext db,
            ISendMessageService MessegService
            ,IAuthService authService
            ) :base(db)
        {
            _MessegService= MessegService;
            _authService= authService;
        }

        public async Task AddAlert(ApplicationUser user,AlertType alertType,DateTime?date,string? Message,string? DoctorName)
        {
            var alert=new AlertBlcok(user, alertType, date ?? DateTime.Now, Message);
            var alertsUsers = await findByAsync(a=>a.UserId==user.Id);
            
            var DrRate = alertsUsers.Where(r => r.alertType == AlertType.RateLow
            &&r.DateTime.Date.Year<DateTime.UtcNow.Year
            &&r.DateTime.Month==DateTime.UtcNow.Month);
            
            if(alertsUsers.Count() == 3|| DrRate.Count()>0)
            {
                await _authService.BlockUser(user);
                await _MessegService.TakeBlock(user.Email, user.Name, alertType == AlertType.CancelManyDates?"Because Of You Lack Of Commitment to attend":"Rate Low");
            }
            else
            {
                if (alertType == AlertType.CancelManyDates)
                    await _MessegService.ForgetDateMedicalSession(user.Email, user.Name, DoctorName, Message);
                else
                    await _MessegService.RateDoctorLow(user.Email, user.Name);
                
                if(alertType == AlertType.CancelManyDates || alertsUsers.Where(r => r.alertType == AlertType.RateLow).Count()==0)
                    await AddAsync(alert);
            }
        }

        public async Task RemoveAll(Guid Id)
        {
            var alert =await findAsync(a => a.Id == Id);
            if (alert == null)
                return;
            await RemoveAsync(alert);
        }
    }
}
