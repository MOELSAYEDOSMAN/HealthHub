using System.ComponentModel.DataAnnotations;

namespace HealthHup.API.Model.Extion.Hospital.Doctor
{
    public class DoctorFilterInput
    {
        public Guid? area { get; set; }=Guid.Empty;
        public Guid? goveId { get; set; }
        [Required]
        public Guid Specialtie { get; set; } = Guid.Empty;
        public bool reate { get; set; }=false;
        public bool joinDate { get; set; } = false;
        [Required]
        public uint Index { get; set; } = 0;
    }
}
