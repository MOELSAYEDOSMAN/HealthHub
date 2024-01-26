namespace HealthHup.API.Model.Models.AdressModle
{
    public class Area
    {
        public Guid Id { get; set; }
        public string key { get; set; }
        public Governorate governorate { get; set; }
        public List<Pharmacy> pharmacys { get; set; }
    }
}
