namespace HealthHup.API.Model.Models.DrugModel
{
    public class ActivePharmaceutical
    {
        public Guid Id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public double precent { get; set; }
        public List<Drug>? drugs { get; set; }
    }
}
