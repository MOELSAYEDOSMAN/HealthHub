using HealthHup.API.Model.Extion.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace HealthHup.API.Service.AccountService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISaveImage _SvImg;
        private readonly IAreaService _areaService;
        private readonly jwt _jwt;
        public AuthService(UserManager<ApplicationUser> userManager, IOptions<jwt> jwt,ISaveImage saveimg,IAreaService areaService)
        {
            _userManager = userManager;
            _SvImg = saveimg;
            _jwt = jwt.Value;
            _areaService = areaService;
        }
        public async Task<OUser> LoginAsync(InputLogin input)
        {
            //Cheack Email Or UserName
            string UserName = new EmailAddressAttribute().IsValid(input.userName) ? new MailAddress(input.userName).User : input.userName;
            //Cheack IF User In DataBase
            var UserLogin=await _userManager.FindByNameAsync(UserName);
            bool ckPass= await _userManager.CheckPasswordAsync(UserLogin, input.password);
            if (UserLogin == null||!ckPass)
                return new OUser()
                {Error=true,IsLogin=false,Message="Email Or Password Incorrect"};
            //Ready login
            var jwtSecurityToken = await CreateJwtTokenAsync(UserLogin);//Create Token
            var rolesList = await _userManager.GetRolesAsync(UserLogin);//Get Roles
            return new()
            {
                Error=false,
                ImgSrc=UserLogin.src,
                IsLogin=true,
                Email=UserLogin.Email,
                Roles=rolesList.ToArray(),
                UserName=UserName,
                Token=new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                ExToken = jwtSecurityToken.ValidTo
            };
        }
        public async Task<OUser> RegisterAsync(InputRegister input, IFormFile? img = null)
        {
            //Cheack Email Or UserName
            string UserName = new EmailAddressAttribute().IsValid(input.email) ? new MailAddress(input.email).User : input.email;

            //Cheack If Main In DataBase
            if (await _userManager.FindByNameAsync(UserName) != null)
                return new()
                { Error = true, IsLogin = false, Message = "The Email Is Registered" };
            //Cheack Area
            var UserArea = await _areaService.find(x => x.key == input.area);
            if (UserArea == null)
                return new()
                { Error = true, IsLogin = false, Message = "No Area by This Name" };
            //Save Image
            string srcImg = "";
            if (img != null)
            {
                srcImg = await _SvImg.UploadImage("User", img);
            }
            else
                srcImg = "user.png";

            ApplicationUser UserRegister = input;//Covert InputRegister To ApplicationUser
            UserRegister.src= srcImg;
            UserRegister.area = UserArea;

            var Result = await _userManager.CreateAsync(UserRegister, input.password);
            if(!Result.Succeeded)
            {
                //Delete Image User
                if (srcImg != "user.png")
                    await _SvImg.DeleteImage($"User/{srcImg}");

                StringBuilder errors = new StringBuilder();
                foreach (var i in Result.Errors)
                    errors.Append(i.Description);
                return new OUser()
                { Error=true,IsLogin=false,Message=errors.ToString()};
            }
            //Give Defulte Role
            await _userManager.AddToRoleAsync(UserRegister, "Patient");
            return new() { Message = "Go To Login Page", IsLogin = false,Error=false};
        }
        public async Task<string> AddRoleAsync(string Email, string Role)
        {
            string UserName = new EmailAddressAttribute().IsValid(Email) ? new MailAddress(Email).User : Email;
            var User =await _userManager.FindByNameAsync(UserName);
            if (User == null)
                return "No User by This Email ";

            if ((await _userManager.GetRolesAsync(User)).FirstOrDefault(x => x == Role) != null)
                return "User Have Role";

            await _userManager.AddToRoleAsync(User,Role);
            return "Success";
        }
        public async Task<string> RemoveRoleAsync(string Email, string Role)
        {
            string UserName = new EmailAddressAttribute().IsValid(Email) ? new MailAddress(Email).User : Email;
            var User = await _userManager.FindByNameAsync(UserName);
            if (User == null)
                return "No User by This Email ";

            if ((await _userManager.GetRolesAsync(User)).FirstOrDefault(x => x == Role) == null)
                return "User Role Not Contains Role";

            await _userManager.RemoveFromRoleAsync(User, Role);
            return "Success";
        }
        public async Task<bool> ChaneImageUserAsync(string Email, IFormFile img)
        {
            string UserName = new EmailAddressAttribute().IsValid(Email) ? new MailAddress(Email).User : Email;
            var User = await _userManager.FindByNameAsync(UserName);
            if (User == null)
                return false;

            if (User.src == "user.png")
            {
                User.src = await _SvImg.UploadImage("User", img);
                await _userManager.UpdateAsync(User);
            }
            else
                await _SvImg.ChangeImg($"User/{User.src}",img);
            return true;
        }
        public async Task<string> ChangePasswordAsync(string Email,string OldPassowrd,string NewPassword)
        {
            string UserName = new EmailAddressAttribute().IsValid(Email) ? new MailAddress(Email).User : Email;
            var User = await _userManager.FindByNameAsync(UserName);
            if (User == null||!await _userManager.CheckPasswordAsync(User,OldPassowrd))
                return "Error in Current Password";
            var us= await _userManager.ChangePasswordAsync(User, OldPassowrd, NewPassword);
            
            if(!us.Succeeded)
            {
                string error = string.Empty;
                foreach (var item in us.Errors)
                {
                    error = $"{error} {item.Code}:{item.Description}\n";
                }
                return error;
            }
            else
            {
                return "Success";
            }
        }
        public async Task<string> ForgetPasswordAsync(string Email,string NewPassword)
        {
            string UserName = new EmailAddressAttribute().IsValid(Email) ? new MailAddress(Email).User : Email;
            var User = await _userManager.FindByNameAsync(UserName);
            if (User == null)
                return "Error in Current User";
            var restToken = await _userManager.GeneratePasswordResetTokenAsync(User);
            var r=await _userManager.ResetPasswordAsync(User, restToken,NewPassword);
            if (!r.Succeeded)
            {
                string error = string.Empty;
                foreach (var item in r.Errors)
                {
                    error = $"{error} {item.Code}:{item.Description}\n";
                }
                return error;
            }
            else
            {
                return "Success";
            }
        }
        //Create Token
        private async Task<JwtSecurityToken> CreateJwtTokenAsync(ApplicationUser input)
        {
            var userClaims = await _userManager.GetClaimsAsync(input);
            var roles = await _userManager.GetRolesAsync(input);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, input.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, input.Email),
                new Claim("uid", input.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.IssUser,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.Expire),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }
    }
}
