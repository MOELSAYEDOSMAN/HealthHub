using HealthHup.API.Service.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthHup.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController,Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotifiyService _notifiyService;
        public NotificationController(INotifiyService notifiyService)
        {
            _notifiyService=notifiyService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _notifiyService.GetUserNotificationsAsync(User.FindFirstValue(ClaimTypes.Email)));
        }
    }
}
