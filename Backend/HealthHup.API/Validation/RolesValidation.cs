using System.ComponentModel.DataAnnotations;

namespace HealthHup.API.Validation
{
    public class RolesValidation : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string Value=(string)value;
            string[] Roles = {"Admin","Doctor", "CustomerService" };
            if (Roles.Any(v=>v==Value))
                return true;
            return false;
        }
    }
}
