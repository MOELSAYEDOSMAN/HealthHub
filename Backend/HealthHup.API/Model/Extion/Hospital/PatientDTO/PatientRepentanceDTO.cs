namespace HealthHup.API.Model.Extion.Hospital.PatientDTO
{
    public class PatientRepentanceDTO
    {
        public IEnumerable<RepentanceDTO> repentances { get; set; }
        public string dcotorName { get; set; }
        public string doctorPhone { get; set; }
        public string doctorEmail { get; set; }
        public string doctorImg { get; set; }
        public string diseaseName { get; set; }
        public Guid doctorId { get; set; }
        public DateOnly date { get; set; }

        public static implicit operator PatientRepentanceDTO(MedicalSession input)
            => new PatientRepentanceDTO
            {
                repentances = RepentanceDTO.ConvertFromListOfRepentance(input.repentances),
                diseaseName=input.DiseaseName,
                dcotorName=input.Doctor.doctor.Name,
                doctorEmail=input.Doctor.doctor.Email,
                doctorImg=input.Doctor.doctor.src,
                doctorPhone=input.Doctor.doctor.PhoneNumber,
                doctorId=input.DoctorId,
                date =DateOnly.FromDateTime(input.date)
            };

        public static IEnumerable<PatientRepentanceDTO> ConvertFromMedicalSession(IList<MedicalSession> input)
        {
            foreach (var i in input)
                yield return i;
        }
    }
}
