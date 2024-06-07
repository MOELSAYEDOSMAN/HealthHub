using System.ComponentModel.DataAnnotations.Schema;

namespace HealthHup.API.Model.Models.Notifaction
{
    public class Notify
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
        public NotifyHeader notifyHeader { get; set; }
        public string? link { get; set; }

    }

    public enum NotifyHeader
    {
        DoctorChangeYourTime, DoctorCancelYourTime,DoctorSelectDate,
        DoctorAddNewDayInSchedule,DoctorRemoveDayFromSchedule,RateDoctor
    }
}
