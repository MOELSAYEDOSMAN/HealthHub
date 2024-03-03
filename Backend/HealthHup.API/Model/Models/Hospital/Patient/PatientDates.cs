using System.ComponentModel.DataAnnotations.Schema;

namespace HealthHup.API.Model.Models.Hospital.Patient
{
    public class PatientDates
    {
        public Guid Id { get; set; }
        public string? patientId { get; set; }
        [ForeignKey("patientId")]
        public ApplicationUser? patient { get; set; }
        public Guid doctorId { get; set; }
        [ForeignKey("doctorId")]
        public Doctor? doctor { get; set; }
        public DateTime date { get; set; }
        public string FromTime { get; set; }
    }
}
