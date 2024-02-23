using HealthHup.API.Model.Extion.Account;
using HealthHup.API.Service.AccountService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
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
                    Error = $"{Error}Error:{i.ErrorMessage}\n";
                return BadRequest(new OUser()
                {
                    Error = true, IsLogin = false, Message = Error
                });
            }
            return Ok(ChangeSrcImage(await _authService.LoginAsync(input)));
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(IFormFile? Img,[Required] IFormCollection input)
        {
            try
            {

                InputRegister UsrInput = JsonConvert.DeserializeObject<InputRegister>(input["input"]);
                if (Img != null)
                    UsrInput.img = Img;
                return Ok(await Register(UsrInput));
            }
            catch
            {
                return Ok(new OUser()
                { Message = "Error in Object Input Or No Data",Error = true,IsLogin = false});
            }
        }
        
        private async Task<OUser> Register(InputRegister input)
        {
            ModelState.ClearValidationState(nameof(input));
            if (!TryValidateModel(input, nameof(input)))
            {
                string Error = string.Empty;
                foreach (var i in ModelState.Values.SelectMany(x => x.Errors))
                    Error = $"{Error}Error:{i.ErrorMessage}\n";
                return  new OUser()
                { Error = true, IsLogin = false, Message = Error };
            }
            var result = ChangeSrcImage(await _authService.RegisterAsync(input, input?.img));
            return result??new OUser();
        }


        [HttpPut("AddRole"), Authorize(Roles = "Admin,CustomerService")]
        public async Task<IActionResult> AddRoles(string Email, string Role)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Role))
                return BadRequest("Must Enter Email And Role");
            return Ok(await _authService.AddRoleAsync(Email, Role));
        }


        [HttpPut("RemoveRole"), Authorize(Roles = "Admin,CustomerService")]
        public async Task<IActionResult> RemoveRole(string Email, string Role)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Role))
                return BadRequest("Must Enter Email And Role");
           return Ok(await _authService.RemoveRoleAsync(Email, Role));
        }

        [HttpPut("ChangePassword"),Authorize]
        public async Task<IActionResult> ChangePassword(string OldPassword,string NewPassowrd)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            if (Email == null)
                return Ok("Must Login Again");
            if (string.IsNullOrEmpty(OldPassword) || string.IsNullOrEmpty(NewPassowrd))
                return Ok("Must Enter OldPassword And New Password");
                return Ok(await _authService.ChangePasswordAsync(Email, OldPassword, NewPassowrd));
        }

        [HttpPut("ForgetPassword"),Authorize]
        public async Task<IActionResult> ForgetPassword(string NewPassword)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            if (Email == null)
                return Ok("Must Login Again");
            if (string.IsNullOrEmpty(NewPassword))
                return Ok("Must Enter NewPassword");
            return Ok(await _authService.ForgetPasswordAsync(Email,NewPassword));
        }


        [HttpPut("ChangePhoto"),Authorize]
        public async Task<IActionResult> ChangePhoto(IFormFile img)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            if (Email == null)
                return Ok("Must Login Again");
            if (img == null)
                return Ok(false);
            
            return Ok(await _authService.ChaneImageUserAsync(Email, img)) ;
        }
        //Private Function
        private OUser? ChangeSrcImage(OUser? input)
        {
            if(input?.ImgSrc!=null)
                input.ImgSrc = $"{this.Request.Scheme}://{this.Request.Host}/Image/User/{input?.ImgSrc}";
            return input;
        }
    }
}
