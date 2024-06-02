namespace HealthHup.API.Service.ModelService.Admin.Logs
{
    public interface IAdminLogs
    {
        public Task AddAdmin(string AdminEmail,string UserEmail);
        public Task RemoveAdmin(string AdminEmail, string UserEmail);
        public Task AddDoctor(string AdminEmail, string UserEmail);
        public Task RemoveDoctor(string AdminEmail, string UserEmail);
        public Task<IList<LogAdminAction>> GetAdminActions(string Email);
    }
}
