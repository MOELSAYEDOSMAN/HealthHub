using HealthHup.API.Validation;
using System.ComponentModel.DataAnnotations;

namespace HealthHup.API.Model.Extion.Hospital.RateModelDto
{
    public class RateDTO
    {
        [Required(ErrorMessage ="Must Enter Rate"), RateValidation(ErrorMessage ="Must Value between 1 To 5")]
        public decimal rate { get; set; }
        [Required(ErrorMessage ="Must Enter Your Opinion")]
        public string message { get; set; }
    }
}
