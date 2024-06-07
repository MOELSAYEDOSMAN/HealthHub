namespace HealthHup.API.Service.ModelService.PatientModelService.MedicalSessionModelService
{
    public interface IMedicalSessionService:IBaseService<MedicalSession>
    {
        //Post
        Task<string> AddMedicalSessionAsync(MedicalSessionDTO input, string EmailDoctor);
        //Get
        Task<OMedical?> GetWithId(Guid Id, string email);
        Task<List<OutMedicalList>?> GetMedicalSessionsWithDoctorAsync(string email, Guid DoctorId);
        Task<List<OutMedicalList>?> GetMedicalSessionsWithPatientAsync(string emailDoctor, string emailPatient);
        Task<List<OutMedicalList>?> GetMedicalSessionsWithDiseaseAsync(string Email, string Disease);
        Task<List<MedicalSession>?> GetMedicalSessionsWithDiseaseCuredAsync(string Id, string Disease);
        //Put
        Task<string> RemoveMedicalSessionNewDrugsAsync(string emailDoctor, string emailPaient, List<string> RemovedRepentances);
        Task<string> AddMedicalSessionNewDrugsAsync(string emailDoctor, string emailPaient, List<RepentanceDto> newRepentances);
    }
}
