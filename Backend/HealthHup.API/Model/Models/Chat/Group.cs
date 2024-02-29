using System.ComponentModel.DataAnnotations.Schema;

namespace HealthHup.API.Model.Models.Chat
{
    public class Group
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool SendMessage { get; set; }
        public bool AccseptedMessages { get; set; }

        [ForeignKey("UserSendId")]
        public ApplicationUser? UserSendRequest { get; set; }
        public string? UserSendId { get; set; }
        [ForeignKey("UserReciveId")]
        public ApplicationUser? UserSendRecive { get; set; }
        public string? UserReciveId { get; set; }
        public List<Message> Messages { get; set; }
    }
}
