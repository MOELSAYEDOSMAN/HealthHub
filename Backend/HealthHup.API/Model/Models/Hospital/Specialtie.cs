namespace HealthHup.API.Model.Models.Hospital
{
    public class Specialtie
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<Doctor> Doctors { get; set; }
    }
}
