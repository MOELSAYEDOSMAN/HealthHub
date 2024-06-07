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
    }
}
