using System.ComponentModel.DataAnnotations.Schema;

namespace HealthHup.API.Model.Models.Hospital.Patient
{
    public class CTRay
    {
        public Guid Id { get; set; }
        public string Src { get; set; }
        [ForeignKey("PatientID")]
        public ApplicationUser Patient { get; set; }
        public string PatientID { get; set; }
    }
}
