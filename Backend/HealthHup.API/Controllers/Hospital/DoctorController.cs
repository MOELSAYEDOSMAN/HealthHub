using HealthHup.API.Model.Extion.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace HealthHup.API.Controllers.Hospital
{
    [Route("Hospital/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }
        //Get Doctors Active
        [HttpGet("GetDoctorsInArea"), Authorize]
        public async Task<IActionResult> GetDoctorsInArea([FromQuery]DoctorFilterInput input)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await _doctorService.GetDoctorsInArea(input, Email));
        }

        [HttpGet("GetDoctorsInGovernorate"), Authorize]
        public async Task<IActionResult> GetDoctorsInGovernorate([FromQuery] DoctorFilterInput input)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await _doctorService.GetDoctorsInGove(input, Email));
        }
        //Get Doctors Not Active
        [HttpGet("GetDoctorsNotActive"), Authorize("Admin,CustomerService")]
        public async Task<IActionResult> GetDoctorsNotActive(uint index=0)
            =>Ok(await _doctorService.GetDoctorsNotActiveAsync((int)index));
        
        //Add Doctor
        [HttpPost("AddDoctor"),Authorize]
        public async Task<IActionResult> AddDoctor(List<IFormFile> Certificates, IFormCollection input)
        {
            try
            {
                InputDoctor inputDoctor = JsonConvert.DeserializeObject<InputDoctor>(input["input"]);
                var Email = User.FindFirstValue(ClaimTypes.Email);
                return await AddDoctorAsync(Certificates, inputDoctor, Email);
            }
            catch
            {
                return BadRequest(new InputDoctor() { error = true, message = "No Data Send" });
            }
        }
        private async Task<IActionResult> AddDoctorAsync(List<IFormFile> Certificates,InputDoctor input,string Email)
        {
            if(!ModelState.IsValid)
            {
                string Error = string.Empty;
                foreach (var error in ModelState.Values.SelectMany(x => x.Errors))
                    Error = $"{Error} \n{error}";
                return Ok(
                    new InputDoctor() { error = true, message = Error }
                    );
            }
            return Ok(await _doctorService.AddDoctorAsync(input,Email,Certificates));
        }
        
        //Active Doctor
        [HttpPut("ActionDoctor"), Authorize("Admin,CustomerService")]
        public async Task<IActionResult> ActionDoctor(Guid Id, bool action)
            => Ok(await _doctorService.ActionDoctorAsync(Id,action));
       
        
        //Find Doctor
        [HttpGet("Get Doctor")]
        public async Task<IActionResult> GetDoctor(Guid Id)
            => Ok(await _doctorService.GetDoctorAsync(Id));
    }
}
