using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthHup.API.Model.Models.Logs
{
    public class AlertBlcok
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        
        public string? message { get; set; }
        
        public AlertType alertType { get; set; }


        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
        public string? UserId { get; set; }

        public AlertBlcok()
        {
            
        }

        public AlertBlcok(ApplicationUser _user,AlertType _type, DateTime _date,string? _Message)
        {
            this.User = _user;
            this.message = _Message;
            this.alertType = _type;
            this.DateTime=_date;
        }

    }

    public enum AlertType
    {
        CancelManyDates,RateLow
    }
}
