using HealthHup.API.Model.Extion.Account;
using HealthHup.API.Service.AccountService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace HealthHup.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(InputLogin input)
        {
            if (!ModelState.IsValid)
            {
                string Error = string.Empty;
                foreach (var i in ModelState.Values.SelectMany(x => x.Errors))
                    Error = $"{Error}\nError:{i.ErrorMessage}";
                return BadRequest(new OUser()
                {
                    Error = true, IsLogin = false, Message = Error
                });
            }
            return Ok(ChangeSrcImage(await _authService.LoginAsync(input)));
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(IFormFile? Img, IFormCollection input)
        {
            InputRegister UsrInput = JsonConvert.DeserializeObject<InputRegister>(input["input"]);
            return Ok(await register(UsrInput, Img));
        }
        private async Task<OUser> register(InputRegister input, IFormFile? img = null)
        {
            if (!ModelState.IsValid)
            {
                string Error = string.Empty;
                foreach (var i in ModelState.Values.SelectMany(x => x.Errors))
                    Error = $"{Error}\nError:{i.ErrorMessage}";
                return new OUser()
                { Error = true, IsLogin = false, Message = Error };
            }
            return ChangeSrcImage(await _authService.RegisterAsync(input, img));
        }


        [HttpGet("AddRole"), Authorize(Roles = "Admin,CustomerService")]
        public async Task<IActionResult> AddRoles(string Email, string Role)
            => Ok(await _authService.AddRoleAsync(Email, Role));


        [HttpGet("RemoveRole"), Authorize(Roles = "Admin,CustomerService")]
        public async Task<IActionResult> RemoveRole(string Email, string Role)
            => Ok(await _authService.RemoveRoleAsync(Email, Role));

        [HttpGet("ChangePassword"),Authorize]
        public async Task<IActionResult> ChangePassword(string OldPassword,string NewPassowrd)
        {
            string Email = ClaimTypes.Email;
            return Ok(await _authService.ChangePasswordAsync(Email, OldPassword, NewPassowrd));
        }

        [HttpGet("ForgetPassword"),Authorize]
        public async Task<IActionResult> ForgetPassword(string NewPassword)
        {
            string Email = ClaimTypes.Email;
            return Ok(await _authService.ForgetPasswordAsync(Email,NewPassword));
        }


        [HttpPost("ChangePhoto"),Authorize]
        public async Task<IActionResult> ChangePhoto(IFormFile img)
        {
            return Ok( await _authService.ChaneImageUserAsync(ClaimTypes.Email,img));
        }
        //Private Function
        private OUser ChangeSrcImage(OUser? input)
        {
            if(input?.ImgSrc==string.Empty)
                input.ImgSrc = $"{this.Request.Scheme}://{this.Request.Host}/Image/User/{input?.ImgSrc}";

            return input;
        }
    }
}
