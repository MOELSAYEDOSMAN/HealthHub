using System.ComponentModel.DataAnnotations;

namespace HealthHup.API.Model.Extion.Hospital.MedicalSessionModelDto
{
    public class RepentanceDto
    {
        [Required(ErrorMessage ="Must Select Drug")]
        public string drugId { get; set; }
        public string? note { get; set; }
        public string? repeat { get; set; }
        [Required(ErrorMessage ="Must Enter Repeat Count")]
        public uint repeatCount { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }

        public static implicit operator Repentance(RepentanceDto input)
            => new Repentance()
            {
                StartDate= input?.startDate,
                EndDate= input?.endDate,
                Note= input?.note,
                Repeat= input?.repeat,
                RepeatCount=(int)input?.repeatCount,
            };
    }
}
