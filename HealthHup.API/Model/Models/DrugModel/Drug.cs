namespace HealthHup.API.Model.Models.DrugModel
{
    public class Drug
    {
        public Guid Id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string? description { get; set; }
        public List<ActivePharmaceutical>? activePharmaceuticals { get; set; }
        public List<PharmacyDrug>? pharmacyDrugs { get; set; }
    }
}
