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
    }
}
