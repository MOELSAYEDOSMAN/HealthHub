using HealthHup.API.Model.Extion.Account;

namespace HealthHup.API.Service.AccountService
{
    public interface IAuthService
    {
        Task<OUser> LoginAsync(InputLogin input);
        Task<OUser> RegisterAsync(InputRegister input,IFormFile?img=null);
        Task<string> AddRoleAsync(string Email, string Role);
        Task<string> RemoveRoleAsync(string Email,string Role);
        Task<bool> ChaneImageUserAsync(string Email, IFormFile img);
        Task<string> ChangePasswordAsync(string Email, string OldPassowrd, string NewPassword);
        Task<string> ForgetPasswordAsync(string Email, string NewPassword);
        Task<ApplicationUser> GetUserAsync(string Email);
        Task<DTOUserInformation>? GetUserWithEmailAsync(string Email);
    }
}
