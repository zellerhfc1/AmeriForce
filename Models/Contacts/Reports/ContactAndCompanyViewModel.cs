using System;

namespace AmeriForce.Models.Contacts.Reports
{
    public class ContactAndCompanyViewModel
    {
        public string Id { get; set; }
        public string AccountId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyType { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string OtherStreet { get; set; }
        public string OtherSuite { get; set; }
        public string OtherCity { get; set; }
        public string OtherState { get; set; }
        public string OtherPostalCode { get; set; }
        public string OtherCountry { get; set; }
        public string MailingStreet { get; set; }
        public string MailingSuite { get; set; }
        public string MailingCity { get; set; }
        public string MailingState { get; set; }
        public string MailingPostalCode { get; set; }
        public string MailingCountry { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string MobilePhone { get; set; }
        public string HomePhone { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Department { get; set; }
        public string AssistantName { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Description { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public DateTime? EmailOptOutDate { get; set; }
        public bool HasOptedOutOfEmail { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string LastModifiedById { get; set; }
        public DateTime? LastActivityDate { get; set; }
        public string EmailBouncedReason { get; set; }
        public DateTime? EmailBouncedDate { get; set; }
        public string Alt_Email { get; set; }
        public string Alt_Contact { get; set; }
        public string Children { get; set; }
        public string Direct_Line { get; set; }
        public string Extension { get; set; }
        public string Initial_Meeting_Details { get; set; }
        public string LinkedIn_Profile { get; set; }
        public string Mailing_Lists { get; set; }
        public string Opt_Out { get; set; }
        public DateTime? Opt_Out_Date { get; set; }
        public string Preferred_Name { get; set; }
        public DateTime? Reassigned_Date { get; set; }
        public DateTime? Referral_Date { get; set; }
        public DateTime? Referral_Partner_Agmnt_Date { get; set; }
        public string Referral_Partner_Agmnt_Details { get; set; }
        public string Referring_Company { get; set; }
        public string Referring_Contact { get; set; }
        public string Relationship_Status { get; set; }
        public string Standard_Pay_Terms { get; set; }
        public string Rating_Sort { get; set; }
        public string Tax_ID { get; set; }
        public string Term_Of_Agreement { get; set; }
        public string Twitter_Profile { get; set; }
        public string Type { get; set; }
        public string Update_Needed { get; set; }
        public DateTime? Update_Needed_Date { get; set; }
        public string Alt_Phone_3 { get; set; }

        public string OwnershipPercentage { get; set; }
        public bool Guarantor { get; set; } = false;

        public string NextActivityID { get; set; }
    }
}
