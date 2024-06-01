namespace HealthHup.API.Service.ModelService.HospitalService.DoctorDates
{
    public interface IDoctorDateService
    {
        Task<string> CancelDate(string DoctorEmail, string PatientEmail);
        Task<string> ChangeDate(string DoctorEmail, string PatientEmail, PatientDateInput input);
        Task<string> PushDate(string DoctorEmail, string PatientEmail, PatientDateInput input);
    }
}
