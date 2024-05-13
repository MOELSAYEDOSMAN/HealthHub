using HealthHup.API.Validation;
using System.ComponentModel.DataAnnotations;

namespace HealthHup.API.Model.Extion.Hospital.PatientDTO
{
    public class PatientDateInput
    {
        [Required, DoctorDateValidation(ErrorMessage ="Must Select xx:xx (AM|PM)")]
        public string From { get; set; }
        [Required]
        public DateTime day { get; set; }

    }
}
