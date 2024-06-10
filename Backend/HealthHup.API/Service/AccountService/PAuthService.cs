namespace HealthHup.API.Service.AccountService
{
    public partial class AuthService
    {
        public async Task BlockUser(ApplicationUser user)
        {
            user.LockoutEnabled = false;
            await _userManager.UpdateAsync(user);
        }
        public async Task<ApplicationUser?> GetWithId(string Id)
        {
            return await _userManager.FindByIdAsync(Id);
        }

        public async Task<string> ReConfirm(string Email)
        {
            var User=await GetUserAsync(Email);
            if (User == null)
                return "is No Email ,You Need To Register";
            if (User.EmailConfirmed)
                return "Email Is Confirmed";
            await _messageService.ConfirmAccount(Email, $"{_env.HttpContext.Request.Scheme}://{_env.HttpContext.Request.Host}/Auth/ConfiermMail?email={Email}&token={await _userManager.GenerateEmailConfirmationTokenAsync(User)}");
            return "Cheack Email";
        }
    }
}
