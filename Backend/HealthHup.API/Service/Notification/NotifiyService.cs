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
        public async Task DoctorAction(ApplicationUser user, string Message)
        {
            await _notifiyBase.AddNotify(user, NotifyHeader.Doctor_Result, Message, string.Empty);
        }
        public async Task AlertDate(ApplicationUser user ,string Message)
        {
            await _notifiyBase.AddNotify(user, NotifyHeader.Alert_Date, Message,string.Empty);
        }
        public async Task DoctorChangeYourTime(ApplicationUser user,string Message)
        {
            await _notifiyBase.AddNotify(user, NotifyHeader.Doctor_Change_Your_Time, Message,null);
        }
        public async Task DoctorSelectDate(ApplicationUser user, string Message)
        {
            await _notifiyBase.AddNotify(user, NotifyHeader.Doctor_Select_Date, Message, null);
        }
        public async Task DoctorCancelYourTime(ApplicationUser user, string Message)
        {
            await _notifiyBase.AddNotify(user, NotifyHeader.Doctor_Cancel_Your_Time, Message, null);
        }
        public async Task DoctorAddNewDayInSchedule(ApplicationUser user, string Message)
        {
            await _notifiyBase.AddNotify(user, NotifyHeader.Doctor_Add_New_Day_In_Schedule, Message, null);
        }
        public async Task DoctorRemoveDayFromSchedule(ApplicationUser user, string Message)
        {
            await _notifiyBase.AddNotify(user, NotifyHeader.Doctor_Remove_Day_From_Schedule, Message, null);
        }
        public async Task RateDoctor(ApplicationUser user, string Message)
        {
            await _notifiyBase.AddNotify(user, NotifyHeader.Rate_Doctor, Message, null);
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
