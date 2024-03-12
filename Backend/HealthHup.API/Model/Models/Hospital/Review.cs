namespace HealthHup.API.Model.Models.Hospital
{
    public class Review
    {
        public Guid Id { get; set; }
        public decimal rate {  get; set; }
        public string Message { get; set; }
        public DateTime SendTime { get; set; }
        public Doctor? doctor { get; set; }
        public Guid? doctorId { get; set; }
        public ApplicationUser? Patient { get; set; }
        public string? PatientId { get; set; }
    }
}
