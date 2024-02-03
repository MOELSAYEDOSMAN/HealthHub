namespace HealthHup.API.JWT
{
    public class Jwt
    {
        public string Key { get; set; }
        public string IssUser { get; set; }
        public string Audience { get; set; }
        public double Expire { get; set; }
    }
}
