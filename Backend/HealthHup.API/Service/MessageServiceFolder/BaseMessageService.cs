using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Serilog;
using System.Net.Mail;

namespace HealthHup.API.Service.MessageServiceFolder
{
    public class BaseMessageService
    {
        private string FromEmail;
        private string Password;
        private MimeMessage _MessageTemp;
        private readonly IWebHostEnvironment _env;
        public BaseMessageService(IConfiguration configuration, IWebHostEnvironment env)
        {
            FromEmail = configuration.GetSection("Smptp").GetValue<string>("Email");
            Password = configuration.GetSection("Smptp").GetValue<string>("Password");
            _env = env;
            
            _MessageTemp=new MimeMessage();
            _MessageTemp.From.Add(new MailboxAddress("HealthHub", FromEmail));

        }
        public async Task SendMessageWithLink(string email, string subject,string link)
        {
                //To
                _MessageTemp.To.Add(MailboxAddress.Parse(email));
                //Subject
                _MessageTemp.Subject = $"HealthHub({subject}) 🔔";
                //Template(Message Body)
                string fileHtml = Path.Combine(_env.ContentRootPath + System.IO.Path.DirectorySeparatorChar,
                    $@"wwwroot/Template/ConfirmMail.html");
                var str = new StreamReader(fileHtml);
                var mailText = str.ReadToEnd();
                str.Close();
                mailText = mailText.Replace("[link]", link);
                var builder = new BodyBuilder();
                builder.HtmlBody = mailText;
                _MessageTemp.Body = builder.ToMessageBody();
            //Send Message
            await TaskSendMessage();


        }
        public async Task SendMessageWithoutLink(string email,string UserName,string subject,string Message)
        {
            //To
            _MessageTemp.To.Add(MailboxAddress.Parse(email));
            //Subject
            _MessageTemp.Subject = $"HealthHub({subject}) 🔔";
            //Template(Message Body)
            string fileHtml = Path.Combine(_env.ContentRootPath + System.IO.Path.DirectorySeparatorChar,
                $@"wwwroot/Template/f1.html");
            var str = new StreamReader(fileHtml);
            var mailText = str.ReadToEnd();
            str.Close();
            mailText = mailText.Replace("[UserName]", UserName).Replace("[Message]", Message);
            var builder = new BodyBuilder();
            builder.HtmlBody = mailText;
            _MessageTemp.Body = builder.ToMessageBody();
            //Send Message
            await TaskSendMessage();
        }

        public async Task TaskSendMessage()
        {
            try
            {
                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                    smtp.Authenticate(FromEmail, Password);
                    await smtp.SendAsync(_MessageTemp);
                    
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            
        }
    }
}
