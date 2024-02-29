using System.ComponentModel.DataAnnotations.Schema;

namespace HealthHup.API.Model.Models.Chat
{
    public class Message
    {
        public Guid Id { get; set; }


        [ForeignKey("UserSendId")]
        public ApplicationUser? UserSend { get; set; }
        public string? UserSendId { get; set; }

        [ForeignKey("UserReciveId")]
        public ApplicationUser? UserRecive { get;set; }
        public string? UserReciveId { get; set; }

        public string text { get; set; }
        public DateTime dateTiemSendMessage { get; set; }

        public bool See { get; set; }
    }
}
