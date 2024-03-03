namespace HealthHup.API.Service.ModelService.PatientModelService.MedicalSessionModelService
{
    public interface IMedicalSessionService:IBaseService<MedicalSession>
    {
        Task<string> AddMedicalSessionAsync(MedicalSessionDTO input, string EmailDoctor);
    }
}
