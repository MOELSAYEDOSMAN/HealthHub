namespace HealthHup.API.Model.Models.PharmacyModel
{
    public class Pharmacy
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double? xLoc { get; set; }
        public double? yLoc { get; set; }
        public Area? area { get; set; }
        public List<PharmacyDrug>? pharmacyDrugs { get; set; }
    }
}
