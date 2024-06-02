using System.ComponentModel.DataAnnotations.Schema;

namespace HealthHup.API.Model.Models.Logs
{
    [Table(name:"AdminLogs")]
    public class LogAdminAction
    {
        public Guid Id { get; set; }
        public string? AdminId { get; set; }
        [ForeignKey("AdminId")]
        public ApplicationUser? Admin { get; set; }

        public AdminAction adminAction { get; set; }
        
        public DateTime dateCreated { get; set; }

        public string? Message { get; set; }

        
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        public LogAdminAction()
        {
            
        }
        public LogAdminAction(ApplicationUser _Admin,string _Message, AdminAction _action)
        {
            this.dateCreated = DateTime.UtcNow;
            this.Admin=_Admin;
            this.adminAction=_action;
            this.Message = _Message;
        }
        public LogAdminAction(ApplicationUser _Admin,ApplicationUser _User ,AdminAction _action)
        {
            this.dateCreated = DateTime.UtcNow;
            this.Admin = _Admin;
            this.User=_User;
            this.adminAction = _action;
        }

    }

    public enum AdminAction
    {
        AddAdmin,RemoveAdmin,AddDoctor,BlockDoctor
    }
}
