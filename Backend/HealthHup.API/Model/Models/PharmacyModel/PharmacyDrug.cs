namespace HealthHup.API.Model.Models.PharmacyModel
{
    public class PharmacyDrug
    {
        public Guid Id { get; set; }
        public Pharmacy Pharmacy { get; set; }
        public Drug Drug { get; set; }
        public int Count { get; set; }
        public DateTime Expire { get; set; }
    }
}
