using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;

namespace HealthHup.API.Controllers.Hospital
{
    [Route("Hospital/Doctor/[controller]")]
    [ApiController]
    public class MedicalSessionController : ControllerBase
    {
        private readonly IMedicalSessionService _sessionService;
        public MedicalSessionController(IMedicalSessionService sessionService)
        {
            _sessionService = sessionService;
        }
        //Get
        [HttpGet("GetMedicalSessionWithDoctor"), Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetMedicalSessionWithDoctor([Required] string PaientEmail)
        {
            var DoctorEmail = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await _sessionService.GetMedicalSessionsWithPatientAsync(DoctorEmail, PaientEmail));
        }
        [HttpGet("GetDoctorSession"), Authorize]
        public async Task<IActionResult> GetDoctorSession([Required] Guid DoctorId)
        {
            var PaientEmail = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await _sessionService.GetMedicalSessionsWithDoctorAsync(PaientEmail, DoctorId));
        }
        [HttpGet("GetSessionDisess"), Authorize]
        public async Task<IActionResult> GetSessionDisease(string? PaientEmail, [Required] string DiseaseName)
        {
            if (string.IsNullOrEmpty(PaientEmail))
                PaientEmail = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await _sessionService.GetMedicalSessionsWithDiseaseAsync(PaientEmail, DiseaseName));
        }
        //Post
        [HttpPost,Authorize(Roles ="Doctor")]
        public async Task<IActionResult> PushMedicalService(MedicalSessionDTO input)
        {
            if (!ModelState.IsValid)
            {
                string Error = "";
                ModelState.Values.SelectMany(e => e.Errors).ToList().ForEach(e =>
                {
                    Error = $"{Error}{e.ErrorMessage}\n";
                });
                return Ok(Error);
            }
            return Ok(await _sessionService.AddMedicalSessionAsync(input,User.FindFirstValue(ClaimTypes.Email)));
        }
        //Put
        [HttpPut("PushNewMedical"),Authorize(Roles ="Doctor")]
        public async Task<IActionResult> PushNewMedical([Required,EmailAddress,FromQuery]string PaientEmail, List<RepentanceDto> newRepentances)
        {
            if (!ModelState.IsValid)
            {
                StringBuilder Error = new StringBuilder();
                foreach (var E in ModelState.Values.SelectMany(e => e.Errors))
                {
                    Error.AppendLine(E.ErrorMessage);
                }
                return BadRequest(Error);
            }
            var DoctorEmail = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await _sessionService.AddMedicalSessionNewDrugsAsync(DoctorEmail,PaientEmail, newRepentances));
        }
        [HttpDelete("RemoveMedicals"),Authorize(Roles ="Doctor")]
        public async Task<IActionResult> RemoveMedicals([Required, EmailAddress, FromQuery]string PaientEmail, [Required]List<string> Medicals)
        {
            var DoctorEmail = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await _sessionService.RemoveMedicalSessionNewDrugsAsync(DoctorEmail,PaientEmail, Medicals));
        }
    }
}
