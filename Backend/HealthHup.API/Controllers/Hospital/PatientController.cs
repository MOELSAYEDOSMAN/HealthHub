using HealthHup.API.Model.Extion.Hospital.RateModelDto;
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
        private readonly IPatientInfoService _patientInfoService;
        private readonly IRateService _rateService;
        public PatientController(IPatientDatesService patientDatesService, IPatientInfoService patientInfoService, IRateService rateService)
        {
            _patientDatesService = patientDatesService;
            _patientInfoService = patientInfoService;
            _rateService = rateService;

        }
        //Info
        [HttpGet("Information/Repentances"), Authorize]
        public async Task<IActionResult> GetRepentances()
            => Ok(await _patientInfoService.GetRepentanceAsync(User.FindFirstValue(ClaimTypes.Email)));

        [HttpGet("Information/Diseases"), Authorize]
        public async Task<IActionResult> GetDiseases()
            => Ok(await _patientInfoService.GetDiseasesAsync(User.FindFirstValue(ClaimTypes.Email)));




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


        //Rate
        [HttpPost("Rate/PushRate"),Authorize]
        public async Task<IActionResult> PushRateToDoctor([Required,FromQuery]Guid DoctorID,RateDTO input)
        {
            if(!ModelState.IsValid)
            {
                string Error = string.Empty;
                foreach (var i in ModelState.Values.SelectMany(x => x.Errors))
                    Error = $"{Error}Error:{i.ErrorMessage}\n";
              return  BadRequest(Error);
            }
            
            return Ok(await _rateService.PushRate(User.FindFirstValue(ClaimTypes.Email), DoctorID, input));
        }
    }
}
