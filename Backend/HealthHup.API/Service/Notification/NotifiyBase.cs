using HealthHup.API.Model.Extion;
using HealthHup.API.Model.Models.Notifaction;
using HealthHup.API.Service.AccountService;

namespace HealthHup.API.Service.Notification
{
    public class NotifiyBase:BaseService<Notify>
    {
        private readonly IAuthService _authService;
        public NotifiyBase(ApplicatoinDataBaseContext db, IAuthService authService) :base(db)
        {
            _authService= authService;
        }

        public async Task AddNotify(ApplicationUser user,NotifyHeader notifyHeader, string Message,string?Link)
        {
            await AddAsync(new()
            {
                DateTime=DateTime.UtcNow,
                User=user,
                Id=Guid.NewGuid(),
                link= Link??string.Empty,
                Message=Message,
                notifyHeader= notifyHeader,
            });
        }

        public async Task<IEnumerable<notfiyDTO>> GetNotificationUser(string Email)
        {
            var User=await _authService.GetUserAsync(Email);
            return notfiyDTO.notfiyDTOs(await findByAsync(n=>n.UserId==User.Id));
        }
        public async Task RemoveList(List<Notify> input)
        {
            await RemoveRangeAsync(input);
        }
        public async Task<IList<Notify>> GetOldNotifys()
            => await findByAsync(n=>n.DateTime.Date<DateTime.UtcNow.Date);

    }
}
