using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthHup.API.Model.Models.Hospital.Patient
{
    public class Disease
    {
        
        public Guid Id { get; set; }
        public string Name { get; set; }=string.Empty;
        public bool persistent { get; set; } = false;
        public bool Cured { get; set; } = false;
        public string? Notes { get; set; } = string.Empty;
        public Guid? responsibledDoctorId { get; set; }
        [ForeignKey("responsibledDoctorId")]
        public Doctor? responsibledDoctor { get; set;}
        public string PatientId { get; set; }//Paient
        [ForeignKey("PatientId")]
        public ApplicationUser Patient { get; set; }

    }
}
