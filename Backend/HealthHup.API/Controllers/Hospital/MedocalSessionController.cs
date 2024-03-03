using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthHup.API.Controllers.Hospital
{
    [Route("Hospital/Doctor/[controller]")]
    [ApiController]
    public class MedocalSessionController : ControllerBase
    {
        private readonly IMedicalSessionService _sessionService;
        public MedocalSessionController(IMedicalSessionService sessionService)
        {
            _sessionService = sessionService;
        }

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
    }
}
