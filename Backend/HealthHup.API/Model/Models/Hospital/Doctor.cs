using Microsoft.EntityFrameworkCore;

namespace HealthHup.API.Model.Models.Hospital
{
    public class Doctor
    {
        public Guid Id { get; set; }
        public DateTime GraduationYear {  get; set; }
        public string CollegeName { get; set; }
        public string SummaryCareer { get; set; }
        public string DepartmentName {  get; set; }
        public ApplicationUser doctor { get; set; }
        public List<Review>? reviews { get; set; }
        public Specialtie drSpecialtie { get; set; }
    }
}
