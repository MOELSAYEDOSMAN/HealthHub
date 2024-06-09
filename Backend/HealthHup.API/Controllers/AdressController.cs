using HealthHup.API.Service.MlService.MLCT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace HealthHup.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdressController : ControllerBase
    {
        private readonly IGovermentService _GovernorateService;
        private readonly IAreaService _areaService;

        
        public AdressController(IGovermentService govermentService,IAreaService areaService)
        {
            
            _GovernorateService = govermentService;
            _areaService = areaService;
            
        }

        
        [HttpGet("GetGovernorate")]
        public async Task<IActionResult> GetGovernorates()
            =>Ok(await _GovernorateService.GetAllAsync());
        [HttpGet("GetAreas")]
        public async Task<IActionResult> GetAreas(string GovermentKey)
            => Ok(string.IsNullOrEmpty(GovermentKey)||string.IsNullOrWhiteSpace(GovermentKey)?new List<Governorate>()
                :await _areaService.GetAreasWithGoverment(GovermentKey));

        //Add
        [HttpPost("PushGovernorate"),Authorize(Roles = "Admin,CustomerService")]
        public async Task<IActionResult> PushGovernorate([Required]string GovernorateKey)
            => Ok(await _GovernorateService.AddAsync(new Governorate()
            {
                Id=Guid.NewGuid(),
                key=GovernorateKey
            }));
     
        [HttpPost("PushArea"), Authorize(Roles = "Admin,CustomerService")]
        public async Task<IActionResult> PushArea([Required]string GovernorateKey,[Required] string areaKey)
            => Ok(await _areaService.AddAsync(new Area()
            {
                Id = Guid.NewGuid(),
                key = areaKey,
                governorate=await _GovernorateService.findAsync(x=>x.key== GovernorateKey)
            }));
    }
}
