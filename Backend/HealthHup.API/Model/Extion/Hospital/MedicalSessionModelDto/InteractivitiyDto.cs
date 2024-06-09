namespace HealthHup.API.Model.Extion.Hospital.MedicalSessionModelDto
{
    public class InteractivitiyDto
    {
        public List<string> interactionResult { get; set; }
        public bool impact { get; set; }

        public InteractivitiyDto()
        {

        }
        public InteractivitiyDto(List<string> _interactionResult,bool _Impact)
        {
            interactionResult = _interactionResult;
            impact = _Impact;
        }
    }
}
