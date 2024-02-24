using Microsoft.EntityFrameworkCore;

namespace HealthHup.API.Model.Models.Hospital.Patient
{
    [Owned]
    public class Disease
    {
        public string Name { get; set; }
        public bool persistent { get; set; }
        public bool Cured { get; set; }
        public string? Notes { get; set; }
        public Doctor responsibledDoctor { get; set;}

    }
}
