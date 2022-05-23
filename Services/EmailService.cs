using AmeriForce.Models.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.Collections.Generic;
using System.Linq;

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
            // split ccs and bccs
            List<string> ccList = new List<string>();
            List<string> bccList = new List<string>();

            if (!string.IsNullOrEmpty(cc))
            {
                ccList = cc.Split(',').ToList();
            }

            if (!string.IsNullOrEmpty(cc))
            {
                bccList = bcc.Split(',').ToList();
            }

            // create message
            var email = new MimeMessage();

            var fromEmail = new MailboxAddress(fromName, fromAddress);
            email.From.Add(fromEmail);
            email.To.Add(MailboxAddress.Parse(to));

            if (ccList.Count > 0)
            {
                foreach(var ccItem in ccList)
                { 
                    email.Cc.Add(MailboxAddress.Parse(ccItem));
                }
            }

            if (bccList.Count > 0)
            {
                foreach (var bccItem in bccList)
                {
                    email.Bcc.Add(MailboxAddress.Parse(bccItem));
                }
            }

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
