namespace HealthHup.API.Service.ModelService.Admin.Alerts
{
    public interface IAlertService
    {
        Task AddAlertDoctorLowRate(ApplicationUser Doctor);
        Task AddAlertPatientCancelDates(ApplicationUser Patient, DateTime dateSession, string DoctorName);
    }
}
