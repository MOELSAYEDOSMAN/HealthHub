namespace HealthHup.API.Service.ModelService.PatientModelService.PatientGeneralInfoModelService
{
    public interface IPatientInfoService:IBaseService<ApplicationUser>
    {
        Task<List<Disease>?> GetDiseasesAsync(string Email);
        Task<IEnumerable<PatientRepentanceDTO>?> GetRepentanceAsync(string email);
        Task<List<Drug>?> GetCurrentDrugs(string email);
        Task<List<ODoctor>> GetResponsibledDoctorAsync(string Email);
    }
}
