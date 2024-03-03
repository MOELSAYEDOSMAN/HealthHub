using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace HealthHup.API.Controllers.Hospital
{
    [Route("Hospital/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientDatesService _patientDatesService;
        public PatientController(IPatientDatesService patientDatesService)
        {
            _patientDatesService = patientDatesService;
        }





        //Patient Date
        //DoctorDatesPatient
        [HttpGet("GetDoctorDates"), Authorize]
        public async Task<IActionResult> GetDoctorDates(Guid DoctorId)
        => Ok(await _patientDatesService.GetDoctorDatesAsync(DoctorId, null));
        [HttpGet("GetPatientDates"), Authorize]
        public async Task<IActionResult> GetPatientDates()
            => Ok(await _patientDatesService.GetPatientDates(User.FindFirstValue(ClaimTypes.Email)));


        [HttpPost("PushDateDoctor"),Authorize]
        public async Task<IActionResult> PushDateDoctor([FromQuery,Required]Guid DoctorId, [FromBody] PatientDateInput input )
        {
            if(!ModelState.IsValid)
            {
                string Error = string.Empty;
                ModelState.Values.SelectMany(v => v.Errors).ToList().ForEach(e => Error = $"{Error} {e.ErrorMessage}\n");
                return Ok(Error);
            }
            return Ok(await _patientDatesService.PushDateAsync(input, DoctorId, User.FindFirstValue(ClaimTypes.Email)));
        }
        [HttpPut("UpdateBookedDate"),Authorize]
        public async Task<IActionResult> UpdateBookedDate([FromQuery,Required]Guid PaintDateid, [FromBody] PatientDateInput input)
        {
            if (!ModelState.IsValid)
            {
                string Error = string.Empty;
                ModelState.Values.SelectMany(v => v.Errors).ToList().ForEach(e => Error = $"{Error} {e.ErrorMessage}\n");
                return Ok(Error);
            }
            return Ok(await _patientDatesService.UpdateDateAsync(input, PaintDateid, User.FindFirstValue(ClaimTypes.Email)));
        }
        
        [HttpDelete("CancelBookedDate"),Authorize]
        public async Task<IActionResult> CancelBookedDate([FromQuery,Required]Guid PaintDateid)
        => Ok(await _patientDatesService.CancleDateAsync(PaintDateid, User.FindFirstValue(ClaimTypes.Email)));
    }
}
