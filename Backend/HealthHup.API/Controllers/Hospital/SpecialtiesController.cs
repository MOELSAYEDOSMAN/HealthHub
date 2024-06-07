using HealthHup.API.Service.AccountService;
using HealthHup.API.Service.ModelService.HospitalService.Hostpital_doctor_Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthHup.API.Controllers.Hospital
{
    [Route("Hospital/[controller]")]
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
            => Ok(await _SpecialtieService.GetAllAsync());
        [HttpPost,Authorize(Roles = "Admin,CustomerService")]
        public async Task<IActionResult> Post(string SpecialtieName)
        {
            if (string.IsNullOrEmpty(SpecialtieName) || await _SpecialtieService.findAsync(s =>s.Name.ToLower()==SpecialtieName.ToLower()) != null)
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
