using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
namespace HealthHup.API.Service.MessageServiceFolder
{
    public class MessageService : IMessageService
    {
        private string FromEmail;
        private string Password;
        public MessageService(IConfiguration configuration)
        {
            FromEmail=configuration.GetSection("Smptp").GetValue<string>("Email");
            Password = configuration.GetSection("Smptp").GetValue<string>("Password");
        }
        public string GetEmail()
            => Password;
        public async Task SendMessage(string ToEmail, string Message,string? Color)
        {
            
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("HealthHup Admin",FromEmail));
            email.To.Add(MailboxAddress.Parse(ToEmail));
            email.Subject = "HealthHub";
            var builder = new BodyBuilder();
            builder.HtmlBody = $"<h1>Health Hub</h1><br><h3 style=\"Color:{Color ?? "Black"}\">{Message}</h3>";
            email.Body = builder.ToMessageBody();
            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(FromEmail, Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
            
            //MailMessage message = new MailMessage()
            //{
            //    From=new MailAddress(FromEmail),
            //    ReplyTo=new MailAddress(ToEmail),
            //    IsBodyHtml=true,
            //    BodyEncoding=Encoding.UTF8,
            //    Subject="Doctor",

            //    Body =$"<h1>Health Hub</h1><br><h3 style=\"Color:{Color??"Black"}\">{Message}</h3>",
            //};
            //var smtpClient =new SmtpClient("smtp.ethereal.email",587) 
            //{
            //Credentials=new NetworkCredential(FromEmail,Password),
            //EnableSsl=true,
            //UseDefaultCredentials=false
            //};
            //smtpClient.Send(message);
        }
    }
}
