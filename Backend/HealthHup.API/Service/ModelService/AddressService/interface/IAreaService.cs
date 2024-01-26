namespace HealthHup.API.Service.ModelService.AddressService.@interface
{
    public interface IAreaService:IBaseService<Area>
    {
        Task<IList<Area>> GetAreasWithGoverment(string goverment);
    }
}
