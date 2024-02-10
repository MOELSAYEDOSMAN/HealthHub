﻿namespace HealthHup.API.Service.ModelService.HospitalService.Hostpital_doctor_Service
{
    public interface IDoctorService:IBaseService<Doctor>
    {
        Task<ListOutPutDoctors> GetDoctorsNotActiveAsync(int index);
        Task<InputDoctor> AddDoctorAsync(InputDoctor input, string Email, List<IFormFile> Certificates);
        Task<bool> ActionDoctorAsync(Guid Id, bool Accespt);
        Task<ODoctor>? GetDoctorAsync(Guid Id);
        Task<ListOutPutDoctors> GetDoctorsInArea(DoctorFilterInput input,string Email);
        Task<ListOutPutDoctors> GetDoctorsInGove(DoctorFilterInput input,string Email);
    }
}
