using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthHup.API.Controllers.Admin
{
    [Route("Admin/BackGroundJob")]
    [Authorize(Roles ="Admin")]
    [ApiController]
    public class HangfireController : ControllerBase
    {
        private readonly IPatientDatesService _PatientDatesService;
        private readonly IBackgroundJobClient _BackgroundJobClient;
        public HangfireController(IPatientDatesService patientDatesService)
        {
            _PatientDatesService = patientDatesService;
        }

        [HttpDelete("OldDates")]
        public async Task<IActionResult> DeleteOldDates()
        {            
            RecurringJob.AddOrUpdate(()=> DeleteOldDatesFunc(), Cron.Daily);
            return Ok(true);
        }
        [NonAction]
        public void DeleteOldDatesFunc()
        {
            _PatientDatesService.RemoveOldDateAsync().Wait();
        }
    }
}
