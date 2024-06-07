namespace HealthHup.API.Service.ModelService.HospitalService.Hostpital_doctor_Service
{
    public interface IDoctorService:IBaseService<Doctor>
    {
        Task<ListOutPutDoctors> GetDoctorsNotActiveAsync(int index);
        Task<InputDoctor> AddDoctorAsync(InputDoctor input, string Email, List<IFormFile> Certificates);
        Task<bool> ActionDoctorAsync(Guid Id, bool Accespt, string adminEmail);
        Task<ODoctor?> GetDoctorAsync(Guid Id);
        Task<ApplicationUser?> GetDoctorMainModel(Guid ID);
        Task<string> GetDoctorSpecialtie(string email);
        Task<bool> ChangePriceSession(string Email, decimal price);
        Task<ListOutPutDoctors> GetDoctorsInArea(DoctorFilterInput input,string Email);
        Task<ListOutPutDoctors> GetDoctorsInGove(DoctorFilterInput input,string Email);
        Task<ListOutPutDoctors> SerchDoctorWithName(string Name, DoctorFilterInput input, string email);
        Task<List<DoctorDate>> GetDoctorDatesAsync(string email);
        Task<string> AddAppointmentBookAsync(List<DoctorDate> Dates, string email);
        Task<string> EditAppointmentBookAsync(DoctorDate Dates, string DayNameOld, string email);
        Task<string> ReoveAppointmentBookAsync(string DayNameDel, string email);
        Task<int> PatientCountAsync(string Email);
        Task<double> PatientPercentageAsync(string email);
    }
}
