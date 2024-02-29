using Microsoft.AspNetCore.Identity;
namespace HealthHup.API.Model.Models
{
    public class ApplicationUser:IdentityUser
    {
        public Area area { get; set; }
        public Guid AreaId { get; set; }
        public string Name { get; set; }
        public string? src { get; set; }
        public bool Gender {  get; set; }
        public DateTime Brdate { get; set; }
        public int Age => DateTime.Now.Year - Brdate.Year;
        public List<Disease>? Diseases { get; set; }
        public List<PatientDates> patientDates { get; set; }
    }
}
