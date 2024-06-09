namespace HealthHup.API.Service.ModelService.HospitalService.DrugModelService
{
    public interface IDrugModelApiService:IBaseService<Drug>
    {
        Task<InteractivitiyDto> CheackListDrugs(string PatientEmail, List<Guid> DrugIds);
    }
}
