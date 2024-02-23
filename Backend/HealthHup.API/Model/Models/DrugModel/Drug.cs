using System.ComponentModel.DataAnnotations;

namespace HealthHup.API.Model.Models.DrugModel
{
    public class Drug
    {
        [Key, Required]
        public Guid Id { get; set; }=Guid.NewGuid();
        [Required]
        public string name { get; set; }
        [Required]
        public string smiles { get; set; }
        
    }
}
