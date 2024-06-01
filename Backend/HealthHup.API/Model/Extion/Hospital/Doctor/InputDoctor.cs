using HealthHup.API.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HealthHup.API.Model.Extion.Hospital.Doctor
{
    public class InputDoctor
    {
        public bool? error { get; set; }
        public string? message { get; set; }=string.Empty;
        [Required(ErrorMessage = "Must Select Location Clinic")]
        public Guid areaClinicId { get; set; }
        [Required(ErrorMessage = "Must Select Graduation Year")]
        public DateTime graduationYear { get; set; }
        [Required(ErrorMessage = " College Name Required"), SerchValidation(ErrorMessage = " College Name Required")]
        public string CollegeName { get; set; }
        [Required(ErrorMessage = " Summary Career Required"), SerchValidation(ErrorMessage = " Summary Career Required")]
        public string SummaryCareer { get; set; }
        [Required(ErrorMessage = "Address Descrption Required"), SerchValidation(ErrorMessage = "Address Descrption Required")]
        public string AddressDescrption { get; set; }
        [Required(ErrorMessage = "Must Select Specialtie")]
        public Guid specialtieId { get; set; }


        //Func
        public static implicit operator Models.Hospital.Doctor(InputDoctor inputDoctor)
            => new()
            {
                Accept=false,
                AddressDescrption=inputDoctor.AddressDescrption,
                CollegeName=inputDoctor.CollegeName,
                DateOfSendRequest=DateTime.Now,
                GraduationYear=inputDoctor.graduationYear,
                SummaryCareer=inputDoctor.SummaryCareer,
                Id=Guid.NewGuid(),
                Certificates=new List<DoctorCertificate>()
            };
    }
}
