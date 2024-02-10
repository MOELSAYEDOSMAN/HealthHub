namespace HealthHup.API.Model.Extion.Hospital.Doctor
{
    public class ODoctor
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string phone { get; set; }
        public string? DrImg { get; set; }
        public bool Gender { get; set; }
        public DateTime DateOfJoin { get; set; }
        public DateTime DateOfSendRequest { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime GraduationYear { get; set; }
        public string CollegeName { get; set; }
        public string SummaryCareer { get; set; }
        public string DepartmentName { get; set; }
        public string AddressDescrption { get; set; }
        public bool Accept { get; set; }
        public string area { get; set; }
        public List<DoctorCertificate>? Certificates { get; set; }
        

        //Functions
        public static implicit operator ODoctor(Model.Models.Hospital.Doctor? input)
        {
            return new ODoctor()
            {
                Id = input?.Id ?? Guid.Empty,
                Accept = input?.Accept ?? false,
                AddressDescrption = input?.AddressDescrption,
                area = input?.area.key ?? string.Empty,
                Certificates = input?.Certificates??new List<DoctorCertificate>(),
                CollegeName = input?.CollegeName ?? string.Empty,
                DepartmentName = input?.drSpecialtie?.Name ?? string.Empty,
                Email = input?.doctor?.Email ?? string.Empty,
                Birthdate = input?.doctor?.Brdate ?? DateTime.MinValue,
                Name = input?.doctor?.Name ?? string.Empty,
                Gender = input?.doctor?.Gender ?? false,
                GraduationYear = input?.GraduationYear ?? DateTime.MinValue,
                phone = input?.doctor?.PhoneNumber ?? string.Empty,
                DrImg = input?.doctor?.src ?? string.Empty,
                SummaryCareer = input?.SummaryCareer ?? string.Empty,
                DateOfJoin=input?.DateOfJoin??DateTime.MinValue,
                DateOfSendRequest=input?.DateOfSendRequest??DateTime.MinValue,
            };
        }

    }
    public class ListOutPutDoctors
    {
        public IList<ODoctor> Doctors { get; set;}
        public int count { get; set; } = 0;
    }
}
