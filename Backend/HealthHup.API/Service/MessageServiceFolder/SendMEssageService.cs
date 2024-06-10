namespace HealthHup.API.Service.MessageServiceFolder
{
    public class SendMEssageService:ISendMessageService
    {
        private readonly BaseMessageService _baseMessage;
        public SendMEssageService(IConfiguration configuration, IWebHostEnvironment env)
        {
            _baseMessage=new BaseMessageService(configuration, env);
        }
        public async Task ActionWithDoctor(string Email,string Name,string Messsage)
        {
            await _baseMessage.SendMessageWithoutLink(Email,Name, "Notice Of Your Application to Become A Doctor", Messsage);

        }
        public async Task ConfirmAccount(string Email,string Link)
        {
            await _baseMessage.SendMessageWithLink(Email, "Confirm Account", Link);
        }

        public async Task ForgetPassword(string Email,string UserName,string Message)
        {
            await _baseMessage.SendMessageWithoutLink(Email,UserName,"Forget Passowrd",$"Enter Code {Message}");
        }

        public async Task AlertDate(string Email,string UserName,string DoctorName, DateAction dateAction, string Message)
        {
            await _baseMessage.SendMessageWithoutLink(Email, UserName, $"Dr\\{DoctorName} {dateAction.ToString()} Date", Message);
        }
        public async Task ForgetDateMedicalSession(string Email,string UserName, string DoctorName,string Message)
        {
            await _baseMessage.SendMessageWithoutLink(Email, UserName, $"Missed A dr:{DoctorName} 's appointment",Message);
        }
        public async Task RateDoctorLow(string Email, string UserName)
        {
            await _baseMessage.SendMessageWithoutLink(Email, UserName, "Your rating is Weak", "You Must Improve Your Rating Over 3 Within a Year");
        }
        public async Task TakeBlock(string Email,string Name,string Message)
        {
            await _baseMessage.SendMessageWithoutLink(Email, Name, $"You Take Block", $"You Take Block {Message}");
        }
    }
}
