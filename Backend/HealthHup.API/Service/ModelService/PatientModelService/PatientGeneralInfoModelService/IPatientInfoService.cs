namespace HealthHup.API.Service.ModelService.PatientModelService.PatientGeneralInfoModelService
{
    public interface IPatientInfoService:IBaseService<ApplicationUser>
    {
        Task<List<Disease>?> GetDiseasesAsync(string Email);
        Task<List<PatientRepentanceDTO>?> GetRepentanceAsync(string email);
    }
}
