using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
namespace HealthHup.API.Controllers.Hospital
{
    [Route("Hospital/[controller]")]
    [ApiController]
    public class DrugController : ControllerBase
    {
        private readonly IBaseService<Drug> _drugService;
        public DrugController(IBaseService<Drug> drugService)
        {
            _drugService = drugService;
        }
        [HttpPost,Authorize(Roles = "Admin,CustomerService")]
        public async Task<IActionResult> Post([Required] Drug input)
        {
            if(!ModelState.IsValid)
                return BadRequest(false);
           return Ok(await _drugService.AddAsync(input));
        }
        [HttpGet("SerchByName")]
        public async Task<IActionResult> GetDrugByName([Required] string name)
            => Ok(await _drugService.findByAsync( d=>d.name.ToUpper().Contains(name.ToUpper()) )  );
    }
}
