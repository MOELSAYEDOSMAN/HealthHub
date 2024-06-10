
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
        Doctor_Change_Your_Time, Doctor_Cancel_Your_Time,Doctor_Select_Date,
        Doctor_Add_New_Day_In_Schedule,Doctor_Remove_Day_From_Schedule,Rate_Doctor,
        Alert_Date,Doctor_Result
    }
}
