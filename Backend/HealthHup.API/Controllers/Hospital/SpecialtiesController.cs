using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthHup.API.Controllers.Hospital
{
    [Route("[controller]")]
    [ApiController]
    public class SpecialtiesController : ControllerBase
    {
        private readonly IBaseService<Specialtie> _SpecialtieService;
        public SpecialtiesController(IBaseService<Specialtie> SpecialtieService)
        {
            _SpecialtieService = SpecialtieService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
            => Ok(await _SpecialtieService.GetAll());

        [HttpPost]
        public async Task<IActionResult> Post(string SpecialtieName)
        {
            if (string.IsNullOrEmpty(SpecialtieName) || await _SpecialtieService.find(s =>s.Name.ToLower()==SpecialtieName.ToLower()) != null)
                return BadRequest(false);

            await _SpecialtieService.AddAsync(new()
            {
                Id=Guid.NewGuid(),
                Name=SpecialtieName
            });
            
            return Ok(true);
                
        }
    }
}
