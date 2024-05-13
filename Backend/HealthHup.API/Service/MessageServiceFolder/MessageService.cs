using Humanizer;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
namespace HealthHup.API.Service.MessageServiceFolder
{
    public class MessageService : IMessageService
    {
        private string FromEmail;
        private string Password;
        private readonly IWebHostEnvironment _env;

        public MessageService(IConfiguration configuration, IWebHostEnvironment env)
        {
            FromEmail = configuration.GetSection("Smptp").GetValue<string>("Email");
            Password = configuration.GetSection("Smptp").GetValue<string>("Password");
            _env = env;

        }
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
                smtp.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                smtp.Authenticate(FromEmail, Password);
                await smtp.SendAsync(email);
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

        public async Task<bool> ConfirmAccount(string Email, string link)
        {
            try
            {
                var Mailmessage = new MimeMessage();
                Mailmessage.From.Add(new MailboxAddress("HealthHub", FromEmail));
                Mailmessage.To.Add(MailboxAddress.Parse(Email));
                Mailmessage.Subject = $"HealthHub(Confirm Account) 🔔";
                string fileHtml = Path.Combine(_env.ContentRootPath + System.IO.Path.DirectorySeparatorChar,
                    $@"wwwroot/Template/ConfirmMail.html");

                //Read File
                var str = new StreamReader(fileHtml);
                var mailText = str.ReadToEnd();
                str.Close();
                mailText = mailText.Replace("[link]", link);
                var builder = new BodyBuilder();
                builder.HtmlBody = mailText;
                Mailmessage.Body = builder.ToMessageBody();
                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                    smtp.Authenticate(FromEmail, Password);
                    await smtp.SendAsync(Mailmessage);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


         
    }
}
