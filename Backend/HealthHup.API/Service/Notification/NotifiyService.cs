using HealthHup.API.Model.Extion;
using HealthHup.API.Model.Models.Notifaction;
using HealthHup.API.Service.AccountService;

namespace HealthHup.API.Service.Notification
{
    public class NotifiyService:INotifiyService
    {
        private readonly NotifiyBase _notifiyBase;
        public NotifiyService(ApplicatoinDataBaseContext db,IAuthService auth)
        {
            _notifiyBase=new NotifiyBase(db,auth);
        }

        public async Task DoctorChangeYourTime(ApplicationUser user,string Message)
        {
            await _notifiyBase.AddNotify(user, NotifyHeader.DoctorChangeYourTime, Message,null);
        }
        public async Task DoctorSelectDate(ApplicationUser user, string Message)
        {
            await _notifiyBase.AddNotify(user, NotifyHeader.DoctorSelectDate, Message, null);
        }
        public async Task DoctorCancelYourTime(ApplicationUser user, string Message)
        {
            await _notifiyBase.AddNotify(user, NotifyHeader.DoctorCancelYourTime, Message, null);
        }
        public async Task DoctorAddNewDayInSchedule(ApplicationUser user, string Message)
        {
            await _notifiyBase.AddNotify(user, NotifyHeader.DoctorAddNewDayInSchedule, Message, null);
        }
        public async Task DoctorRemoveDayFromSchedule(ApplicationUser user, string Message)
        {
            await _notifiyBase.AddNotify(user, NotifyHeader.DoctorRemoveDayFromSchedule, Message, null);
        }
        public async Task RateDoctor(ApplicationUser user, string Message)
        {
            await _notifiyBase.AddNotify(user, NotifyHeader.RateDoctor, Message, null);
        }
        public async Task<IEnumerable<notfiyDTO>> GetUserNotificationsAsync(string Email)
            => await _notifiyBase.GetNotificationUser(Email);
        public async Task RemoveOldNotifactions()
        {
            var oldNotifys=await _notifiyBase.GetOldNotifys();
            if (oldNotifys.Count > 0)
                await _notifiyBase.RemoveList(oldNotifys.ToList());
        }
    }
}
