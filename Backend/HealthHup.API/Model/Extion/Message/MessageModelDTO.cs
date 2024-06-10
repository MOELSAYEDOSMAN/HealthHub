
namespace HealthHup.API.Model.Extion.Message
{
    public class MessageModelDTO
    {
        public string name { get; set; }
        public string message { get; set; }
        public DateOnly date { get; set; }
        public string time { get; set; }
        public string imgSrc { get; set; }
        public string email { get; set; }


        public static implicit operator MessageModelDTO(Model.Models.Chat.Message input)
            => new MessageModelDTO
            {
                date = DateOnly.FromDateTime(input.dateTiemSendMessage),
                email = input.UserSend.Email,
                imgSrc = input.UserSend.src,
                message = input.text,
                name = input.UserSend.Name,
                time = input.dateTiemSendMessage.ToShortTimeString()
            };


        public static IEnumerable<MessageModelDTO> ToListMessageModelDTOFromListMessage(IList<Model.Models.Chat.Message> messages)
        {
            foreach (var message in messages)
                yield return message;
        }

    }
}
