using System.ComponentModel.DataAnnotations;

namespace HealthHup.API.Model.Extion.Account
{
    public class InputLogin
    {
        [Required(ErrorMessage ="Must Enter UserName|Email")]
        public string userName { get; set; }
        [Required(ErrorMessage ="Must Enter Password") ,MinLength(length: 8,ErrorMessage ="Password Must Be at Least 8 Characters"),RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[_#$^+=!*()@%&]).{8,}$",ErrorMessage = "Password Must Consist of a [A-Z],a number[0-9],a Symbol[_#$^+=!*()@%&]")]
        public string password { get; set; }
    }
}
