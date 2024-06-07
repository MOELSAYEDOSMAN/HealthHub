using HealthHup.API.Service.AccountService;

namespace HealthHup.API.Service.ModelService.Admin.Alerts
{
    public class AlertService: IAlertService
    {
        private readonly GlobalAlertService _alertService;
        public AlertService(ApplicatoinDataBaseContext db, ISendMessageService messageService,IAuthService authService)
        {
            _alertService= new GlobalAlertService(db, messageService, authService);
        }

        public async Task AddAlertDoctorLowRate(ApplicationUser Doctor)
        {
            await _alertService.AddAlert(Doctor, AlertType.RateLow, DateTime.Now, string.Empty,string.Empty);
        }

        public async Task AddAlertPatientCancelDates(ApplicationUser Patient, DateTime dateSession, string DoctorName)
        {
            await _alertService.AddAlert(Patient, AlertType.CancelManyDates, dateSession, $"Didn't Go To Session ,Dr/{DoctorName} Date: {DateOnly.FromDateTime(dateSession.Date)}",DoctorName);
        }
    }
}
