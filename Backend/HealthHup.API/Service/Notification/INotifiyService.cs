using HealthHup.API.Model.Extion;
using System.Threading.Tasks;

namespace HealthHup.API.Service.Notification
{
    public interface INotifiyService
    {
        Task DoctorAction(ApplicationUser user, string Message);
        Task AlertDate(ApplicationUser user, string Message);
        Task RemoveOldNotifactions();
        Task DoctorSelectDate(ApplicationUser user, string Message);
        Task DoctorChangeYourTime(ApplicationUser user, string Message);
        Task DoctorCancelYourTime(ApplicationUser user, string Message);
        Task DoctorAddNewDayInSchedule(ApplicationUser user, string Message);
        Task DoctorRemoveDayFromSchedule(ApplicationUser user, string Message);
        Task RateDoctor(ApplicationUser user, string Message);
        Task<IEnumerable<notfiyDTO>> GetUserNotificationsAsync(string Email);
    }
}
