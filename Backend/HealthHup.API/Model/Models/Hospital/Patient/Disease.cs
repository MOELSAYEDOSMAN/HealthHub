using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthHup.API.Model.Models.Hospital.Patient
{
    [Owned]
    public class Disease
    {
        
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool persistent { get; set; }
        public bool Cured { get; set; }
        public string? Notes { get; set; }
        public Guid? responsibledDoctorId { get; set; }
        [ForeignKey("responsibledDoctorId")]
        public Doctor? responsibledDoctor { get; set;}
        public string ApplicationUserId { get; set; }//Paient

    }
}
