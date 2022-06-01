using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AmeriForce.Data;
using AmeriForce.Models.Contacts.Reports;
using AmeriForce.Helpers;

namespace AmeriForce.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ContactsDevExtremeAPIController : Controller
    {
        private ApplicationDbContext _context;
        private UserHelper _userHelper;

        public ContactsDevExtremeAPIController(ApplicationDbContext context) {
            _context = context;
            _userHelper = new UserHelper(_context);
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var contacts = _context.Contacts.Select(i => new {
                i.Id,
                i.AccountId,
                i.Salutation,
                i.FirstName,
                i.LastName,
                i.MiddleName,
                i.OtherStreet,
                i.OtherSuite,
                i.OtherCity,
                i.OtherState,
                i.OtherPostalCode,
                i.OtherCountry,
                i.MailingStreet,
                i.MailingSuite,
                i.MailingCity,
                i.MailingState,
                i.MailingPostalCode,
                i.MailingCountry,
                i.Phone,
                i.Fax,
                i.MobilePhone,
                i.HomePhone,
                i.Email,
                i.Title,
                i.Department,
                i.AssistantName,
                i.Birthdate,
                i.Description,
                i.OwnerId,
                i.EmailOptOutDate,
                i.HasOptedOutOfEmail,
                i.CreatedDate,
                i.CreatedById,
                i.LastModifiedDate,
                i.LastModifiedById,
                i.LastActivityDate,
                i.EmailBouncedReason,
                i.EmailBouncedDate,
                i.Alt_Email,
                i.Alt_Contact,
                i.Children,
                i.Direct_Line,
                i.Extension,
                i.Initial_Meeting_Details,
                i.LinkedIn_Profile,
                i.Mailing_Lists,
                i.Opt_Out,
                i.Opt_Out_Date,
                i.Preferred_Name,
                i.Reassigned_Date,
                i.Referral_Date,
                i.Referral_Partner_Agmnt_Date,
                i.Referral_Partner_Agmnt_Details,
                i.Referring_Company,
                i.Referring_Contact,
                i.Relationship_Status,
                i.Standard_Pay_Terms,
                i.Rating_Sort,
                i.Tax_ID,
                i.Term_Of_Agreement,
                i.Twitter_Profile,
                i.Type,
                i.Update_Needed,
                i.Update_Needed_Date,
                i.Alt_Phone_3,
                i.OwnershipPercentage,
                i.Guarantor,
                i.NextActivityID
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "Id" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(contacts, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Contact();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Contacts.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.Id });
        }

        [HttpPut]
        public async Task<IActionResult> Put(string key, string values) {
            var model = await _context.Contacts.FirstOrDefaultAsync(item => item.Id == key);
            if(model == null)
                return StatusCode(409, "Object not found");

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task Delete(string key) {
            var model = await _context.Contacts.FirstOrDefaultAsync(item => item.Id == key);

            _context.Contacts.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(Contact model, IDictionary values) {
            string ID = nameof(Contact.Id);
            string ACCOUNT_ID = nameof(Contact.AccountId);
            string SALUTATION = nameof(Contact.Salutation);
            string FIRST_NAME = nameof(Contact.FirstName);
            string LAST_NAME = nameof(Contact.LastName);
            string MIDDLE_NAME = nameof(Contact.MiddleName);
            string OTHER_STREET = nameof(Contact.OtherStreet);
            string OTHER_SUITE = nameof(Contact.OtherSuite);
            string OTHER_CITY = nameof(Contact.OtherCity);
            string OTHER_STATE = nameof(Contact.OtherState);
            string OTHER_POSTAL_CODE = nameof(Contact.OtherPostalCode);
            string OTHER_COUNTRY = nameof(Contact.OtherCountry);
            string MAILING_STREET = nameof(Contact.MailingStreet);
            string MAILING_SUITE = nameof(Contact.MailingSuite);
            string MAILING_CITY = nameof(Contact.MailingCity);
            string MAILING_STATE = nameof(Contact.MailingState);
            string MAILING_POSTAL_CODE = nameof(Contact.MailingPostalCode);
            string MAILING_COUNTRY = nameof(Contact.MailingCountry);
            string PHONE = nameof(Contact.Phone);
            string FAX = nameof(Contact.Fax);
            string MOBILE_PHONE = nameof(Contact.MobilePhone);
            string HOME_PHONE = nameof(Contact.HomePhone);
            string EMAIL = nameof(Contact.Email);
            string TITLE = nameof(Contact.Title);
            string DEPARTMENT = nameof(Contact.Department);
            string ASSISTANT_NAME = nameof(Contact.AssistantName);
            string BIRTHDATE = nameof(Contact.Birthdate);
            string DESCRIPTION = nameof(Contact.Description);
            string OWNER_ID = nameof(Contact.OwnerId);
            string EMAIL_OPT_OUT_DATE = nameof(Contact.EmailOptOutDate);
            string HAS_OPTED_OUT_OF_EMAIL = nameof(Contact.HasOptedOutOfEmail);
            string CREATED_DATE = nameof(Contact.CreatedDate);
            string CREATED_BY_ID = nameof(Contact.CreatedById);
            string LAST_MODIFIED_DATE = nameof(Contact.LastModifiedDate);
            string LAST_MODIFIED_BY_ID = nameof(Contact.LastModifiedById);
            string LAST_ACTIVITY_DATE = nameof(Contact.LastActivityDate);
            string EMAIL_BOUNCED_REASON = nameof(Contact.EmailBouncedReason);
            string EMAIL_BOUNCED_DATE = nameof(Contact.EmailBouncedDate);
            string ALT_EMAIL = nameof(Contact.Alt_Email);
            string ALT_CONTACT = nameof(Contact.Alt_Contact);
            string CHILDREN = nameof(Contact.Children);
            string DIRECT_LINE = nameof(Contact.Direct_Line);
            string EXTENSION = nameof(Contact.Extension);
            string INITIAL_MEETING_DETAILS = nameof(Contact.Initial_Meeting_Details);
            string LINKED_IN_PROFILE = nameof(Contact.LinkedIn_Profile);
            string MAILING_LISTS = nameof(Contact.Mailing_Lists);
            string OPT_OUT = nameof(Contact.Opt_Out);
            string OPT_OUT_DATE = nameof(Contact.Opt_Out_Date);
            string PREFERRED_NAME = nameof(Contact.Preferred_Name);
            string REASSIGNED_DATE = nameof(Contact.Reassigned_Date);
            string REFERRAL_DATE = nameof(Contact.Referral_Date);
            string REFERRAL_PARTNER_AGMNT_DATE = nameof(Contact.Referral_Partner_Agmnt_Date);
            string REFERRAL_PARTNER_AGMNT_DETAILS = nameof(Contact.Referral_Partner_Agmnt_Details);
            string REFERRING_COMPANY = nameof(Contact.Referring_Company);
            string REFERRING_CONTACT = nameof(Contact.Referring_Contact);
            string RELATIONSHIP_STATUS = nameof(Contact.Relationship_Status);
            string STANDARD_PAY_TERMS = nameof(Contact.Standard_Pay_Terms);
            string RATING_SORT = nameof(Contact.Rating_Sort);
            string TAX_ID = nameof(Contact.Tax_ID);
            string TERM_OF_AGREEMENT = nameof(Contact.Term_Of_Agreement);
            string TWITTER_PROFILE = nameof(Contact.Twitter_Profile);
            string TYPE = nameof(Contact.Type);
            string UPDATE_NEEDED = nameof(Contact.Update_Needed);
            string UPDATE_NEEDED_DATE = nameof(Contact.Update_Needed_Date);
            string ALT_PHONE_3 = nameof(Contact.Alt_Phone_3);
            string OWNERSHIP_PERCENTAGE = nameof(Contact.OwnershipPercentage);
            string GUARANTOR = nameof(Contact.Guarantor);
            string NEXT_ACTIVITY_ID = nameof(Contact.NextActivityID);

            if(values.Contains(ID)) {
                model.Id = Convert.ToString(values[ID]);
            }

            if(values.Contains(ACCOUNT_ID)) {
                model.AccountId = Convert.ToString(values[ACCOUNT_ID]);
            }

            if(values.Contains(SALUTATION)) {
                model.Salutation = Convert.ToString(values[SALUTATION]);
            }

            if(values.Contains(FIRST_NAME)) {
                model.FirstName = Convert.ToString(values[FIRST_NAME]);
            }

            if(values.Contains(LAST_NAME)) {
                model.LastName = Convert.ToString(values[LAST_NAME]);
            }

            if(values.Contains(MIDDLE_NAME)) {
                model.MiddleName = Convert.ToString(values[MIDDLE_NAME]);
            }

            if(values.Contains(OTHER_STREET)) {
                model.OtherStreet = Convert.ToString(values[OTHER_STREET]);
            }

            if(values.Contains(OTHER_SUITE)) {
                model.OtherSuite = Convert.ToString(values[OTHER_SUITE]);
            }

            if(values.Contains(OTHER_CITY)) {
                model.OtherCity = Convert.ToString(values[OTHER_CITY]);
            }

            if(values.Contains(OTHER_STATE)) {
                model.OtherState = Convert.ToString(values[OTHER_STATE]);
            }

            if(values.Contains(OTHER_POSTAL_CODE)) {
                model.OtherPostalCode = Convert.ToString(values[OTHER_POSTAL_CODE]);
            }

            if(values.Contains(OTHER_COUNTRY)) {
                model.OtherCountry = Convert.ToString(values[OTHER_COUNTRY]);
            }

            if(values.Contains(MAILING_STREET)) {
                model.MailingStreet = Convert.ToString(values[MAILING_STREET]);
            }

            if(values.Contains(MAILING_SUITE)) {
                model.MailingSuite = Convert.ToString(values[MAILING_SUITE]);
            }

            if(values.Contains(MAILING_CITY)) {
                model.MailingCity = Convert.ToString(values[MAILING_CITY]);
            }

            if(values.Contains(MAILING_STATE)) {
                model.MailingState = Convert.ToString(values[MAILING_STATE]);
            }

            if(values.Contains(MAILING_POSTAL_CODE)) {
                model.MailingPostalCode = Convert.ToString(values[MAILING_POSTAL_CODE]);
            }

            if(values.Contains(MAILING_COUNTRY)) {
                model.MailingCountry = Convert.ToString(values[MAILING_COUNTRY]);
            }

            if(values.Contains(PHONE)) {
                model.Phone = Convert.ToString(values[PHONE]);
            }

            if(values.Contains(FAX)) {
                model.Fax = Convert.ToString(values[FAX]);
            }

            if(values.Contains(MOBILE_PHONE)) {
                model.MobilePhone = Convert.ToString(values[MOBILE_PHONE]);
            }

            if(values.Contains(HOME_PHONE)) {
                model.HomePhone = Convert.ToString(values[HOME_PHONE]);
            }

            if(values.Contains(EMAIL)) {
                model.Email = Convert.ToString(values[EMAIL]);
            }

            if(values.Contains(TITLE)) {
                model.Title = Convert.ToString(values[TITLE]);
            }

            if(values.Contains(DEPARTMENT)) {
                model.Department = Convert.ToString(values[DEPARTMENT]);
            }

            if(values.Contains(ASSISTANT_NAME)) {
                model.AssistantName = Convert.ToString(values[ASSISTANT_NAME]);
            }

            if(values.Contains(BIRTHDATE)) {
                model.Birthdate = values[BIRTHDATE] != null ? Convert.ToDateTime(values[BIRTHDATE]) : (DateTime?)null;
            }

            if(values.Contains(DESCRIPTION)) {
                model.Description = Convert.ToString(values[DESCRIPTION]);
            }

            if(values.Contains(OWNER_ID)) {
                model.OwnerId = Convert.ToString(values[OWNER_ID]);
            }

            if(values.Contains(EMAIL_OPT_OUT_DATE)) {
                model.EmailOptOutDate = values[EMAIL_OPT_OUT_DATE] != null ? Convert.ToDateTime(values[EMAIL_OPT_OUT_DATE]) : (DateTime?)null;
            }

            if(values.Contains(HAS_OPTED_OUT_OF_EMAIL)) {
                model.HasOptedOutOfEmail = Convert.ToBoolean(values[HAS_OPTED_OUT_OF_EMAIL]);
            }

            if(values.Contains(CREATED_DATE)) {
                model.CreatedDate = Convert.ToDateTime(values[CREATED_DATE]);
            }

            if(values.Contains(CREATED_BY_ID)) {
                model.CreatedById = Convert.ToString(values[CREATED_BY_ID]);
            }

            if(values.Contains(LAST_MODIFIED_DATE)) {
                model.LastModifiedDate = Convert.ToDateTime(values[LAST_MODIFIED_DATE]);
            }

            if(values.Contains(LAST_MODIFIED_BY_ID)) {
                model.LastModifiedById = Convert.ToString(values[LAST_MODIFIED_BY_ID]);
            }

            if(values.Contains(LAST_ACTIVITY_DATE)) {
                model.LastActivityDate = values[LAST_ACTIVITY_DATE] != null ? Convert.ToDateTime(values[LAST_ACTIVITY_DATE]) : (DateTime?)null;
            }

            if(values.Contains(EMAIL_BOUNCED_REASON)) {
                model.EmailBouncedReason = Convert.ToString(values[EMAIL_BOUNCED_REASON]);
            }

            if(values.Contains(EMAIL_BOUNCED_DATE)) {
                model.EmailBouncedDate = values[EMAIL_BOUNCED_DATE] != null ? Convert.ToDateTime(values[EMAIL_BOUNCED_DATE]) : (DateTime?)null;
            }

            if(values.Contains(ALT_EMAIL)) {
                model.Alt_Email = Convert.ToString(values[ALT_EMAIL]);
            }

            if(values.Contains(ALT_CONTACT)) {
                model.Alt_Contact = Convert.ToString(values[ALT_CONTACT]);
            }

            if(values.Contains(CHILDREN)) {
                model.Children = Convert.ToString(values[CHILDREN]);
            }

            if(values.Contains(DIRECT_LINE)) {
                model.Direct_Line = Convert.ToString(values[DIRECT_LINE]);
            }

            if(values.Contains(EXTENSION)) {
                model.Extension = Convert.ToString(values[EXTENSION]);
            }

            if(values.Contains(INITIAL_MEETING_DETAILS)) {
                model.Initial_Meeting_Details = Convert.ToString(values[INITIAL_MEETING_DETAILS]);
            }

            if(values.Contains(LINKED_IN_PROFILE)) {
                model.LinkedIn_Profile = Convert.ToString(values[LINKED_IN_PROFILE]);
            }

            if(values.Contains(MAILING_LISTS)) {
                model.Mailing_Lists = Convert.ToString(values[MAILING_LISTS]);
            }

            if(values.Contains(OPT_OUT)) {
                model.Opt_Out = Convert.ToString(values[OPT_OUT]);
            }

            if(values.Contains(OPT_OUT_DATE)) {
                model.Opt_Out_Date = values[OPT_OUT_DATE] != null ? Convert.ToDateTime(values[OPT_OUT_DATE]) : (DateTime?)null;
            }

            if(values.Contains(PREFERRED_NAME)) {
                model.Preferred_Name = Convert.ToString(values[PREFERRED_NAME]);
            }

            if(values.Contains(REASSIGNED_DATE)) {
                model.Reassigned_Date = values[REASSIGNED_DATE] != null ? Convert.ToDateTime(values[REASSIGNED_DATE]) : (DateTime?)null;
            }

            if(values.Contains(REFERRAL_DATE)) {
                model.Referral_Date = values[REFERRAL_DATE] != null ? Convert.ToDateTime(values[REFERRAL_DATE]) : (DateTime?)null;
            }

            if(values.Contains(REFERRAL_PARTNER_AGMNT_DATE)) {
                model.Referral_Partner_Agmnt_Date = values[REFERRAL_PARTNER_AGMNT_DATE] != null ? Convert.ToDateTime(values[REFERRAL_PARTNER_AGMNT_DATE]) : (DateTime?)null;
            }

            if(values.Contains(REFERRAL_PARTNER_AGMNT_DETAILS)) {
                model.Referral_Partner_Agmnt_Details = Convert.ToString(values[REFERRAL_PARTNER_AGMNT_DETAILS]);
            }

            if(values.Contains(REFERRING_COMPANY)) {
                model.Referring_Company = Convert.ToString(values[REFERRING_COMPANY]);
            }

            if(values.Contains(REFERRING_CONTACT)) {
                model.Referring_Contact = Convert.ToString(values[REFERRING_CONTACT]);
            }

            if(values.Contains(RELATIONSHIP_STATUS)) {
                model.Relationship_Status = Convert.ToString(values[RELATIONSHIP_STATUS]);
            }

            if(values.Contains(STANDARD_PAY_TERMS)) {
                model.Standard_Pay_Terms = Convert.ToString(values[STANDARD_PAY_TERMS]);
            }

            if(values.Contains(RATING_SORT)) {
                model.Rating_Sort = Convert.ToString(values[RATING_SORT]);
            }

            if(values.Contains(TAX_ID)) {
                model.Tax_ID = Convert.ToString(values[TAX_ID]);
            }

            if(values.Contains(TERM_OF_AGREEMENT)) {
                model.Term_Of_Agreement = Convert.ToString(values[TERM_OF_AGREEMENT]);
            }

            if(values.Contains(TWITTER_PROFILE)) {
                model.Twitter_Profile = Convert.ToString(values[TWITTER_PROFILE]);
            }

            if(values.Contains(TYPE)) {
                model.Type = Convert.ToString(values[TYPE]);
            }

            if(values.Contains(UPDATE_NEEDED)) {
                model.Update_Needed = Convert.ToString(values[UPDATE_NEEDED]);
            }

            if(values.Contains(UPDATE_NEEDED_DATE)) {
                model.Update_Needed_Date = values[UPDATE_NEEDED_DATE] != null ? Convert.ToDateTime(values[UPDATE_NEEDED_DATE]) : (DateTime?)null;
            }

            if(values.Contains(ALT_PHONE_3)) {
                model.Alt_Phone_3 = Convert.ToString(values[ALT_PHONE_3]);
            }

            if(values.Contains(OWNERSHIP_PERCENTAGE)) {
                model.OwnershipPercentage = Convert.ToString(values[OWNERSHIP_PERCENTAGE]);
            }

            if(values.Contains(GUARANTOR)) {
                model.Guarantor = Convert.ToBoolean(values[GUARANTOR]);
            }

            if(values.Contains(NEXT_ACTIVITY_ID)) {
                model.NextActivityID = Convert.ToString(values[NEXT_ACTIVITY_ID]);
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState) {
            var messages = new List<string>();

            foreach(var entry in modelState) {
                foreach(var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }





        #region Report APIs

        [HttpGet]
        public async Task<IActionResult> GetReferralPartnersInSpecificStates(DataSourceLoadOptions loadOptions)
        {
            var contactsAndCompanies = new List<ContactAndCompanyViewModel>();

            var contacts = from con in _context.Contacts
                           join com in _context.Companies on con.AccountId equals com.ID
                           select new { con, com };


            foreach (var contact in contacts)
            {
                var contactAndCompany = new ContactAndCompanyViewModel
                {
                    Id = contact.con.Id,
                    FirstName = contact.con.FirstName,
                    LastName = contact.con.LastName,
                    CompanyName = contact.com.Name,
                    MailingStreet = contact.con.MailingStreet,
                    MailingCity = contact.con.MailingCity,
                    MailingState = contact.con.MailingState,
                    MailingPostalCode = contact.con.MailingPostalCode,
                    Phone = contact.con.Phone,
                    MobilePhone = contact.con.MobilePhone,
                    Email = contact.con.Email,  
                    Rating_Sort = contact.con.Rating_Sort,
                    Relationship_Status = contact.con.Relationship_Status,
                    CompanyType = contact.com.CompanyType,
                    Opt_Out = contact.con.Opt_Out,
                    HasOptedOutOfEmail = contact.con.HasOptedOutOfEmail,
                    OwnerName = _userHelper.GetNameFromID(contact.con.OwnerId)
                };
                contactsAndCompanies.Add(contactAndCompany);
            }

            //var pipelineClientsQueryable = contactsAndCompanies.Select(c => new
            //{
            //    c.Name,
            //    c.OwnerName,
            //    c.CreatedDate
            //}).AsQueryable();

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "Id" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(DataSourceLoader.Load(contactsAndCompanies, loadOptions));
        }


        #endregion

    }
}