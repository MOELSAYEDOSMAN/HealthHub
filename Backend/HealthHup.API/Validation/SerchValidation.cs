using System.ComponentModel.DataAnnotations;

namespace HealthHup.API.Validation
{
    public class SerchValidation : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string val= (string)value;
            if(string.IsNullOrEmpty(val)||string.IsNullOrWhiteSpace(val))
                return false;

            return true;
        }
    }
}
