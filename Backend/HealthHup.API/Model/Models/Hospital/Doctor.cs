using Microsoft.EntityFrameworkCore;

namespace HealthHup.API.Model.Models.Hospital
{
    public class Doctor
    {
        public Guid Id { get; set; }
        public DateTime GraduationYear {  get; set; }
        public string CollegeName { get; set; }
        public string SummaryCareer { get; set; }
        public string AddressDescrption { get; set; }
        public bool Accept { get; set; }
        public decimal priceSession { get; set; }   
        public DateTime DateOfJoin { get; set; }
        public DateTime DateOfSendRequest { get; set; }
        public Guid drSpecialtieId { get; set; }
        public Specialtie drSpecialtie { get; set; }
        public string? doctorId { get; set; }
        public ApplicationUser doctor { get; set; }
        public Guid areaId { get; set; }
        public Area area { get; set; }
        public List<DoctorCertificate>? Certificates { get; set; }
        public List<DoctorDate>? Dates { get; set; }
        public List<Review>? reviews { get; set; }
        public List<PatientDates> patientDates { get; set; }
        public List<MedicalSession> medicicalSessions { get; set;}
    }
}
