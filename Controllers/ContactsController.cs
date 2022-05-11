using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AmeriForce.Data;
using AmeriForce.Helpers;
using AmeriForce.Models.Contacts;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using AmeriForce.Services;
using Microsoft.AspNetCore.Http;
using AmeriForce.Models;
using System.Web;
using AmeriForce.Models.Companies;
using System.Text;
using System.Security.Claims;
using AmeriForce.Models.Email;
using Microsoft.Extensions.Configuration;

namespace AmeriForce.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private GuidHelper _guidHelper = new GuidHelper();
        private CompanyHelper _companyHelper;
        private LOVHelper _lovHelper;
        private UserHelper _userHelper;
        private UploadHelper _uploadHelper;

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly EmailConfigurationViewModel _emailConfig = new EmailConfigurationViewModel();

        public ContactsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            configuration.GetSection("EmailConfiguration").Bind(_emailConfig);

            _lovHelper = new LOVHelper(_context);
            _companyHelper = new CompanyHelper(_context);
            _userHelper = new UserHelper(_context);
            _uploadHelper = new UploadHelper(_context, _webHostEnvironment);
        }

        // GET: Contacts
        public async Task<IActionResult> Index()
        {
            List<ContactIndexViewModel> contactIndexViewModel = new List<ContactIndexViewModel>();
            var contacts = _context.Contacts.Take(200).OrderByDescending(c => c.CreatedDate);
            if (contacts.Count() > 0)
            {
                foreach (var contact in contacts)
                {
                    var contactToList = new ContactIndexViewModel
                    {
                        ID = contact.Id,
                        AccountID = contact.AccountId,
                        OwnerName = GetUserNameFromID(contact.OwnerId),
                        Company = GetCompanyName(contact.AccountId),
                        ContactName = $"{contact.FirstName} {contact.LastName}",
                        Email = contact.Email,
                        Grade = contact.Rating_Sort,
                        Phone = contact.Phone,
                        RelationshipStatus = contact.Relationship_Status
                    };
                    contactIndexViewModel.Add(contactToList);
                }
            }
            return View(contactIndexViewModel);
        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailsModel = new ContactDetailsViewModel();
            Contact contact = _context.Contacts.Find(id);

            string contactFiles = GetContactFiles(id);
            ViewBag.ContactFiles = contactFiles;

            detailsModel.contact = contact;
            detailsModel.OwnerName = GetUserNameFromID(contact.OwnerId);
            detailsModel.clients = _context.Clients.Where(c => c.Referring_Contact == contact.Id);
            detailsModel.contactNotes = _context.CRMTasks.Where(c => c.WhoId == contact.Id && c.Id != contact.NextActivityID).OrderByDescending(c => c.ActivityDate);
            detailsModel.contactNextTask = _context.CRMTasks.Where(c => c.Id == detailsModel.contact.NextActivityID).FirstOrDefault();

            if (detailsModel.contactNextTask == null)
            {
                var nextCRMTask = new CRMTask()
                {
                    Id = new GuidHelper().GetGUIDString("task"),
                    WhoId = detailsModel.contact.Id,
                    Type = "Warning",
                    OwnerId = detailsModel.contact.OwnerId,
                    ActivityDate = Convert.ToDateTime("01/01/2000"),
                    Description = "No activity entered for this contact yet"
                };
                _context.CRMTasks.Add(nextCRMTask);
                contact.NextActivityID = nextCRMTask.Id;
                _context.SaveChanges();

                detailsModel.contactNextTask = _context.CRMTasks.Where(c => c.Id == nextCRMTask.Id).FirstOrDefault();
            }
            detailsModel.contactNextTaskOwner = GetUserNameFromID(detailsModel.contactNextTask.OwnerId);


            detailsModel.TaskList = _lovHelper.GetTaskTypes();
            detailsModel.ActiveUserList = _lovHelper.GetActiveUsers();
            detailsModel.TaskListNotes = _lovHelper.GetTaskTypes();
            detailsModel.ActiveUserListNotes = _lovHelper.GetActiveUsers();
            detailsModel.YesNoList = _lovHelper.GetYesNoList();
            detailsModel.MailMergeTemplateList = _lovHelper.GetMailMergeTemplateList();


            // Duplicate Detection
            var contactForDuplicateDetection = new ContactDuplicateViewModel()
            {
                FirstName = detailsModel.contact.FirstName,
                LastName = detailsModel.contact.LastName,
                Title = detailsModel.contact.Title,
                Address = detailsModel.contact.MailingStreet,
                CRMID = detailsModel.contact.Id
            };
            var contactDuplicates = new ContactDuplicateDetection(_context).DetectDuplicateBasedOnContact(contactForDuplicateDetection);

            if (contactDuplicates != null)
            {
                detailsModel.contactDuplicates = contactDuplicates;
            }

            if (contact == null)
            {
                return NotFound();
            }
            return View(detailsModel);


            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var contact = await _context.Contacts
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (contact == null)
            //{
            //    return NotFound();
            //}

            //return View(contact);
        }

        // GET: Contacts/Create
        public IActionResult Create()
        {
            var contactCreateModel = new ContactCreateViewModel();

            contactCreateModel.BaseList = _lovHelper.GetBaseList();
            contactCreateModel.CompanyList = _lovHelper.GetCompanyDropdownList();
            contactCreateModel.OwnerList = _lovHelper.GetOwnerDropdown();
            contactCreateModel.ClientStageList = _lovHelper.GetClientStages();
            contactCreateModel.BDOList = _lovHelper.GetBDOList();
            contactCreateModel.StateList = _lovHelper.GetStateList();
            contactCreateModel.TaskList = _lovHelper.GetTaskTypes();
            contactCreateModel.ActiveUserList = _lovHelper.GetActiveUsers();
            contactCreateModel.ReferralTypeList = _lovHelper.GetReferralTypes();

            contactCreateModel.RelationshipStatusList = _lovHelper.GetRelationshipStatusList("New");
            contactCreateModel.TagGradeSortList = _lovHelper.GetTagGradeSortList();
            contactCreateModel.UpdateNeededList = _lovHelper.GetUpdateNeededList();
            contactCreateModel.MailingLists = _lovHelper.GetMailingLists();

            return View(contactCreateModel);
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,AccountId,Salutation,FirstName,LastName,MiddleName,OtherStreet,OtherCity,OtherState,OtherPostalCode,OtherCountry,MailingStreet,MailingCity,MailingState,MailingPostalCode,MailingCountry,Phone,Fax,MobilePhone,HomePhone,Email,Title,Department,AssistantName,Birthdate,Description,OwnerId,HasOptedOutOfEmail,CreatedDate,CreatedById,LastModifiedDate,LastModifiedById,LastActivityDate,EmailBouncedReason,EmailBouncedDate,Alt_Email,Alt_Contact,Children,Direct_Line,Extension,Initial_Meeting_Details,LinkedIn_Profile,Mailing_Lists,Opt_Out,Opt_Out_Date,Preferred_Name,Reassigned_Date,Referral_Date,Referral_Partner_Agmnt_Date,Referral_Partner_Agmnt_Details,Referring_Company,Referring_Contact,Relationship_Status,Standard_Pay_Terms,Rating_Sort,Tax_ID,Term_Of_Agreement,Twitter_Profile,Type,Update_Needed,Update_Needed_Date,Alt_Phone_3,OwnershipPercentage,Guarantor,NextActivityID")] Contact contact)
        public async Task<IActionResult> Create(ContactCreateViewModel contactCreateModel, IFormCollection collection)
        {
            try
            {
                #region Validation
                var validationHelper = new ValidationHelper();

                // VALIDATION HERE...
                string firstName = validationHelper.ValidateNonRequiredString(collection["contactData_FirstName"].ToString());
                string lastName = validationHelper.ValidateRequiredString(collection["contactData_LastName"].ToString());
                string preferredName = validationHelper.ValidateNonRequiredString(collection["contactData_PreferredName"].ToString());
                string title = validationHelper.ValidateNonRequiredString(collection["contactData_Title"].ToString());
                string department = validationHelper.ValidateNonRequiredString(collection["contactData_Department"].ToString());
                string bdo = validationHelper.ValidateRequiredString(contactCreateModel.contactData.OwnerId);
                string relationshipStatus = validationHelper.ValidateRequiredString(contactCreateModel.contactData.Relationship_Status);
                string tagGradeSort = validationHelper.ValidateRequiredString(contactCreateModel.contactData.Rating_Sort);

                var updateNeededIntermediate = contactCreateModel.UpdateNeededIntermediate;
                string updateNeeded = "";
                if (updateNeededIntermediate != null)
                {
                    var updateNeededString = string.Join(",", updateNeededIntermediate);
                    updateNeeded = validationHelper.ValidateNonRequiredString(updateNeededString);
                }

                string updateNeededDatePreValidation = collection["contactData_Update_Needed_Date"].ToString();
                //string updateNeededDatePreValidation = (contactModel.contactData.Update_Needed_Date == null) ? contactModel.contactData.Update_Needed_Date.ToString() : "";
                DateTime? updateNeededDate = validationHelper.ValidateDateWithString_NotRequired(updateNeededDatePreValidation);

                bool emailOptOut = contactCreateModel.contactData.HasOptedOutOfEmail;

                string updateEmailOptOutDatePreValidation = collection["contactData_EmailOptOutDate"].ToString();
                //string updateEmailOptOutDatePreValidation = (contactModel.contactData.EmailOptOutDate == null) ? contactModel.contactData.EmailOptOutDate.ToString() : "";
                DateTime? emailOptOutDate = validationHelper.ValidateDateWithString_NotRequired(updateEmailOptOutDatePreValidation);


                var mailingListIntermediate = contactCreateModel.MailingListIntermediate;
                string mailingLists = "";
                if (mailingListIntermediate != null)
                {
                    var mailingListString = string.Join(",", mailingListIntermediate);
                    mailingLists = validationHelper.ValidateNonRequiredString(mailingListString);
                }

                string linkedInProfile = validationHelper.ValidateNonRequiredString(collection["contactData_LinkedIn_Profile"].ToString());
                string email = validationHelper.ValidateEmail(collection["contactData_Email"].ToString());
                string alternateEmail = validationHelper.ValidateEmail(collection["contactData_Alt_Email"].ToString());
                string directLine = validationHelper.ValidatePhone(collection["contactData_Direct_Line"].ToString());
                string businessPhone = validationHelper.ValidatePhone(collection["contactData_Phone"].ToString());
                string mobilePhone = validationHelper.ValidatePhone(collection["contactData_MobilePhone"].ToString());
                string homePhone = validationHelper.ValidatePhone(collection["contactData_HomePhone"].ToString());

                string contactDetails = validationHelper.ValidateNonRequiredString(collection["contactData_Initial_Meeting_Details"].ToString());

                string companyID = validationHelper.ValidateNonRequiredString(collection["CompanyID"].ToString());
                string companyName = validationHelper.ValidateNonRequiredString(collection["CompanyName"].ToString());
                string businessDescription = validationHelper.ValidateNonRequiredString(collection["companyData_Description"].ToString());
                string sicCode = validationHelper.ValidateNonRequiredString(collection["companyData_SICCode"].ToString());
                string charterState = validationHelper.ValidateNonRequiredString(contactCreateModel.companyData.CharterState);
                decimal ownershipPercentage = validationHelper.ValidateDecimalNonNegative(collection["contactData_OwnershipPercentage"].ToString());
                bool guarantor = contactCreateModel.contactData.Guarantor;

                string mailingStreet = validationHelper.ValidateNonRequiredString(collection["contactData_MailingStreet"].ToString());
                string mailingSuite = validationHelper.ValidateNonRequiredString(collection["contactData_MailingSuite"].ToString());
                string mailingCity = validationHelper.ValidateNonRequiredString(collection["contactData_MailingCity"].ToString());
                string mailingState = validationHelper.ValidateNonRequiredString(contactCreateModel.contactData.MailingState);
                string mailingPostalCode = validationHelper.ValidateNonRequiredString(collection["contactData_MailingPostalCode"].ToString());

                string physicalStreet = validationHelper.ValidateNonRequiredString(collection["contactData_OtherStreet"].ToString());
                string physicalSuite = validationHelper.ValidateNonRequiredString(collection["contactData_OtherSuite"].ToString());
                string physicalCity = validationHelper.ValidateNonRequiredString(collection["contactData_OtherCity"].ToString());
                string physicalState = validationHelper.ValidateNonRequiredString(contactCreateModel.contactData.OtherState);
                string physicalPostalCode = validationHelper.ValidateNonRequiredString(collection["contactData_OtherPostalCode"].ToString());

                string referringCompanyString = validationHelper.ValidateNonRequiredString(collection["contactData_Referring_Company"].ToString());
                string referringContactString = validationHelper.ValidateNonRequiredString(collection["contactData_Referring_Contact"].ToString());
                string referringCompany = validationHelper.ValidateNonRequiredString(collection["ReferringCompanyId"].ToString());
                string referringContact = validationHelper.ValidateNonRequiredString(collection["ReferringContactId"].ToString());
                string agreementDetails = validationHelper.ValidateNonRequiredString(collection["contactData_Referral_Partner_Agreement_Details"].ToString());

                string referralDatePreValidation = collection["contactData_Referral_Date"].ToString();
                DateTime? referralDate = validationHelper.ValidateDateWithString_NotRequired(referralDatePreValidation);

                string agreementDatePreValidation = collection["contactData_Referral_Partner_Agmnt_Date"].ToString();
                DateTime? agreementDate = validationHelper.ValidateDateWithString_NotRequired(agreementDatePreValidation);

                string activityCallType = validationHelper.ValidateRequiredString(contactCreateModel.taskData.Type);
                string activityTaskOwner = validationHelper.ValidateRequiredString(contactCreateModel.taskData.OwnerId);

                DateTime activityTaskDatePreValidation = Convert.ToDateTime(collection["taskData_ActivityDate"].ToString());
                DateTime activityTaskDate = validationHelper.ValidateDate(activityTaskDatePreValidation);
                string activityNotes = validationHelper.ValidateRequiredString(collection["taskData_Description"].ToString());
                #endregion

                var transaction = _context.Database.BeginTransaction();

                    var newContact = new Contact()
                    {
                        Id = _guidHelper.GetGUIDString("contact"),
                        AccountId = companyID,
                        FirstName = firstName,
                        LastName = lastName,
                        OtherStreet = physicalStreet,
                        OtherSuite = physicalSuite,
                        OtherCity = physicalCity,
                        OtherState = physicalState,
                        OtherPostalCode = physicalPostalCode,
                        MailingStreet = mailingStreet,
                        MailingSuite = mailingSuite,
                        MailingCity = mailingCity,
                        MailingState = mailingState,
                        MailingPostalCode = mailingPostalCode,
                        Phone = businessPhone,
                        MobilePhone = mobilePhone,
                        HomePhone = homePhone,
                        Email = email,
                        Title = title,
                        Department = department,
                        //Description
                        OwnerId = bdo,
                        HasOptedOutOfEmail = emailOptOut,
                        EmailOptOutDate = emailOptOutDate,
                        CreatedDate = DateTime.Now,
                        CreatedById = "0034W00000APoNIQA1", //UserID,
                        LastModifiedDate = DateTime.Now,
                        LastModifiedById = "0034W00000APoNIQA1", //UserID,
                        LastActivityDate = DateTime.Now,
                        //EmailBouncedReason
                        // EmailBouncedDate
                        Alt_Email = alternateEmail,
                        //Alt_Contact
                        //Children
                        Direct_Line = directLine,
                        //Extension
                        Initial_Meeting_Details = contactDetails,
                        LinkedIn_Profile = linkedInProfile,
                        Mailing_Lists = mailingLists,
                        Opt_Out = emailOptOut.ToString(),
                        Opt_Out_Date = emailOptOutDate,
                        Preferred_Name = preferredName,
                        //Reassigned_Date
                        Referral_Date = referralDate,
                        Referral_Partner_Agmnt_Date = agreementDate,
                        Referral_Partner_Agmnt_Details = agreementDetails,
                        Referring_Company = referringCompany,
                        Referring_Contact = referringContact,
                        Relationship_Status = relationshipStatus,
                        Rating_Sort = tagGradeSort,
                        //Type
                        Update_Needed = updateNeeded,
                        Update_Needed_Date = updateNeededDate,
                        OwnershipPercentage = ownershipPercentage.ToString(),
                        Guarantor = guarantor,
                        //NextActivityID
                    };
                    _context.Contacts.Add(newContact);

                    var company = _context.Companies.Where(c => c.ID == companyID).FirstOrDefault();
                    if (company != null)
                    {
                        company.Name = companyName;
                        company.Description = businessDescription;
                        company.SICCode = sicCode;
                        company.CharterState = charterState;
                    }
                    else
                    {
                        string newCompanyID = new GuidHelper().GetGUIDString("company");
                        var newCompany = new Company()
                        {
                            ID = newCompanyID,
                            Name = companyName,
                            Description = businessDescription,
                            SICCode = sicCode,
                            CharterState = charterState
                        };
                        newContact.AccountId = newCompanyID;
                        _context.Companies.Add(newCompany);
                    }


                    var newReferringCompany = _context.Companies.Where(c => c.ID == referringCompany).FirstOrDefault();
                    if (newReferringCompany != null)
                    {
                        newReferringCompany.Name = referringCompanyString;
                    }
                    else
                    {
                        string newReferringCompanyID = new GuidHelper().GetGUIDString("company");
                        var newReferringCompanyInsert = new Company()
                        {
                            ID = newReferringCompanyID,
                            Name = companyName
                        };
                        newContact.Referring_Company = newReferringCompanyID;
                        _context.Companies.Add(newReferringCompanyInsert);
                    }


                    var newReferringContact = _context.Contacts.Where(c => c.Id == referringContact).FirstOrDefault();
                    if (newReferringContact != null)
                    {
                        newReferringContact.LastName = referringContactString;
                    }
                    else
                    {
                        string newReferringCompanyID = new GuidHelper().GetGUIDString("company");
                        var newReferringCompanyInsert = new Company()
                        {
                            ID = newReferringCompanyID,
                            Name = referringContactString
                        };
                        newContact.Referring_Company = newReferringCompanyID;
                        _context.Companies.Add(newReferringCompanyInsert);
                    }



                    string newTaskID = new GuidHelper().GetGUIDString("task");
                    var newActivity = new CRMTask()
                    {
                        Id = newTaskID,
                        Type = activityCallType,
                        ActivityDate = activityTaskDate,
                        OwnerId = activityTaskOwner,
                        Description = activityNotes
                    };
                    _context.CRMTasks.Add(newActivity);
                    
                    newContact.NextActivityID = newTaskID;

                    _context.SaveChanges();

                    transaction.Commit();

                

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Content("Error");
            }


        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ContactEditViewModel editContactViewModel = new ContactEditViewModel();

            var contactData = await _context.Contacts.FindAsync(id); 
            if (contactData == null)
            {
                return NotFound();
            }

            //var mailingListItems = contactData.Mailing_Lists.Split(',').ToList();
            //List<SelectListItem> selectedMailingLists = new List<SelectListItem>();
            //foreach (var item in mailingListItems)
            //{
            //    selectedMailingLists.Add(new SelectListItem(item, item, true));
            //}
            //editContactViewModel.MailingListIntermediate = new List<SelectListItem>();
            //editContactViewModel.MailingListIntermediate.AddRange(selectedMailingLists); 
            
            
            var companyData = await _context.Companies.Where(c => c.ID == contactData.AccountId).FirstOrDefaultAsync();
            if (companyData == null)
            {
                companyData = await _context.Companies.Where(c => c.ID == "001xxxxxxxxxxxxxxx").FirstOrDefaultAsync();
            }
            List<Client> clients = _context.Clients.Where(c => c.ContactId == id).ToList();
            List<CRMTask> contactNotes = _context.CRMTasks.Where(c => c.WhoId == id).ToList();
            CRMTask contactNextTask = _context.CRMTasks.Where(c => c.Id == contactData.NextActivityID).FirstOrDefault();

            editContactViewModel.contactData = contactData;
            editContactViewModel.companyData = companyData;
            editContactViewModel.ReferralCompanyName = (companyData != null) ? GetCompanyName(companyData.ID) : "";
            editContactViewModel.clients = clients;
            editContactViewModel.contactNotes = contactNotes;
            editContactViewModel.taskData = contactNextTask;

            editContactViewModel.CompanyList = _lovHelper.GetCompanyDropdownList();
            editContactViewModel.TaskList = _lovHelper.GetTaskTypes();
            editContactViewModel.ActiveUserList = _lovHelper.GetActiveUsers();
            editContactViewModel.TaskListNotes = _lovHelper.GetTaskTypes();
            editContactViewModel.ActiveUserListNotes = _lovHelper.GetActiveUsers();
            editContactViewModel.YesNoList = _lovHelper.GetYesNoList();

            editContactViewModel.BDOList = _lovHelper.GetBDOList();
            editContactViewModel.RelationshipStatusList = _lovHelper.GetRelationshipStatusList();
            editContactViewModel.TagGradeSortList = _lovHelper.GetTagGradeSortList();
            editContactViewModel.UpdateNeededList = _lovHelper.GetUpdateNeededList();
            editContactViewModel.MailingLists = _lovHelper.GetMailingLists();
            editContactViewModel.StateList = _lovHelper.GetStateList();


            if (contactData.Mailing_Lists != null)
            {
                var mailingListItems = contactData.Mailing_Lists.Split(',').ToList();
                foreach (var item in mailingListItems)
                {
                    var selectedMailingList = editContactViewModel.MailingLists.Where(m => m.Text == item).FirstOrDefault();
                    if (selectedMailingList != null)
                    {
                        selectedMailingList.Selected = true;
                    }
                }
            }


            if (contactData.Update_Needed != null)
            {
                var updateNeededItems = contactData.Update_Needed.Split(',').ToList();
                foreach (var item in updateNeededItems)
                {
                    var selectedUpdateNeeded = editContactViewModel.UpdateNeededList.Where(m => m.Text == item).FirstOrDefault();
                    if (selectedUpdateNeeded != null)
                    {
                        selectedUpdateNeeded.Selected = true;
                    }
                }
            }


            return View(editContactViewModel);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("Id,AccountId,Salutation,FirstName,LastName,MiddleName,OtherStreet,OtherCity,OtherState,OtherPostalCode,OtherCountry,MailingStreet,MailingCity,MailingState,MailingPostalCode,MailingCountry,Phone,Fax,MobilePhone,HomePhone,Email,Title,Department,AssistantName,Birthdate,Description,OwnerId,HasOptedOutOfEmail,CreatedDate,CreatedById,LastModifiedDate,LastModifiedById,LastActivityDate,EmailBouncedReason,EmailBouncedDate,Alt_Email,Alt_Contact,Children,Direct_Line,Extension,Initial_Meeting_Details,LinkedIn_Profile,Mailing_Lists,Opt_Out,Opt_Out_Date,Preferred_Name,Reassigned_Date,Referral_Date,Referral_Partner_Agmnt_Date,Referral_Partner_Agmnt_Details,Referring_Company,Referring_Contact,Relationship_Status,Standard_Pay_Terms,Rating_Sort,Tax_ID,Term_Of_Agreement,Twitter_Profile,Type,Update_Needed,Update_Needed_Date,Alt_Phone_3,OwnershipPercentage,Guarantor,NextActivityID")] Contact contact)
        public async Task<IActionResult> Edit(string id, ContactEditViewModel contactEditVM, IFormCollection collection)
        {
            //if (id != contact.Id)
            //{
            //    return NotFound();
            //}

            #region Validation
            var validationHelper = new ValidationHelper();

            // VALIDATION HERE...
            string firstName = validationHelper.ValidateNonRequiredString(collection["contactData_FirstName"].ToString());
            string lastName = validationHelper.ValidateRequiredString(collection["contactData_LastName"].ToString());
            string preferredName = validationHelper.ValidateNonRequiredString(collection["contactData_PreferredName"].ToString());
            string title = validationHelper.ValidateNonRequiredString(collection["contactData_Title"].ToString());
            string department = validationHelper.ValidateNonRequiredString(collection["contactData_Department"].ToString());
            string bdo = validationHelper.ValidateRequiredString(contactEditVM.contactData.OwnerId);
            string relationshipStatus = validationHelper.ValidateRequiredString(contactEditVM.contactData.Relationship_Status);
            string tagGradeSort = validationHelper.ValidateRequiredString(contactEditVM.contactData.Rating_Sort);

            var updateNeededIntermediate = contactEditVM.UpdateNeededIntermediate;
            string updateNeeded = "";
            if (updateNeededIntermediate != null)
            {
                var updateNeededString = string.Join(",", updateNeededIntermediate);
                updateNeeded = validationHelper.ValidateNonRequiredString(updateNeededString);
            }

            string updateNeededDatePreValidation = collection["contactData_Update_Needed_Date"].ToString();
            //string updateNeededDatePreValidation = (contactModel.contactData.Update_Needed_Date == null) ? contactModel.contactData.Update_Needed_Date.ToString() : "";
            DateTime? updateNeededDate = validationHelper.ValidateDateWithString_NotRequired(updateNeededDatePreValidation);

            bool emailOptOut = contactEditVM.contactData.HasOptedOutOfEmail;

            string updateEmailOptOutDatePreValidation = collection["contactData_EmailOptOutDate"].ToString();
            //string updateEmailOptOutDatePreValidation = (contactModel.contactData.EmailOptOutDate == null) ? contactModel.contactData.EmailOptOutDate.ToString() : "";
            DateTime? emailOptOutDate = validationHelper.ValidateDateWithString_NotRequired(updateEmailOptOutDatePreValidation);


            string mailingListIntermediate = collection["MailingListIntermediate"];
            string mailingLists = "";
            if (mailingListIntermediate != null)
            {
                var mailingListString = string.Join(",", mailingListIntermediate);
                mailingLists = validationHelper.ValidateNonRequiredString(mailingListString);
            }

            string linkedInProfile = validationHelper.ValidateNonRequiredString(collection["contactData_LinkedIn_Profile"].ToString());
            string email = validationHelper.ValidateEmail(collection["contactData_Email"].ToString());
            string alternateEmail = validationHelper.ValidateEmail(collection["contactData_Alt_Email"].ToString());
            string directLine = validationHelper.ValidatePhone(collection["contactData_Direct_Line"].ToString());
            string businessPhone = validationHelper.ValidatePhone(collection["contactData_Phone"].ToString());
            string mobilePhone = validationHelper.ValidatePhone(collection["contactData_MobilePhone"].ToString());
            string homePhone = validationHelper.ValidatePhone(collection["contactData_HomePhone"].ToString());

            string contactDetails = validationHelper.ValidateNonRequiredString(collection["contactData_Initial_Meeting_Details"].ToString());

            string companyID = validationHelper.ValidateNonRequiredString(collection["CompanyID"].ToString());
            string companyName = validationHelper.ValidateNonRequiredString(collection["CompanyName"].ToString());
            string businessDescription = validationHelper.ValidateNonRequiredString(collection["companyData_Description"].ToString());
            string sicCode = validationHelper.ValidateNonRequiredString(collection["companyData_SICCode"].ToString());
            string charterState = validationHelper.ValidateNonRequiredString(contactEditVM.companyData.CharterState);
            decimal ownershipPercentage = validationHelper.ValidateDecimalNonNegative(collection["contactData_OwnershipPercentage"].ToString());
            bool guarantor = contactEditVM.contactData.Guarantor;

            string mailingStreet = validationHelper.ValidateNonRequiredString(collection["contactData_MailingStreet"].ToString());
            string mailingSuite = validationHelper.ValidateNonRequiredString(collection["contactData_MailingSuite"].ToString());
            string mailingCity = validationHelper.ValidateNonRequiredString(collection["contactData_MailingCity"].ToString());
            string mailingState = validationHelper.ValidateNonRequiredString(contactEditVM.contactData.MailingState);
            string mailingPostalCode = validationHelper.ValidateNonRequiredString(collection["contactData_MailingPostalCode"].ToString());

            string physicalStreet = validationHelper.ValidateNonRequiredString(collection["contactData_OtherStreet"].ToString());
            string physicalSuite = validationHelper.ValidateNonRequiredString(collection["contactData_OtherSuite"].ToString());
            string physicalCity = validationHelper.ValidateNonRequiredString(collection["contactData_OtherCity"].ToString());
            string physicalState = validationHelper.ValidateNonRequiredString(contactEditVM.contactData.OtherState);
            string physicalPostalCode = validationHelper.ValidateNonRequiredString(collection["contactData_OtherPostalCode"].ToString());

            string referringCompanyString = validationHelper.ValidateNonRequiredString(collection["contactData_Referring_Company"].ToString());
            string referringContactString = validationHelper.ValidateNonRequiredString(collection["contactData_Referring_Contact"].ToString());
            string referringCompany = validationHelper.ValidateNonRequiredString(collection["ReferringCompanyId"].ToString());
            string referringContact = validationHelper.ValidateNonRequiredString(collection["ReferringContactId"].ToString());
            string agreementDetails = validationHelper.ValidateNonRequiredString(collection["contactData_Referral_Partner_Agreement_Details"].ToString());

            string referralDatePreValidation = collection["contactData_Referral_Date"].ToString();
            DateTime? referralDate = validationHelper.ValidateDateWithString_NotRequired(referralDatePreValidation);

            string agreementDatePreValidation = collection["contactData_Referral_Partner_Agmnt_Date"].ToString();
            DateTime? agreementDate = validationHelper.ValidateDateWithString_NotRequired(agreementDatePreValidation);

            string activityID = validationHelper.ValidateRequiredString(collection["NextScheduledTaskID"]);
            string activityCallType = validationHelper.ValidateRequiredString(contactEditVM.taskData.Type);
            string activityTaskOwner = validationHelper.ValidateRequiredString(contactEditVM.taskData.OwnerId);

            DateTime activityTaskDatePreValidation = Convert.ToDateTime(collection["taskData_ActivityDate"].ToString());
            DateTime activityTaskDate = validationHelper.ValidateDate(activityTaskDatePreValidation);
            string activityNotes = validationHelper.ValidateRequiredString(collection["taskData_Description"].ToString());
            #endregion


            try {

                var transaction = _context.Database.BeginTransaction();


                var updatedContact = _context.Contacts.Where(c=>c.Id == id).FirstOrDefault();
                if (updatedContact != null) {
                    updatedContact.AccountId = companyID;
                    updatedContact.FirstName = firstName;
                    updatedContact.LastName = lastName;
                    updatedContact.OtherStreet = physicalStreet;
                    updatedContact.OtherSuite = physicalSuite;
                    updatedContact.OtherCity = physicalCity;
                    updatedContact.OtherState = physicalState;
                    updatedContact.OtherPostalCode = physicalPostalCode;
                    updatedContact.MailingStreet = mailingStreet;
                    updatedContact.MailingSuite = mailingSuite;
                    updatedContact.MailingCity = mailingCity;
                    updatedContact.MailingState = mailingState;
                    updatedContact.MailingPostalCode = mailingPostalCode;
                    updatedContact.Phone = businessPhone;
                    updatedContact.MobilePhone = mobilePhone;
                    updatedContact.HomePhone = homePhone;
                    updatedContact.Email = email;
                    updatedContact.Title = title;
                    updatedContact.Department = department;
                    //Description
                    updatedContact.OwnerId = bdo;
                    updatedContact.HasOptedOutOfEmail = emailOptOut;
                    updatedContact.EmailOptOutDate = emailOptOutDate;
                    updatedContact.CreatedDate = DateTime.Now;
                    updatedContact.CreatedById = "0034W00000APoNIQA1"; //UserID;
                    updatedContact.LastModifiedDate = DateTime.Now;
                    updatedContact.LastModifiedById = "0034W00000APoNIQA1"; //UserID;
                    updatedContact.LastActivityDate = DateTime.Now;
                    //EmailBouncedReason
                    // EmailBouncedDate
                    updatedContact.Alt_Email = alternateEmail;
                    //Alt_Contact
                    //Children
                    updatedContact.Direct_Line = directLine;
                    //Extension
                    updatedContact.Initial_Meeting_Details = contactDetails;
                    updatedContact.LinkedIn_Profile = linkedInProfile;
                    updatedContact.Mailing_Lists = mailingLists;
                    updatedContact.Opt_Out = emailOptOut.ToString();
                    updatedContact.Opt_Out_Date = emailOptOutDate;
                    updatedContact.Preferred_Name = preferredName;
                    //Reassigned_Date
                    updatedContact.Referral_Date = referralDate;
                    updatedContact.Referral_Partner_Agmnt_Date = agreementDate;
                    updatedContact.Referral_Partner_Agmnt_Details = agreementDetails;
                    updatedContact.Referring_Company = referringCompany;
                    updatedContact.Referring_Contact = referringContact;
                    updatedContact.Relationship_Status = relationshipStatus;
                    updatedContact.Rating_Sort = tagGradeSort;
                    //Type
                    updatedContact.Update_Needed = updateNeeded;
                    updatedContact.Update_Needed_Date = updateNeededDate;
                    updatedContact.OwnershipPercentage = ownershipPercentage.ToString();
                    updatedContact.Guarantor = guarantor;
                //NextActivityID
            }

            var company = _context.Companies.Where(c => c.ID == companyID).FirstOrDefault();
            if (company != null)
            {
                company.Name = companyName;
                company.Description = businessDescription;
                company.SICCode = sicCode;
                company.CharterState = charterState;
            }
            else
            {
                string newCompanyID = new GuidHelper().GetGUIDString("company");
                var newCompany = new Company()
                {
                    ID = newCompanyID,
                    Name = companyName,
                    Description = businessDescription,
                    SICCode = sicCode,
                    CharterState = charterState
                };
               // newContact.AccountId = newCompanyID;
                _context.Companies.Add(newCompany);
            }


            var newReferringCompany = _context.Companies.Where(c => c.ID == referringCompany).FirstOrDefault();
            if (newReferringCompany != null)
            {
                newReferringCompany.Name = referringCompanyString;
            }
            else
            {
                string newReferringCompanyID = new GuidHelper().GetGUIDString("company");
                var newReferringCompanyInsert = new Company()
                {
                    ID = newReferringCompanyID,
                    Name = companyName
                };
                //newContact.Referring_Company = newReferringCompanyID;
                _context.Companies.Add(newReferringCompanyInsert);
            }


            var newReferringContact = _context.Contacts.Where(c => c.Id == referringContact).FirstOrDefault();
            if (newReferringContact != null)
            {
                newReferringContact.LastName = referringContactString;
            }
            else
            {
                string newReferringCompanyID = new GuidHelper().GetGUIDString("company");
                var newReferringCompanyInsert = new Company()
                {
                    ID = newReferringCompanyID,
                    Name = referringContactString
                };
                //newContact.Referring_Company = newReferringCompanyID;
                _context.Companies.Add(newReferringCompanyInsert);
            }



            //string newTaskID = new GuidHelper().GetGUIDString("task");
            //var newActivity = new CRMTask()
            //{
            //    Id = newTaskID,
            //    Type = activityCallType,
            //    ActivityDate = activityTaskDate,
            //    OwnerId = activityTaskOwner,
            //    Description = activityNotes
            //};
            //_context.CRMTasks.Add(newActivity);

            var nextScheduledTask = _context.CRMTasks.Where(c=> c.Id == activityID).FirstOrDefault();
                if (nextScheduledTask != null)
                {
                    nextScheduledTask.Type = activityCallType;
                    nextScheduledTask.ActivityDate = activityTaskDate;
                    nextScheduledTask.OwnerId = activityTaskOwner;
                    nextScheduledTask.Description = activityNotes;
                }

                // newContact.NextActivityID = newTaskID;

                _context.SaveChanges();

            transaction.Commit();



            return RedirectToAction("Index");
        }
            catch (Exception ex)
            {
                return Content("Error");
    }


            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(contact);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!ContactExists(contact.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            return View(contactEditVM);
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Upload(string id)
        {
            ViewBag.ClientID = id;
            return View();
        }


        [HttpPost, ActionName("Upload")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormCollection collection)
        {
            var fileUploadModel = new FileUploadViewModel();
            //var fileDescription = collection["upload_FileName"];
            var contactID = Convert.ToString(collection["uploadContactID"]);
            var fileUser = User.Identity.Name;
            var userFullName = User.Identity.Name;

            _uploadHelper.CreatedocumentDirectory(contactID, "Contacts");
            string webRootPath = _webHostEnvironment.WebRootPath;
            var fileLocation = $"Documents/Contacts/{contactID}";
            var uploadedFiles = HttpContext.Request.Form.Files;

            var user = _context.Users.SingleOrDefault(u => u.UserName == userFullName);

            try
            {
                List<string> fileUploads = new List<string>();
                foreach (IFormFile uploadedFile in uploadedFiles)
                {
                    string fileName = Path.GetFileName(uploadedFile.FileName);
                    fileName = fileName.Replace("'", "\\'");
                    using (FileStream stream = new FileStream(Path.Combine(webRootPath, fileLocation, fileName), FileMode.Create))
                    {
                        uploadedFile.CopyTo(stream);

                        fileUploads.Add(fileName);
                        ViewBag.UploadStatus += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                    }
                }

                return RedirectToAction("Details", "Contacts", new { id = contactID, uploadStatus = "SUCCESS" });
            }
            catch
            {
                ViewBag.FileUploadSuccess = "Fail";
                return View();
            }
        }

        public ActionResult SendEmail(string id)
        {
            var tempEmailID = new GuidHelper().GetGUIDString("tempemail");
            var sendEmailModel = new ContactSendEmailMessageViewModel();
            sendEmailModel.ContactName = GetContactName(id);
            sendEmailModel.RelatedTo = id;
            sendEmailModel.TemporaryID = tempEmailID;
            sendEmailModel.FromName = _userHelper.GetNameFromEmail(_httpContextAccessor.HttpContext.User.Identity.Name);
            sendEmailModel.FromAddress = HttpContext.User.Identity.Name;
            sendEmailModel.UserEmailList = _lovHelper.GetActiveUsersEmailList();
            sendEmailModel.ContactEmailList = _lovHelper.GetContactEmailListByContactId(id);
            return View(sendEmailModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendEmail(string id, IFormCollection collection)
        {

            IEmailService emailService = new EmailService(_emailConfig);
            emailService.Send(collection["FromName"].ToString(), collection["FromAddress"].ToString(),"jasonpzeller@gmail.com", collection["CcAddress"].ToString(), collection["BccAddress"].ToString(), collection["Subject"].ToString(), collection["HTMLBody"].ToString());


            //var emailHelper = new SendGridEmailHelper();

            //var toAddressEmail = "jasonpzeller@gmail.com"; // new ContactHelper().GetEmailAddress(collection["ToAddress"].ToString());
            //var collect = collection["ToAddress"].ToString();
            //var collect2 = collection["CcAddress"].ToString();

            //var tos = new List<string> { toAddressEmail };
            //var ccs = new List<string> { "bigjpztri@gmail.com", "zellerff@yahoo.com" };
            //var bccs = new List<string> { "jzeller@amerisource.us.com", };
            //string tosCombined = string.Join(",", tos);
            //string ccsCombined = string.Join(",", ccs);
            //string bccsCombined = string.Join(",", bccs);

            //var email = new OutlookEmailHelper();

            //var newEmail = new EmailViewModel
            //{
            //    FromAddress = "admin@amerisource.us.com",
            //    ToAddresses = tosCombined,
            //    CCAddresses = ccsCombined,
            //    BCCAddresses = bccsCombined,
            //    Subject = "This is a test of the email system",
            //    Body = new StringBuilder("Hello, welcome to our test!<br><br><img src='http://webs2:88/Intranet/Home/GetImg/28' />")
            //};

            //var emailStatus = email.SendEmail(newEmail);

            //return Content(emailStatus.ToString());

            return Content($"{collection["Subject"].ToString()} {collection["HTMLBody"].ToString()}");
        }


















        [HttpPost]
        public JsonResult UpdateNextScheduledCall(string NextCallID,
                                                            string NextCallType,
                                                            string NextCallOwnerID,
                                                            string NextCallActivityDate,
                                                            string NextCallDescription)
        {

            try
            {
                var existingCall = _context.CRMTasks.Where(c => c.Id == NextCallID).FirstOrDefault();

                if (existingCall != null)
                {
                    existingCall.Type = NextCallType;
                    existingCall.OwnerId = NextCallOwnerID;
                    existingCall.ActivityDate = Convert.ToDateTime(NextCallActivityDate);
                    existingCall.Description = NextCallDescription;
                    //existingCall.CreatedById = User.Identity.Name;
                    existingCall.LastModifiedById = User.Identity.Name;
                    existingCall.LastModifiedDate = DateTime.Now;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }

            return Json(new { callID = NextCallOwnerID });
        }


        [HttpPost]
        public JsonResult UpdateContactNote(string ContactID,
                                            string NextNoteType,
                                           string NextNoteOwnerID,
                                           string NextNoteActivityDate,
                                           string NextNoteDescription)
        {

            try
            {
                var newNote = new CRMTask()
                {
                    Id = new GuidHelper().GetGUIDString("task"),
                    WhoId = ContactID,
                    Type = NextNoteType,
                    OwnerId = NextNoteOwnerID,
                    ActivityDate = DateTime.Now,
                    Description = $"{NextNoteDescription}<br>Complete By: {NextNoteActivityDate}",
                    CreatedById = User.Identity.Name
                };
                _context.CRMTasks.Add(newNote);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }

            return Json(new
            {
                callID = NextNoteOwnerID
            });
        }


        [HttpPost]
        public JsonResult MailMergeWordDoc(string ContactID, string TemplateType, string OwnerID)
        {
            var mailMergeHelper = new MailMergeHelper(_context, OwnerID, "Contacts", TemplateType, ContactID);
            //mailMergeHelper.WordDocumentMailMerge();

            return Json(new
            {
                resultMessage = "success"
            });
        }


        [HttpPost]
        public JsonResult ContactReassignToNewOwner(string ContactID, string NewOwnerID, string OwnerID)
        {
            try
            {
                var currentContact = _context.Contacts.Where(c => c.Id == ContactID).FirstOrDefault();
                if (currentContact != null && OwnerID != "")
                {
                    currentContact.OwnerId = NewOwnerID;
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }

            return Json(new
            {
                resultMessage = "success"
            });
        }



        [HttpPost]
        public JsonResult GetAutocompleteFeature_Companies(string enteredText)
        {

            var companyName = (from company in _lovHelper.GetCompanyDropdownList()
                               where company.Text.ToUpper().StartsWith(enteredText.ToUpper())
                               select new { company.Text });
            return Json(companyName);
        }


        [HttpPost]
        public JsonResult GetCompanyID(string enteredText)
        {

            var companyInfo = (from company in _context.Companies.ToList()
                               where company.Name.ToUpper().StartsWith(enteredText.ToUpper())
                               select new { company.ID, company.Name, company.Description, company.SICCode, company.CharterState });

            return Json(companyInfo);
        }

        public JsonResult GetContactListByCompany(string id)
        {
            List<SelectListItem> contacts = new List<SelectListItem>();
            contacts = _lovHelper.GetContactListByCompany(id).ToList();


            return Json(new SelectList(contacts, "Value", "Text"));
        }



        public async Task<ActionResult> ContactMergeTest(string id)
        {
            var potentialMerges = new CompanyContactMerge(id, _context).GetPotentialContactsToMerge();

            return Content("");
        }


        private bool ContactExists(string id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }

        public string GetCompanyName(string id)
        {
            return _companyHelper.GetCompanyName(id);
        }

        public string GetContactName(string id)
        {
            return new ContactHelper(_context).GetName(id);
        }

        public string GetUserNameFromID(string id)
        {
            return _userHelper.GetNameFromID(id);
        }

        public string GetUserIDFromUserName(string userName)
        {
            return _userHelper.GetIDFromName(userName);
        }

        public string GetEmailFromUserName(string userName)
        {
            return _userHelper.GetEmailFromUserName(userName);
        }

        // GET UPLOADED FILES
        internal string GetContactFiles(string id)
        {
            var returnString = "";

            string webRootPath = _webHostEnvironment.WebRootPath;
            var fileLocation = $"Documents/Contacts/{id}";
            var fileTypeFontAwesome = "file";
            var colorFontAwesome = "80a7d8";

            try
            {
                returnString += "<div class='container'><div class='row'><table class='table table-condensed table-hover table-striped' style='width:100%;padding:0px;'>";

                var files = new DirectoryInfo(Path.Combine(webRootPath, fileLocation)).GetFiles().OrderByDescending(f => f.CreationTime);

                foreach (var f in files)
                {
                    var fileInfo = new FileInfo(f.ToString());
                    string scrubbedF = f.ToString().Replace("'", "\\'");

                    if (fileInfo.Extension.ToUpper().Contains("PDF"))
                    {
                        fileTypeFontAwesome = "file-pdf";
                        colorFontAwesome = "ff0000";
                    }

                    if (fileInfo.Extension.ToUpper().Contains("DOC"))
                    {
                        fileTypeFontAwesome = "file-word";
                        colorFontAwesome = "0078d7 ";
                    }

                    if (fileInfo.Extension.ToUpper().Contains("XLS") || fileInfo.Extension.ToUpper().Contains("CSV"))
                    {
                        fileTypeFontAwesome = "file-excel";
                        colorFontAwesome = "1D6F42";
                    }

                    if (fileInfo.Extension.ToUpper().Contains("PNG") || fileInfo.Extension.ToUpper().Contains("JPG") || fileInfo.Extension.ToUpper().Contains("JPEG")
                                || fileInfo.Extension.ToUpper().Contains("GIF") || fileInfo.Extension.ToUpper().Contains("TIFF"))
                    {
                        fileTypeFontAwesome = "file-image";
                        colorFontAwesome = "999";
                    }

                    //returnString += "<a href='../../Images/DecisionLogicReports/" + Path.GetFileName(f) + "' target='_blank'><span class='glyphicon glyphicon-file' data-toggle='tooltip' title='" + Path.GetFileName(f) + "' style='color:#80a7d8;'></span></a>";
                    //returnString += String.Format("<a href='../../Uploads/CBR/{0}/{1}' target='_blank'><span class='glyphicon glyphicon-file' data-toggle='tooltip' title='{1}' style='color:#80a7d8;'></span></a><br>", id, Path.GetFileName(f));

                    returnString += $@"<tr><td style='width:5%;padding:3px;' class='leftTextAlign'>
                                            <i class='fa fa-{fileTypeFontAwesome}' data-toggle='tooltip' title='{1}' style='color:#{colorFontAwesome};font-size:14px;padding:3px;'></i></td>
                                                <td class='leftTextAlign' style='width:60%;padding:3px;'><a href='../../Documents/Contacts/{id}/{Path.GetFileName(f.ToString()).Replace("'", HttpUtility.UrlEncode("'"))}' target='_blank'><span style='font-size:12px;'>{Path.GetFileName(f.ToString())}</span>
                                                </a></td>
                                                <td style='width:35%;padding:3px;'>Uploaded: {fileInfo.LastWriteTime.ToString()}</td>
                                                </tr>";

                    //fileInfo.LastWriteTime.ToString() + "</tr></table></div></div>", id, Path.GetFileName(f), "<b>" + Path.GetFileName(f) + "</b><br>Uploaded: " +
                    //fileInfo.LastWriteTime.ToString() + "<br>Last Opened: " + fileInfo.LastAccessTime.ToString(), fileTypeFontAwesome, colorFontAwesome);
                }
                returnString += "</table></div></div>";
            }
            catch
            {
                return "<div class='well well-sm' style='margin:2px;'>No Files Yet</div>";
            }
            return returnString;

        }


    }
}
