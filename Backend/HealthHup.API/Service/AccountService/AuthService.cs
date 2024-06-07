using HealthHup.API.Model.Extion.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HealthHup.API.Service.AccountService
{
    public partial class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBaseService<Doctor> _doctorService;
        private readonly ISaveImage _SvImg;
        private readonly IAreaService _areaService;
        private readonly Jwt _jwt;
        private readonly IHttpContextAccessor _env;
        private readonly ISendMessageService _messageService;
        public AuthService(UserManager<ApplicationUser> userManager, IOptions<Jwt> jwt,ISaveImage saveimg,IAreaService areaService
            , IBaseService<Doctor> doctorService, IHttpContextAccessor env,
            ISendMessageService messageService)
        {
            _userManager = userManager;
            _SvImg = saveimg;
            _jwt = jwt.Value;
            _areaService = areaService;
            _doctorService = doctorService;
            _env = env;
            _messageService = messageService;
        }
        //post
        public async Task<OUser> LoginAsync(InputLogin input)
        {
            //Cheack Email Or UserName
            string UserName = new EmailAddressAttribute().IsValid(input.userName) ? new MailAddress(input.userName).User : input.userName;
            //Cheack IF User In DataBase
            var UserLogin=await _userManager.FindByNameAsync(UserName);
            if (UserLogin == null||!await _userManager.CheckPasswordAsync(UserLogin, input.password))
                return new OUser()
                {Error=true,IsLogin=false,Message="Email Or Password Incorrect"};

            if (!UserLogin.LockoutEnabled)
                return new OUser()
                { Error = true, IsLogin = false, Message = "Yor are blocked" };

            if(!UserLogin.EmailConfirmed)
                return new OUser()
                { Error = true, IsLogin = false, Message = "need Confirme Mail" };
            //Ready login
            var jwtSecurityToken = await CreateJwtTokenAsync(UserLogin);//Create Token
            var rolesList = await _userManager.GetRolesAsync(UserLogin);//Get Roles
            var area =await _areaService.findAsync(a => a.Id == UserLogin.AreaId, new string[] { "governorate" });
            return new()
            {
                Error=false,
                ImgSrc=UserLogin?.src??string.Empty,
                IsLogin=true,
                Email=UserLogin?.Email?? string.Empty,
                Roles=rolesList.ToArray(),
                UserName=UserName,
                Token=new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                ExToken = jwtSecurityToken.ValidTo,
                Gender=UserLogin.Gender,
                area=area.key,
                gove=area.governorate.key
            };
        }
        public async Task<OUser> RegisterAsync(InputRegister input, IFormFile? img = null)
        {
            //Cheack Email Or UserName
            string UserName = new EmailAddressAttribute().IsValid(input.email) ? new MailAddress(input.email).User : input.email;
            
            //Cheack If Main In DataBase
            if (await _userManager.FindByNameAsync(UserName.Replace(".",string.Empty)) != null)
                return new()
                { Error = true, IsLogin = false, Message = "The Email Is Registered" };
            //Cheack Area
            var UserArea = await _areaService.findAsync(x => x.key == input.area);
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
           
            await _messageService.ConfirmAccount(input.email, $"{_env.HttpContext.Request.Scheme}://{_env.HttpContext.Request.Host}/Auth/ConfiermMail?email={input.email}&token={await _userManager.GenerateEmailConfirmationTokenAsync(UserRegister)}");
            return new() { Message = $"Cheack Mail to Confirem Mail", IsLogin = false,Error=false};
        }
        
        public async Task<bool> ConfiermMail(string token,string email)
        {
            var User = await GetUserAsync(email);
            //Cheack IF User In DB
            if (User == null)
                return false;
 
            //Cheack IF Confirmed
            if (User.EmailConfirmed)
                return false;


            //Confirm
            var result = await _userManager.ConfirmEmailAsync(User, token.Replace(' ','+'));
            if (!result.Succeeded)
                return false;

 

            return true;

        }
        // put
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
        //get
        public async Task<ApplicationUser?> GetUserAsync(string Email)
        {
            var User=await _userManager.FindByEmailAsync(Email);
            return User;
        }
        public async Task<DTOUserInformation>? GetUserWithEmailAsync (string Email)
        {
            var user=await _userManager.FindByEmailAsync(Email);
            if (user == null)
                return null;
            var area = await _areaService.findAsync(x => x.Id == user.AreaId, new string[] { "governorate" });
            var DtoUser = new DTOUserInformation();
            DtoUser = user;
            DtoUser.area = area?.key;
            DtoUser.gove = area?.governorate?.key;
            return DtoUser;
        }
        public async Task<OUser> CheackDoctorRoleAsync(string Email)
        {
            var User = await GetUserAsync(Email);
            //Cheack If User In System
            if (User == null)
                return new()
                {
                    Error=true,
                    Message="You need Login"
                };
            //Cheack Roles
            var Role=await _userManager.GetRolesAsync(User);
            if(Role.Count(r=>r=="Doctor")==0)
            {
                var Doctor = await _doctorService.findAsync(d => d.doctorId == User.Id);
                if (Doctor == null)
                    return new()
                    {
                        Error = true,
                        Message = "Your Request Has been Rejected,You Can Contact A Customer Service Or Re-Upload The Order"
                    };
                else if (!Doctor.Accept)
                    return new()
                    {
                        Error = true,
                        Message = "Your Request Has Not Yet been Reviewed"
                    };
            }
            //Create New Token
            var jwtSecurityToken = await CreateJwtTokenAsync(User);//Create Token
            return new()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                ExToken = jwtSecurityToken.ValidTo,
                Message = "Accept",
                Error = false,
                Roles=Role.ToArray(),
            };
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
                new Claim(JwtRegisteredClaimNames.Sub, input.UserName?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, input.Email??""),
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
