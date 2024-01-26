namespace HealthHup.API.Model.Extion.Account
{
    public class OUser
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string ImgSrc { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
        public string? Message { get; set; }
        public bool Error { get; set; } = false;
        public bool IsLogin { get; set; }
        public DateTime ExToken { get; set; }
    }
}
