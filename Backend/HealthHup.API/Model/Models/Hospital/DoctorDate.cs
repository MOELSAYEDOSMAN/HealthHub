using Microsoft.EntityFrameworkCore;

namespace HealthHup.API.Model.Models.Hospital
{
    [Owned]
    public class DoctorDate
    {
        public string DayName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
