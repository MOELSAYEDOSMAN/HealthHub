using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace HealthHup.API.Model.Extion.Account
{
    public class InputRegister
    {
        [Required(ErrorMessage ="Must Enter Name(First Name,Mid Name,Last Name)"),DataType(dataType:DataType.Text)]
        public string name { get; set; }
        [Required,RegularExpression(@"^[23]\d{2}(0[1-9]|1[0-2])(0[1-9]|[12]\d|3[01])\d{7}$",ErrorMessage = "National ID Required")]
        public string nationalID { get; set; }
        [Required(ErrorMessage ="Must Enter Email (***@example.com)"), DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [Required(ErrorMessage = "Must Enter Password"), MinLength(length: 8, ErrorMessage = "Password Must Be at Least 8 Characters"), RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[_#$^+=!*()@%&]).{8,}$", ErrorMessage = "Password Must Consist of a [A-Z],a number[0-9],a Symbol[_#$^+=!*()@%&]")]
        public string password { get; set; }
        [Required(ErrorMessage ="Must Enter Phone"),StringLength(maximumLength:11,MinimumLength =11,ErrorMessage ="You Must Enter 11 Numbers"),RegularExpression(@"^(?:\+20|0)?1[0125]\d{8}$", ErrorMessage ="01[0|1|2|5]*******")]
        public string phone { get; set; }
        [Required(ErrorMessage ="Must Select BirthDay"), DataType(DataType.Date)]
        public DateTime birday { get; set; }
        [Required(ErrorMessage ="Must Select Area")]
        public string area { get; set; }
        [Required]
        public bool Gender { get; set; }
        [JsonIgnore]
        public IFormFile? img { get; set; }
        //Func
        public static implicit operator ApplicationUser(InputRegister input)
            => new()
            {
                Email=input.email,
                PhoneNumber=input.phone,
                Brdate=input.birday,
                Name=input.name,
                Gender=input.Gender,
                nationalID=input.nationalID,
                UserName = new EmailAddressAttribute().IsValid(input.email) ? new MailAddress(input.email).User : input.email
            };
    }
}
