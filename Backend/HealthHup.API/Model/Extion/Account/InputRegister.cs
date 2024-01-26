﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace HealthHup.API.Model.Extion.Account
{
    public class InputRegister
    {
        [Required,DataType(dataType:DataType.Text)]
        public string name { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [Required, DataType(DataType.Password),MinLength(8),RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[_#$^+=!*()@%&]).{8,}$")]
        public string password { get; set; }
        [Required,MinLength(11) ,RegularExpression(@"^01[0125][1-9]{8}$")]
        public string phone { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime birday { get; set; }
        [Required]
        public string area { get; set; }
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
                UserName = new EmailAddressAttribute().IsValid(input.email) ? new MailAddress(input.email).User : input.email
            };
    }
}
