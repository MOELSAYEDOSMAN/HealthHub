
namespace HealthHup.API.Service.ModelService.AddressService
{
    public class AreaService:BaseService<Area>, IAreaService
    {
        private readonly IGovermentService _govermentService;
        public AreaService(ApplicatoinDataBaseContext db,IGovermentService govermentService) :base(db)
        {
            _govermentService = govermentService;
        }
        public async Task<IList<Area>> GetAreasWithGoverment(string town)
        =>(await _govermentService.find(t=>t.key==town,new string[] {"areas"}))?.areas;
    }
}
