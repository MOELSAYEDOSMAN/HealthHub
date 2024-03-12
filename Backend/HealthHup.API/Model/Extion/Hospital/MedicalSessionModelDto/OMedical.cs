namespace HealthHup.API.Model.Extion.Hospital.MedicalSessionModelDto
{
    public class OMedical
    {
        public string diseaseName { get; set; }
        public DateOnly sessionDate { get; set; }
        public string? notes { get; set; } = "";
        public List<RepentanceDto> repentances { get; set;}
        public Guid doctorId { get; set; }
        public string doctorName { get; set; }
        public string doctorImg { get; set;}

        public static implicit operator OMedical(MedicalSession input)
            => new OMedical()
            {
                diseaseName=input?.DiseaseName,
                doctorId=input.DoctorId,
                notes=input?.Notes,
                doctorImg=input.Doctor.doctor.src,
                doctorName=input.Doctor.doctor.Name,
                repentances = new List<RepentanceDto>(),
                sessionDate=DateOnly.FromDateTime(input.date),
            };
    }
     
}
