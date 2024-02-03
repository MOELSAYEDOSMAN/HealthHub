namespace HealthHup.API.Model.Models.Hospital
{
    public class Review
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public DateTime SendTime { get; set; }
        public Doctor doctor { get; set; }
        public ApplicationUser Patient { get; set; }
    }
}
