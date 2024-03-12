using HealthHup.API.Model.Extion.Account;

namespace HealthHup.API.Service.AccountService
{
    public interface IAuthService
    {
        //post
        Task<OUser> LoginAsync(InputLogin input);
        Task<OUser> RegisterAsync(InputRegister input,IFormFile?img=null);
        //put
        Task<string> AddRoleAsync(string Email, string Role);
        Task<string> RemoveRoleAsync(string Email,string Role);
        Task<bool> ChaneImageUserAsync(string Email, IFormFile img);
        Task<string> ChangePasswordAsync(string Email, string OldPassowrd, string NewPassword);
        Task<string> ForgetPasswordAsync(string Email, string NewPassword);
        //Get
        Task<ApplicationUser?> GetUserAsync(string Email);
        Task<DTOUserInformation>? GetUserWithEmailAsync(string Email);
        Task<OUser> CheackDoctorRoleAsync(string Email);
    }
}
