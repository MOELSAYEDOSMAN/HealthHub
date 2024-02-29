namespace HealthHup.API.Service.ModelService.PatientModelService.PatientDatesModelService
{
    public interface IPatientDatesService:IBaseService<PatientDates>
    {
        Task<List<PatientDateDTO>> GetDoctorDatesAsync(Guid? Id, string? email);
        Task<string> PushDateAsync(PatientDateInput input, Guid DrId, string Email);
        Task<bool> CancleDateAsync(Guid PaintDateid, string Email);
        Task RemoveOldDateAsync();
        Task<string> UpdateDateAsync(PatientDateInput input, Guid DateId, string Email);
    }
}
