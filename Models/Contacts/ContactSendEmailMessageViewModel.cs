using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace AmeriForce.Models.Contacts
{
    public class ContactSendEmailMessageViewModel
    {
        public string Id { get; set; }
        public string ContactName { get; set; }
        public string ActivityId { get; set; }
        public string CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string LastModifiedById { get; set; }
        public DateTime? SystemModstamp { get; set; }
        public string TextBody { get; set; }
        public string HtmlBody { get; set; }
        public string Subject { get; set; }
        public string FromName { get; set; }
        public string FromAddress { get; set; }
        public string ToName { get; set; }
        public string ToAddress { get; set; }
        public string CcAddress { get; set; }
        public string BccAddress { get; set; }
        public int? HasAttachment { get; set; }
        public int? Status { get; set; }
        public DateTime? MessageDate { get; set; }
        public int? MessageSize { get; set; }
        public string MessageIdentifier { get; set; }
        public string ThreadIdentifier { get; set; }
        public int? IsTracked { get; set; }
        public DateTime? FirstOpenedDate { get; set; }
        public DateTime? LastOpenedDate { get; set; }
        public string RelatedTo { get; set; }
        public string TemporaryID { get; set; }


        public IEnumerable<SelectListItem> UserEmailList { get; set; }

        public IEnumerable<SelectListItem> ContactEmailList { get; set; }


    }
}
