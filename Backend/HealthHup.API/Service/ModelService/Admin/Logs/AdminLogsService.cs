
using HealthHup.API.Service.AccountService;

namespace HealthHup.API.Service.ModelService.Admin.Logs
{
    public class AdminLogsService : IAdminLogs
    {
        private readonly AdminLogs _AdminLogs;
        public AdminLogsService(ApplicatoinDataBaseContext db, IAuthService authService)
        {
            _AdminLogs=new AdminLogs(db, authService);
        }
        public async Task AddAdmin(string AdminEmail, string UserEmail)
        {
            await _AdminLogs.AddLogs(AdminEmail,UserEmail,AdminAction.AddAdmin);
        }

        public async Task AddDoctor(string AdminEmail, string UserEmail)
        {
            await _AdminLogs.AddLogs(AdminEmail, UserEmail, AdminAction.AddDoctor);
        }

        public async Task<IList<LogAdminAction>> GetAdminActions(string Email)
        {
           return  await _AdminLogs.GetLogs(Email);
        }

        public async Task RemoveAdmin(string AdminEmail, string UserEmail)
        {
            await _AdminLogs.AddLogs(AdminEmail, UserEmail, AdminAction.RemoveAdmin);
        }

        public async Task RemoveDoctor(string AdminEmail, string UserEmail)
        {
            await _AdminLogs.AddLogs(AdminEmail, UserEmail, AdminAction.BlockDoctor);
        }
    }
}
