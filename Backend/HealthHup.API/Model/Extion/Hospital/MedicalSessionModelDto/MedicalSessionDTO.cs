using System.ComponentModel.DataAnnotations;

namespace HealthHup.API.Model.Extion.Hospital.MedicalSessionModelDto
{
    public class MedicalSessionDTO
    {
        public string? sessionnote { get; set; }
        [Required(ErrorMessage ="Must Enter Disease Name")]
        public string diseaseName { get; set; }
        [Required(ErrorMessage ="Must Select Patient Email"),DataType(DataType.EmailAddress)]
        public string patientEmail { get; set; }
        public List<RepentanceDto> repentances { get; set; }
        [Required]
        public bool persistent { get; set; }
        public string? diseasnote { get; set; }

        public static implicit operator MedicalSession(MedicalSessionDTO input)
            => new()
            {
                date = DateTime.Now,
                DiseaseName = input.diseaseName,
                Id = Guid.NewGuid(),
                Notes = input?.sessionnote,
            };

        public static implicit operator Disease(MedicalSessionDTO input)
            => new()
            {
                Id = Guid.NewGuid(),
                Notes = input.diseasnote,
                Name = input.diseaseName,
                persistent = input.persistent,
            };

    }
    
}
