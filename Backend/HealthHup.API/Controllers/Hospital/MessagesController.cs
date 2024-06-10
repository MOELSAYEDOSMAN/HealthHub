using HealthHup.API.Service.AccountService;
using HealthHup.API.Service.ModelService.HospitalService.Chat;
using HealthHup.API.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace HealthHup.API.Controllers.Hospital
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IChatService _messageService;
        
        public MessagesController(IChatService messageService)
        {
            _messageService=messageService;
        }

        [HttpGet("Chat"),Authorize]
        public async Task<IActionResult> Chat([SerchValidation,Required,EmailAddress] string Email)
        {
            return Ok(await _messageService.GetMessages(User.FindFirstValue(ClaimTypes.Email),Email));
        }

        [HttpGet("UserChat"), Authorize]
        public async Task<IActionResult> UserChat()
        {
            return Ok(await _messageService.GetUserChat(User.FindFirstValue(ClaimTypes.Email)));
        }
    }
}
