using System.ComponentModel.DataAnnotations;

namespace HealthHup.API.Model.Extion.Account
{
    public class InputLogin
    {
        [Required]
        public string userName { get; set; }
        [Required ,MinLength(8), RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[_#$^+=!*()@%&]).{8,}$")]
        public string password { get; set; }
    }
}
