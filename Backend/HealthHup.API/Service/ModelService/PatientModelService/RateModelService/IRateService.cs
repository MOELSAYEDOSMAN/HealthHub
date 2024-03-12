using HealthHup.API.Model.Extion.Hospital.RateModelDto;

namespace HealthHup.API.Service.ModelService.PatientModelService.RateModelService
{
    public interface IRateService:IBaseService<Review>
    {
        //post
        Task<string> PushRate(string PatientEmail, Guid DoctorId, RateDTO input);
    }
}
