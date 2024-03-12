namespace HealthHup.API.Model.Extion.Hospital.MedicalSessionModelDto
{
    public class OutMedicalList
    {
        public Guid id { get; set; }
        public string diseaseName { get; set; }
        public DateOnly sessionDate { get; set; }
        public string? notes { get; set; } = "";

        public static implicit operator OutMedicalList(MedicalSession input)
            => new OutMedicalList()
            {
                id=input.Id,
                diseaseName=input.DiseaseName,
                notes=input?.Notes,
                sessionDate=DateOnly.FromDateTime(input.date)
            };
    }
}
