using System;

namespace AmeriForce.Models.Email
{
    public class SentEmailViewModel
    {
        public int VMID { get; set; }
        public string EmailID { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime SystemModstamp { get; set; }
        public string TextBody { get; set; }
        public string HTMLBody { get; set; }
        public string Subject { get; set; }
        public string FromName { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string CcAddress { get; set; }
        public string BccAddress { get; set; }
        public DateTime MessageDate { get; set; }
        public string RelatedToId { get; set; }


        public SentEmailViewModel()
        {
            CreatedDate = DateTime.Now;
            SystemModstamp = DateTime.Now;
            MessageDate = DateTime.Now;
        }

    }
}
