namespace HealthHup.API.Model.Models.Hospital
{
    public class Clinic
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Area area { get; set; }  
        public string AddressDescrption { get; set; }
        public Doctor Manger { get; set; }
        public IList<Doctor> Doctors { get; set; }
    }
}
