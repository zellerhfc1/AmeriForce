using AmeriForce.Data;
using Microsoft.Extensions.Hosting.Internal;
using System.IO;

namespace AmeriForce.Helpers
{
    public class OutlookEmailHelper
    {
        private readonly ApplicationDbContext _context;

        public OutlookEmailHelper(ApplicationDbContext context)
        {
            _context = context;
        }
        //public string SendEmail(EmailViewModel crmEmail)
        //{
        //    MailMessage mailMessage = new MailMessage();
        //    mailMessage.From = new MailAddress(crmEmail.FromAddress);

        //    mailMessage.To.Add(crmEmail.ToAddresses);
        //    mailMessage.CC.Add(crmEmail.CCAddresses);
        //    mailMessage.Bcc.Add(crmEmail.BCCAddresses);

        //    mailMessage.Subject = crmEmail.Subject;
        //    mailMessage.Body = crmEmail.Body.ToString();
        //    mailMessage.IsBodyHtml = true;
        //    //mailMessage.Attachments.Add(new System.Net.Mail.Attachment(@"C:\Users\Zfamily\documents\visual studio 2015\Projects\ConsoleApplicationTesting\ClassLibrary1\Dylan.jpg"));
        //    //mailMessage.Attachments.Add(new System.Net.Mail.Attachment(@"C: \Users\Zfamily\documents\visual studio 2015\Projects\ConsoleApplicationTesting\ClassLibrary1\20190421_174031.jpg"));

        //    SmtpClient client = new SmtpClient();
        //    client.Credentials = new NetworkCredential("jzeller@amerisourcefunding.com", "St@pl3sgerms");
        //    client.Port = 587;
        //    client.Host = "smtp.office365.com";
        //    client.EnableSsl = true;

        //    client.Send(mailMessage);

        //    return "";
        //}


        //public byte[] GetTrackingImage(string id)
        //{
        //    var dir = HostingEnvironment.MapPath(@"~\images\");
        //    var imagePath = Path.Combine(dir, "ameriforceLogo_small.png");
        //    var imageFile = new FileInfo(imagePath);

        //    if (imageFile.Exists)
        //    {
        //            var email = db.EmailMessages.Where(e => e.Id == id).FirstOrDefault();
        //            if (email != null)
        //            {
        //                email.LastOpenedDate = DateTime.Now;
        //                db.SaveChanges();
        //            }
                
        //        return System.IO.File.ReadAllBytes(imagePath);
        //    }
        //    return null;
        //}
    }
}
