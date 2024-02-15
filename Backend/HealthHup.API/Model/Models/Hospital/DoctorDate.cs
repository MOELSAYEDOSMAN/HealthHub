using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using HealthHup.API.Validation;
namespace HealthHup.API.Model.Models.Hospital
{
    [Owned]
    public class DoctorDate
    {
        [Required(ErrorMessage ="Must Seletc Day")]
        public string DayName { get; set; }
        [Required(ErrorMessage ="Must Enter Time"), DoctorDateValidation(ErrorMessage ="Format like 10:30 AM")]
        public string From { get; set; }
        [Required(ErrorMessage = "Must Enter Time"), DoctorDateValidation(ErrorMessage = "Format like 10:30 AM")]
        public string To { get; set; }
    }
}
