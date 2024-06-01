namespace HealthHup.API.Model.Extion.Hospital.PatientDTO
{
    public class PatientDates_DTO
    {
        //Doctor Information
        public Guid? doctorId { get; set; }
        public string doctorName { get; set; }
        public string doctorSpecialty { get; set; }
        public string areaName { get; set; }
        public string governorateName { get; set; }

        //Date Information
        public Guid paientDateId { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public DateOnly date { get; set; }
        public string dayName { get; set; }


        public static implicit operator PatientDates_DTO(PatientDates input)
            => new PatientDates_DTO()
            {
                paientDateId=input.Id,
                doctorId= input?.doctor?.Id,
                doctorName=input?.doctor?.doctor?.Name,
                areaName = input?.doctor?.area.key,
                governorateName = input?.doctor?.area?.governorate?.key,
                doctorSpecialty = input?.doctor?.drSpecialtie.Name,
                date=DateOnly.FromDateTime(input?.date??DateTime.MinValue),
                dayName=input?.date.DayOfWeek.ToString()??"",
                from=input.FromTime
            };
    }
}
