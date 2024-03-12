using System.ComponentModel.DataAnnotations.Schema;

namespace HealthHup.API.Model.Models.Hospital.Patient
{
    public class MedicalSession
    {
        public Guid Id { get; set; }
        public DateTime date { get; set; }
        public string? Notes { get; set; }
        public string DiseaseName { get; set; }
        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public string? PatientId { get; set; }
        [ForeignKey("PatientId")]
        public ApplicationUser? patient { get; set; }
        public List<Repentance> repentances { get; set; }
    }
}
