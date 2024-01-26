namespace HealthHup.API.Model.Models.AdressModle
{
    public class Governorate
    {
        public Guid Id { get; set; }
        public string key { get; set; }
        public List<Area> areas { get; set; }
    }
}
