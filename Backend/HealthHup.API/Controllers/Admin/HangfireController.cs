using Hangfire;
using HealthHup.API.Service.BackGroundJobs.Dates;
using HealthHup.API.Service.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthHup.API.Controllers.Admin
{
    [Route("Admin/BackGroundJob")]
    [ApiController]
    public class HangfireController : ControllerBase
    {
        private readonly IBDataPeients _PatientDatesService;
        private readonly INotifiyService _notifiyService;

        public HangfireController(IBDataPeients patientDatesService, INotifiyService notifiyService)
        {
            _PatientDatesService = patientDatesService;
            _notifiyService = notifiyService;

        }

        [HttpDelete("OldDates")]
        public async Task<IActionResult> DeleteOldDates()
        {            
            RecurringJob.AddOrUpdate(()=> DeleteOldDatesFunc(), Cron.Daily(hour:0,minute:0));
            return Ok(true);
        }
        [HttpDelete("OldDisease")]
        public async Task<IActionResult> DeleteOldDisease()
        {
            RecurringJob.AddOrUpdate(() => DeleteOldDiseaseFunc(), Cron.Monthly(day:15));
            return Ok(true);
        }
        [HttpDelete("OldNotify")]
        public async Task<IActionResult> DeleteOldNotify()
        {
            RecurringJob.AddOrUpdate(() => DeleteOldNotifyFunc(), Cron.Daily(hour: 0, minute: 0));
            return Ok(true);
        }
        [NonAction]
        public void DeleteOldDatesFunc()
        {
            _PatientDatesService.DeleteOldDates().Wait();
        }
        [NonAction]
        public void DeleteOldDiseaseFunc()
        {
            _PatientDatesService.DeleteOldDisease().Wait();
        }
        [NonAction]
        public void DeleteOldNotifyFunc()
        {
            _notifiyService.RemoveOldNotifactions().Wait();
        }
    }
}
