namespace HealthHup.API.Model.Extion.Hospital.PatientDTO
{
    public class PatientRepentanceDTO
    {
        public List<Repentance> repentances { get; set; }
        public string diseaseName { get; set; }
        public DateOnly date { get; set; }

        public static implicit operator PatientRepentanceDTO(MedicalSession input)
            => new PatientRepentanceDTO
            {
                repentances = input.repentances,
                diseaseName=input.DiseaseName,
                date=DateOnly.FromDateTime(input.date)
            };
    }
}
