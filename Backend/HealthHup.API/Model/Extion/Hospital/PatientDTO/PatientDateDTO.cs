namespace HealthHup.API.Model.Extion.Hospital.PatientDTO
{
    public class PatientDateDTO
    {
        public string patientName { get; set; }
        public string email { get; set; }
        public string patientPhone { get; set; }
        public DateOnly date { get; set; }
        public string dayName { get; set; }
        public string from { get; set; }
        public string to { get; set; }

        public static implicit operator PatientDateDTO(PatientDates input)
            => new PatientDateDTO() { 
            patientName=input?.patient?.Name,
            email= input?.patient?.Email,
            patientPhone= input?.patient?.PhoneNumber,
            date=DateOnly.FromDateTime(input.date),
            from=input.FromTime
            };
    }

    public enum DateAction
    {
        Add,Cancel,Change
    }
}
