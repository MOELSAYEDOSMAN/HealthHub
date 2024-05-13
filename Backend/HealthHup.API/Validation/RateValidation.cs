using System.ComponentModel.DataAnnotations;

namespace HealthHup.API.Validation
{
    public class RateValidation : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            decimal rate = (decimal)value;
            if(rate>1&&rate<5)
                return true;
            return false;
        }
    }
}
