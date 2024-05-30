namespace HealthHup.API.Model.Extion.Hospital.Doctor
{
    public class ODoctor
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string? DrImg { get; set; } = string.Empty;
        public bool Gender { get; set; }=false;
        public DateTime DateOfJoin { get; set; } = DateTime.MinValue;
        public DateTime DateOfSendRequest { get; set; } = DateTime.MinValue;
        public DateTime Birthdate { get; set; } = DateTime.MinValue;
        public DateTime GraduationYear { get; set; } = DateTime.MinValue;
        public string CollegeName { get; set; } = string.Empty;
        public string SummaryCareer { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string AddressDescrption { get; set; } = string.Empty;
        public bool Accept { get; set; } = false;
        public decimal rate { get; set; } = 0;
        public decimal priceSession { get; set; } = 0;
        public string area { get; set; } = string.Empty;
        public List<DoctorDate>? Dates { get; set; } = new List<DoctorDate>();
        public List<DoctorCertificate>? Certificates { get; set; } = new List<DoctorCertificate>();
        

        //Functions
        public static implicit operator ODoctor(Model.Models.Hospital.Doctor? input)
        {
            return new ODoctor()
            {
                Id = input?.Id ?? Guid.Empty,
                Accept = input?.Accept ?? false,
                AddressDescrption = input?.AddressDescrption ?? string.Empty,
                area = input?.area?.key ?? string.Empty,
                Certificates = input?.Certificates ?? new List<DoctorCertificate>(),
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
                DateOfJoin = input?.DateOfJoin ?? DateTime.MinValue,
                DateOfSendRequest = input?.DateOfSendRequest ?? DateTime.MinValue,
                Dates = input?.Dates ?? new List<DoctorDate>(),
                priceSession = input.priceSession
            };
        }

        public static IEnumerable<ODoctor> Doctors(IList<Model.Models.Hospital.Doctor>? input)
        {
            foreach (var d in input)
                yield return d;
        }

    }
    
    public class ListOutPutDoctors
    {
        public List<ODoctor> Doctors { get; set;}=new List<ODoctor>();
        public int count { get; set; } = 0;
    }
}
