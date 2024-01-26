namespace HealthHup.API.Service.ModelService.AddressService.@interface
{
    public interface IAreaService:IBaseService<Area>
    {
        Task<List<Area>> GetAreasWithGoverment(string goverment);
    }
}
