using AmeriForce.Models.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace AmeriForce.Services
{
    public interface IEmailService
    {
        void Send(string fromName, string fromAddress, string to, string cc, string bcc, string subject, string html);
    }

    public class EmailService : IEmailService
    {
        //private readonly AppSettings _appSettings;

        //public EmailService(IOptions<AppSettings> appSettings)
        //{
        //    _appSettings = appSettings.Value;
        //}

        private readonly EmailConfigurationViewModel _emailConfig;
        public EmailService(EmailConfigurationViewModel emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public void Send(string fromName, string fromAddress, string to, string cc, string bcc, string subject, string html)
        {
            // create message
            var email = new MimeMessage();

            var fromEmail = new MailboxAddress(fromName, fromAddress);
            email.From.Add(fromEmail); // MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse(to));
            //email.Cc.Add(MailboxAddress.Parse(cc));
            //email.Bcc.Add(MailboxAddress.Parse(bcc));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailConfig.UserName, _emailConfig.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
