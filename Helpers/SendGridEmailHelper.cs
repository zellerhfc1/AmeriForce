using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace AmeriForce.Helpers
{
    public class SendGridEmailHelper
    {
        public async Task<string> SendEmail(EmailHelper email)
        {
            //var email = new EmailHelper
            //{
            //    EmailSubject = "This is a test",
            //    EmailTextContent = "This is a test",
            //    EmailHTMLContent = "<b>This is a test</b>",s
            //    EmailTo = "jzeller@amerisource.us.com",
            //    EmailToName = "Inside Jason",
            //    EmailCC = "zellerff@yahoo.com",
            //    EmailCCName = "Fantasy Jason",
            //    EmailSendID = id
            //};

            var response = await email.SendMail();
            //await email.
            //sendEmailModel.RelatedTo = id;
            //return View(sendEmailModel);
            return response;
        }
    }


    public class EmailHelper
    {
        public string EmailFrom { get; set; }
        public string EmailFromName { get; set; }
        public string EmailSubject { get; set; }
        public string EmailTextContent { get; set; }
        public string EmailHTMLContent { get; set; }
        public string EmailTo { get; set; }
        public string EmailToName { get; set; }
        public string EmailCC { get; set; }
        public string EmailCCName { get; set; }
        public string EmailBCC { get; set; }
        public string EmailBCCName { get; set; }
        public string EmailSendID { get; set; }

        public EmailHelper()
        {
            EmailFrom = "jzeller@amerisource.us.com";
            EmailFromName = "AmeriForce Admin";
        }

        public async Task<string> SendMail()
        {
            var client = new SendGridClient("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");
            var from = new EmailAddress(EmailFrom, EmailFromName);
            var subject = EmailSubject;
            var to = new EmailAddress(EmailTo, EmailToName);
            var plainTextContent = EmailTextContent;
            var htmlContent = EmailHTMLContent;
            htmlContent = htmlContent.Replace(Environment.NewLine, "<br>");
            htmlContent += $"<br><a href='https://amerisource.us.com'>asdfasdfasdf</a><br><br><img src='~/Images/updateGraphic.jpg' />";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            msg.SetBccSetting(true, "jzeller@amerisource.us.com");

            if (EmailCC != "")
            {
                var cc = new EmailAddress(EmailCC, EmailCCName);
                msg.AddCc(cc);
            }

            if (EmailBCC != "")
            {
                var bcc = new EmailAddress(EmailBCC, EmailBCCName);
                msg.AddCc(bcc);
            }

            //msg.AddAttachment(HttpContext.("~/Images/updateGraphic.jpg"));

            //var y = HttpServerUtility.Map

            //msg.AddContent(MimeType.Html, htmlContent);
            //msg.AddCc(cc);
            //msg.AddBcc(bcc);
            //msg.SetClickTracking(true, true);
            //msg.AddBcc(bcc);
            Response response;

            try
            {
                response = await client.SendEmailAsync(msg);
                if (response.StatusCode != HttpStatusCode.OK
                    && response.StatusCode != HttpStatusCode.Accepted)
                {
                    var errorMessage = response.Body.ReadAsStringAsync().Result;
                    throw new Exception($"Failed to send mail to {to}, status code {response.StatusCode}, {errorMessage}");
                }
            }
            catch (WebException exc)
            {
                throw new WebException(new StreamReader(exc.Response.GetResponseStream()).ReadToEnd(), exc);
            }

            return response.Body.ToString();
        }

    }
}
