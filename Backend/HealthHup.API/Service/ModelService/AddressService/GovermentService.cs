namespace HealthHup.API.Service.ModelService.AddressService
{
    public class GovermentService:BaseService<Governorate>, IGovermentService
    {
        public GovermentService(ApplicatoinDataBaseContext _db):base(_db)
        {
        }
    }
}
