using HealthHup.API.Service.AccountService;

namespace HealthHup.API.Service.ModelService.Admin.Logs
{
    public class AdminLogs:BaseService<LogAdminAction>
    {
        readonly IAuthService _authService;
        public AdminLogs(ApplicatoinDataBaseContext db, IAuthService authService) :base(db)
        {
            _authService = authService;
        }

        public async Task AddLogs(string EmailAdmin,string EmailUser, AdminAction action)
        {
            var admin=await _authService.GetUserAsync(EmailAdmin);
            var User = await _authService.GetUserAsync(EmailUser);

            if (admin == null)
                return;
            LogAdminAction log;
            
            if (User == null)
                log= new LogAdminAction(admin,$"No User With This Mail {EmailUser}",action);
            else
                log=new LogAdminAction(admin,User,action);

            await AddAsync(log);
        }

        public async Task<IList<LogAdminAction>> GetLogs(string email)
        {
            var Admin=await _authService.GetUserAsync(email);
            return await findByAsync(l=>l.AdminId==Admin.Id);
        }
    }
}
