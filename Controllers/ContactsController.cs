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
using Microsoft.AspNetCore.Authorization;

namespace AmeriForce.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private GuidHelper _guidHelper = new GuidHelper();
        private CompanyHelper _companyHelper;
        private ContactHelper _contactHelper;
        private LOVHelper _lovHelper;
        private LOVHelper _lovHelperFiles;
        private UserHelper _userHelper;
        private UploadHelper _uploadHelper;
        private MailMergeHelper _mailMergeHelper;

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly EmailConfigurationViewModel _emailConfig = new EmailConfigurationViewModel();

        private readonly string _userID;

        public ContactsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            configuration.GetSection("EmailConfiguration").Bind(_emailConfig);

            _lovHelper = new LOVHelper(_context);
            _lovHelperFiles = new LOVHelper(_context, _webHostEnvironment);
            _companyHelper = new CompanyHelper(_context);
            _contactHelper = new ContactHelper(_context);
            _userHelper = new UserHelper(_context);
            _mailMergeHelper = new MailMergeHelper(_context, webHostEnvironment);
            _uploadHelper = new UploadHelper(_context, _webHostEnvironment);
            _userID = _userHelper.GetIDFromName(_httpContextAccessor.HttpContext.User.Identity.Name);
        }

        // GET: Contacts
        public async Task<IActionResult> Index(int? id)
        {

            List<ContactIndexViewModel> contactIndexViewModel = new List<ContactIndexViewModel>();
            //var contacts = _context.Contacts.OrderByDescending(c => c.CreatedDate);

            //switch (id)
            //{
            //    case 1:
            //        ViewBag.SubTitle = "New Contacts";
            //        ViewBag.TimeStamp = DateTime.Now.ToString("MMMM dd, yyyy HH:mm:ss");
            //        //string userID = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault().Id;
            //        contacts = _context.Contacts.Where(c => c.Relationship_Status == "New").OrderByDescending(c => c.CreatedDate);
            //        ViewBag.ClientCount = contacts.Count();
            //        //return View(contactList);
            //        break;
            //    case 2:
            //        ViewBag.SubTitle = "My Contacts (All)";
            //        ViewBag.TimeStamp = DateTime.Now.ToString("MMMM dd, yyyy HH:mm:ss");
            //        //string userIDAll = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault().Id;
            //        contacts = _context.Contacts.Where(c => c.OwnerId == _userID && (c.Rating_Sort == "A" || c.Rating_Sort == "B" || c.Rating_Sort == "C" || c.Rating_Sort == "D")).Take(1000).OrderBy(c => c.Rating_Sort);
            //        ViewBag.ContactCount = contacts.Count();
            //        //return View(contactListAll); 
            //        break;
            //    case 3:
            //        ViewBag.SubTitle = "Active Referral Partner Contacts";
            //        ViewBag.TimeStamp = DateTime.Now.ToString("MMMM dd, yyyy HH:mm:ss");
            //        contacts = _context.Contacts.Where(c => c.OwnerId == _userID && c.Relationship_Status == "Active").OrderBy(c => c.LastModifiedDate);
            //        ViewBag.ContactCount = contacts.Count();
            //        //return View(activeReferralPartners);
            //        break;
            //    default:
            //        ViewBag.SubTitle = "All Contacts";
            //        ViewBag.TimeStamp = DateTime.Now.ToString("MMMM dd, yyyy HH:mm:ss");
            //        contacts = _context.Contacts.OrderByDescending(c => c.CreatedDate);
            //        ViewBag.ContactCount = contacts.Count();
            //        //return View(contactListDefault);
            //        break;
            //}

            //if (contacts.Count() > 0)
            //{
            //    foreach (var contact in contacts)
            //    {
            //        var contactToList = new ContactIndexViewModel
            //        {
            //            ID = contact.Id,
            //            AccountID = contact.AccountId,
            //            OwnerName = GetUserNameFromID(contact.OwnerId),
            //            Company = GetCompanyName(contact.AccountId),
            //            ContactName = (!String.IsNullOrEmpty(contact.FirstName)) ? $"{contact.FirstName} {contact.LastName}" : $"{contact.LastName}",
            //            Email = contact.Email,
            //            Grade = contact.Rating_Sort,
            //            Phone = contact.Phone,
            //            RelationshipStatus = contact.Relationship_Status
            //        };
            //        contactIndexViewModel.Add(contactToList);
            //    }
            //}
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
            string mailMergedFiled = GetMailMergedFiles(id);
            ViewBag.ContactFiles = contactFiles;
            ViewBag.MailMergedFiles = mailMergedFiled;

            detailsModel.contact = contact;
            detailsModel.OwnerName = GetUserNameFromID(contact.OwnerId);
            detailsModel.CompanyName = GetCompanyName(contact.AccountId);
            detailsModel.ReferringCompany = GetCompanyName(contact.Referring_Company);
            detailsModel.ReferringContact = GetContactName(contact.Referring_Contact);
            detailsModel.clients = _context.Clients.Where(c => c.Referring_Contact == contact.Id);

            var contactNotes = _context.CRMTasks.Where(c => c.WhoId == contact.Id && c.Id != contact.NextActivityID).OrderByDescending(c => c.ActivityDate);
            if (contactNotes != null)
            {
                var existingNotes = new List<ContactNoteViewModel>();
                foreach(var contactNote in contactNotes)
                {
                    var existingNote = new ContactNoteViewModel()
                    {
                        Owner = GetUserNameFromID(contactNote.OwnerId),
                        cDate = contactNote.ActivityDate?.ToString("MM/dd/yyyy") ?? "",
                        cTime = contactNote.ActivityDate?.ToString("hh:mm:ss tt") ?? "",
                        Type = contactNote.Type,
                        Subject = contactNote.Subject,
                        Description = contactNote.Description
                    };
                    existingNotes.Add(existingNote);
                }
                detailsModel.contactNotesViewModel = existingNotes;

            }

            detailsModel.contactNextTask = _context.CRMTasks.Where(c => c.Id == detailsModel.contact.NextActivityID).FirstOrDefault();
            detailsModel.emailMessages = _context.EmailMessages.Where(e => e.RelatedTo == id).OrderByDescending(e=>e.CreatedDate).ToList();

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
            detailsModel.MailMergeTemplateList = _lovHelperFiles.GetMailMergeTemplateList("contacts");


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
                    CreatedById = _userHelper.GetIDFromName(User.Identity.Name),
                    LastModifiedDate = DateTime.Now,
                    LastModifiedById = _userHelper.GetIDFromName(User.Identity.Name),
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
                _context.SaveChanges();

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
                        CharterState = charterState,
                        CreatedBy = _userHelper.GetIDFromName(User.Identity.Name),
                        CreatedDate = DateTime.Now,
                    };
                    newContact.AccountId = newCompanyID;
                    _context.Companies.Add(newCompany);
                }

                if (!String.IsNullOrEmpty(referringCompany))
                {
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
                }

                if (!String.IsNullOrEmpty(referringContact))
                {
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
                }



                string newTaskID = new GuidHelper().GetGUIDString("task");
                var newActivity = new CRMTask()
                {
                    Id = newTaskID,
                    WhoId = newContact.Id,
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
            editContactViewModel.ReferralCompanyName = (contactData.Referring_Company != null) ? GetCompanyName(contactData.Referring_Company) : "";
            editContactViewModel.clients = clients;
            editContactViewModel.contactNotes = contactNotes; 
            

            if (contactNextTask == null)
            {
                string newTaskID = new GuidHelper().GetGUIDString("task");
                var nextCRMTask = new CRMTask()
                {
                    Id = newTaskID,
                    WhoId = editContactViewModel.contactData.Id,
                    Type = "Warning",
                    OwnerId = editContactViewModel.contactData.OwnerId,
                    ActivityDate = Convert.ToDateTime("01/01/2000"),
                    Description = "This contact is either old or has just been transferred in the data migration.  Please update the next available task to remain current and show up on the home page calendar"
                };
                _context.CRMTasks.Add(nextCRMTask);
                contactData.NextActivityID = newTaskID;
                _context.SaveChanges();

                editContactViewModel.taskData = _context.CRMTasks.Where(c => c.Id == nextCRMTask.Id).FirstOrDefault();
            } else
            {
                editContactViewModel.taskData = contactNextTask;
            }
            


            editContactViewModel.CompanyList = _lovHelper.GetCompanyDropdownList();
            editContactViewModel.TaskList = _lovHelper.GetTaskTypes();
            editContactViewModel.ActiveUserList = _lovHelper.GetActiveUsers();
            editContactViewModel.TaskListNotes = _lovHelper.GetTaskTypes();
            editContactViewModel.ActiveUserListNotes = _lovHelper.GetActiveUsers();
            editContactViewModel.YesNoList = _lovHelper.GetYesNoList();

            editContactViewModel.BDOList = _lovHelper.GetBDOListWithInactive();
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
                var updateNeededItems = contactData.Update_Needed.Split(';').ToList();
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

            //var d = DateTime.ParseExact(collection["taskData_ActivityDate"].ToString(), "MM/dd/yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);


            //DateTime dateValue;
            //bool isDate = DateTime.TryParse(collection["taskData_ActivityDate"].ToString(), out dateValue);
            //if (isDate)
            //{
            //    //return dateValue;
            //}
           

            //var x = 1;
            //var activityTaskDatePrePreVal = collection["taskData_ActivityDate"].ToString("yyyy-MM-dd HH:mm:ss").Replace(' ', 'T');
            //string activityTaskDatePrePreVal = contactEditVM.taskData.ActivityDate.ToString().Replace('T', ' ');

            DateTime activityTaskDatePreValidation = Convert.ToDateTime(contactEditVM.taskData.ActivityDate);
            DateTime activityTaskDate = validationHelper.ValidateDate(activityTaskDatePreValidation);
            string activityNotes = validationHelper.ValidateRequiredString(collection["taskData_Description"].ToString());
            #endregion


            try {

                var transaction = _context.Database.BeginTransaction();


                var updatedContact = _context.Contacts.Where(c => c.Id == id).FirstOrDefault();
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
                    updatedContact.OwnerId = bdo;
                    updatedContact.HasOptedOutOfEmail = emailOptOut;
                    updatedContact.EmailOptOutDate = emailOptOutDate;
                    updatedContact.LastModifiedDate = DateTime.Now;
                    updatedContact.LastModifiedById = _userHelper.GetIDFromName(User.Identity.Name);
                    updatedContact.LastActivityDate = DateTime.Now;
                    updatedContact.Alt_Email = alternateEmail;
                    updatedContact.Direct_Line = directLine;
                    updatedContact.Initial_Meeting_Details = contactDetails;
                    updatedContact.LinkedIn_Profile = linkedInProfile;
                    updatedContact.Mailing_Lists = mailingLists;
                    updatedContact.Opt_Out = emailOptOut.ToString();
                    updatedContact.Opt_Out_Date = emailOptOutDate;
                    updatedContact.Preferred_Name = preferredName;
                    updatedContact.Referral_Date = referralDate;
                    updatedContact.Referral_Partner_Agmnt_Date = agreementDate;
                    updatedContact.Referral_Partner_Agmnt_Details = agreementDetails;
                    updatedContact.Referring_Company = referringCompany;
                    updatedContact.Referring_Contact = referringContact;
                    updatedContact.Relationship_Status = relationshipStatus;
                    updatedContact.Rating_Sort = tagGradeSort;
                    updatedContact.Update_Needed = updateNeeded;
                    updatedContact.Update_Needed_Date = updateNeededDate;
                    updatedContact.OwnershipPercentage = ownershipPercentage.ToString();
                    updatedContact.Guarantor = guarantor;
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


                if (!String.IsNullOrEmpty(referringCompany))
                {
                    var newReferringCompany = _context.Companies.Where(c => c.ID == referringCompany).FirstOrDefault();
                    if (newReferringCompany != null)
                    {
                        //newReferringCompany.Name = referringCompanyString;
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
                }


                if (!String.IsNullOrEmpty(referringContact))
                {
                    var newReferringContact = _context.Contacts.Where(c => c.Id == referringContact).FirstOrDefault();
                    if (newReferringContact != null)
                    {
                        //newReferringContact.LastName = referringContactString;
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

                var nextScheduledTask = _context.CRMTasks.Where(c => c.Id == activityID).FirstOrDefault();
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
            string emailGUID = _guidHelper.GetGUIDString("email");

            UploadContactEmailAttachments(id, emailGUID, collection);

            List<string> contactEmailFiles = GetEmailContactFiles(id, emailGUID);

            IEmailService emailService = new EmailService(_emailConfig);

            var toAddress = _contactHelper.GetEmailAddress(collection["ToAddress"].ToString());
            var preHTML = $"<pre style='font-family:Arial,Helvetica;'>{collection["HTMLBody"].ToString()}</pre>";
            emailService.Send(collection["FromName"].ToString(), collection["FromAddress"].ToString(), toAddress, collection["CcAddress"].ToString(), 
                                    collection["BccAddress"].ToString(), collection["Subject"].ToString(), preHTML, id, emailGUID, contactEmailFiles);

            var sentEmailVM = new SentEmailViewModel()
            {
                EmailID = emailGUID,
                CreatedById = _userID,
                CreatedDate = DateTime.Now, 
                MessageDate = DateTime.Now,
                TextBody = collection["HTMLBody"].ToString(),
                HTMLBody = collection["HTMLBody"].ToString(),
                Subject = collection["Subject"].ToString(),
                FromName = collection["FromName"].ToString(),
                FromAddress = collection["FromAddress"].ToString(),
                ToAddress = toAddress,
                CcAddress = collection["CcAddress"].ToString(),
                BccAddress = collection["BccAddress"].ToString(),
                RelatedToId = collection["ToAddress"].ToString()
            };
            RecordSentEmail(sentEmailVM);

            return RedirectToAction("Details", "Contacts", new { id = id });
        }

        //public IActionResult WordRTFMailMerge()
        //{
        //    WordRTFMailMergeModel model = new WordRTFMailMergeModel();
        //    return View(model);
        //}

        //public IActionResult Test()
        //{
        //    _mailMergeHelper.MailMergeForASpecificTemplateOpenXML("asdf");

        //    //Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
        //    ////string clearancename = Form1.Texts;

        //    //string webRootPath = _webHostEnvironment.WebRootPath;
        //    //var fileLocation = $"Templates/clients/Form-Opp - Term Sheet - Standard.doc";
        //    //var uploadedFiles = HttpContext.Request.Form.Files;



        //    //Microsoft.Office.Interop.Word.Document doc = app.Documents.Open($"{fileLocation}");
        //    ////Word.Words wds = doc.Sections[1].Range.Words;
        //    //doc.Activate();

        //    return Content("asdf");
        //}








        public void RecordSentEmail(SentEmailViewModel sentEmailVM)
        {
            if (sentEmailVM != null)
            {
                var newEmailAudit = new EmailMessage
                {
                    Id = sentEmailVM.EmailID,
                    CreatedById = sentEmailVM.CreatedById,
                    CreatedDate = DateTime.Now,
                    MessageDate = DateTime.Now,
                    TextBody = sentEmailVM.TextBody,
                    HtmlBody = sentEmailVM.HTMLBody,
                    Subject = sentEmailVM.Subject,
                    FromName = sentEmailVM.FromName,
                    FromAddress = sentEmailVM.FromAddress,
                    ToAddress = sentEmailVM.ToAddress,
                    CcAddress = sentEmailVM.CcAddress,
                    BccAddress = sentEmailVM.BccAddress,
                    RelatedTo = sentEmailVM.RelatedToId
                };
                _context.EmailMessages.Add(newEmailAudit);
                _context.SaveChanges();
            }
        }

        public List<string> UploadContactEmailAttachments(string id, string emailGUID, IFormCollection collection)
        {
            var fileUploadModel = new FileUploadViewModel();

            var contactID = id;
            var fileUser = User.Identity.Name;
            var userFullName = _userHelper.GetNameFromUserName(User.Identity.Name);

            _uploadHelper.CreatedocumentDirectory(emailGUID, $"Emails/Contacts/{contactID}");
            string webRootPath = _webHostEnvironment.WebRootPath;
            var fileLocation = $"Emails/Contacts/{contactID}/{emailGUID}";
            var uploadedFiles = HttpContext.Request.Form.Files;

            var user = _context.Users.SingleOrDefault(u => u.UserName == userFullName);

            List<string> fileUploads = new List<string>();
            try
            {
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

                return fileUploads;
            }
            catch
            {

            }
            return fileUploads;
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
                    Description = $"{NextNoteDescription}<br>Completed On: {NextNoteActivityDate.Replace("T"," ")}",
                    CreatedById = GetUserIDFromUserName(User.Identity.Name),
                    LastModifiedById = GetUserIDFromUserName(User.Identity.Name),
                    CompletedDateTime = Convert.ToDateTime(NextNoteActivityDate),
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


        //[HttpPost]
        //public JsonResult MailMergeWordDoc(string ContactID, string TemplateType, string OwnerID)
        //{
        //    var mailMergeHelper = new MailMergeHelper(_context, _webHostEnvironment, OwnerID, "Contacts", TemplateType, ContactID);
        //    //mailMergeHelper.WordDocumentMailMerge();

        //    return Json(new
        //    {
        //        resultMessage = "success"
        //    });
        //}


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


        [HttpPost]
        public JsonResult GetContactChartInfo()
        {
            List<ContactIndexViewModel> contacts = new List<ContactIndexViewModel>();
            try
            {
                var currentContacts = _context.Contacts.OrderByDescending(c => c.CreatedDate).ToList();
                if (currentContacts.Count > 0)
                {
                    foreach (var currentContact in currentContacts)
                    {
                        contacts.Add(new ContactIndexViewModel
                        {
                            ID = currentContact.Id,
                            OwnerName = _userHelper.GetNameFromID(currentContact.OwnerId),
                            Grade = currentContact.Rating_Sort,
                            ContactName = GetContactName(currentContact.Id),
                            Company = GetCompanyName(currentContact.AccountId),
                            Phone = currentContact.Phone,
                            Email = currentContact.Email,
                        });
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return Json(contacts);
        }

        public JsonResult GetContactListByCompany(string id)
        {
            List<SelectListItem> contacts = new List<SelectListItem>();
            contacts = _lovHelper.GetContactListByCompany(id).ToList();


            return Json(new SelectList(contacts, "Value", "Text"));
        }

        public JsonResult MailMerge(string ContactID, string TemplateName, string OwnerID)
        {

            // Create VM and pass to mail merge function
            var contactMailMergeVM = new ContactMailMergeViewModel();
            contactMailMergeVM.contact = _context.Contacts.Where(c => c.Id == ContactID).FirstOrDefault();
            if (contactMailMergeVM.contact != null)
            {
                contactMailMergeVM.company = _context.Companies.Where(c => c.ID == contactMailMergeVM.contact.AccountId).FirstOrDefault();
            }
            contactMailMergeVM.owner = _context.Users.Where(c => c.Id == OwnerID).FirstOrDefault();
            contactMailMergeVM.TemplateFileName = TemplateName;

            _mailMergeHelper.MailMergeForASpecificTemplateOpenXML(contactMailMergeVM);

            return Json(new
            {
                resultMessage = "success"
            });
        }


        public JsonResult GetContactListByCompanyName(string name)
        {
            string companyID = _companyHelper.GetCompanyIDFromName(name);
            List<SelectListItem> contacts = new List<SelectListItem>();
            contacts = _lovHelper.GetContactListByCompany(companyID).ToList();


            return Json(new SelectList(contacts, "Value", "Text"));
        }


        public JsonResult GetHTMLTemplate(string templateName)
        {
            var emailTemplate = _context.TestCompany.Where(t => t.ID.ToString() == templateName).FirstOrDefault();
            if (emailTemplate != null)
            {
                return Json(emailTemplate.Name);
            }
            return Json("No Template Available");
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

        // GET Mail Merged FILES
        internal string GetMailMergedFiles(string id)
        {
            var returnString = "";

            string webRootPath = _webHostEnvironment.WebRootPath;
            var fileLocation = $"MergedDocuments/Contacts/{id}";
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
                    var encodedFileName = Path.GetFileName(f.ToString()).Replace("'", HttpUtility.UrlEncode("'")); 
                    encodedFileName = Path.GetFileName(f.ToString()).Replace("%", HttpUtility.UrlEncode("%"));
                    returnString += $@"<tr><td style='width:5%;padding:3px;' class='leftTextAlign'>
                                            <i class='fa fa-{fileTypeFontAwesome}' data-toggle='tooltip' title='{1}' style='color:#{colorFontAwesome};font-size:14px;padding:3px;'></i></td>
                                                <td class='leftTextAlign' style='width:60%;padding:3px;'><a href='../../MergedDocuments/Contacts/{id}/{encodedFileName}' target='_blank'><span style='font-size:12px;'>{Path.GetFileName(f.ToString())}</span>
                                                </a></td>
                                                <td style='width:35%;padding:3px;'>Uploaded: {fileInfo.LastWriteTime.ToString()}</td>
                                                </tr>";

                    //< td class='leftTextAlign' style='width:60%;padding:3px;'><a href = '../../MergedDocuments/Contacts/{id}/{Path.GetFileName(f.ToString()).Replace("'", HttpUtility.UrlEncode("'"))}' target='_blank'><span style = 'font-size:12px;' >{Path.GetFileName(f.ToString())}</span>
                    
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



        // GET UPLOADED FILES
        internal List<string> GetEmailContactFiles(string contactID, string emailID)
        {
            var returnFiles = new List<string>();

            string webRootPath = _webHostEnvironment.WebRootPath;
            var fileLocation = $"Emails/Contacts/{contactID}/{emailID}";
            var files = new DirectoryInfo(Path.Combine(webRootPath, fileLocation)).GetFiles().OrderByDescending(f => f.CreationTime);

            try
            {
                foreach (var f in files)
                {
                    var scrubbedF = Path.GetFileName(f.ToString()).Replace("'", HttpUtility.UrlEncode("'"));
                    returnFiles.Add(scrubbedF);
                }
            }
            catch
            {
                
            }
            return returnFiles;


            //try
            //{
            //    returnString += "<div class='container'><div class='row'><table class='table table-condensed table-hover table-striped' style='width:100%;padding:0px;'>";


            //    foreach (var f in files)
            //    {
            //        var fileInfo = new FileInfo(f.ToString());
            //        string scrubbedF = f.ToString().Replace("'", "\\'");

            //        if (fileInfo.Extension.ToUpper().Contains("PDF"))
            //        {
            //            fileTypeFontAwesome = "file-pdf";
            //            colorFontAwesome = "ff0000";
            //        }

            //        if (fileInfo.Extension.ToUpper().Contains("DOC"))
            //        {
            //            fileTypeFontAwesome = "file-word";
            //            colorFontAwesome = "0078d7 ";
            //        }

            //        if (fileInfo.Extension.ToUpper().Contains("XLS") || fileInfo.Extension.ToUpper().Contains("CSV"))
            //        {
            //            fileTypeFontAwesome = "file-excel";
            //            colorFontAwesome = "1D6F42";
            //        }

            //        if (fileInfo.Extension.ToUpper().Contains("PNG") || fileInfo.Extension.ToUpper().Contains("JPG") || fileInfo.Extension.ToUpper().Contains("JPEG")
            //                    || fileInfo.Extension.ToUpper().Contains("GIF") || fileInfo.Extension.ToUpper().Contains("TIFF"))
            //        {
            //            fileTypeFontAwesome = "file-image";
            //            colorFontAwesome = "999";
            //        }

            //        //returnString += "<a href='../../Images/DecisionLogicReports/" + Path.GetFileName(f) + "' target='_blank'><span class='glyphicon glyphicon-file' data-toggle='tooltip' title='" + Path.GetFileName(f) + "' style='color:#80a7d8;'></span></a>";
            //        //returnString += String.Format("<a href='../../Uploads/CBR/{0}/{1}' target='_blank'><span class='glyphicon glyphicon-file' data-toggle='tooltip' title='{1}' style='color:#80a7d8;'></span></a><br>", id, Path.GetFileName(f));

            //        returnString += $@"<tr><td style='width:5%;padding:3px;' class='leftTextAlign'>
            //                                <i class='fa fa-{fileTypeFontAwesome}' data-toggle='tooltip' title='{1}' style='color:#{colorFontAwesome};font-size:14px;padding:3px;'></i></td>
            //                                    <td class='leftTextAlign' style='width:60%;padding:3px;'><a href='../../Documents/Contacts/{id}/{Path.GetFileName(f.ToString()).Replace("'", HttpUtility.UrlEncode("'"))}' target='_blank'><span style='font-size:12px;'>{Path.GetFileName(f.ToString())}</span>
            //                                    </a></td>
            //                                    <td style='width:35%;padding:3px;'>Uploaded: {fileInfo.LastWriteTime.ToString()}</td>
            //                                    </tr>";

            //        //fileInfo.LastWriteTime.ToString() + "</tr></table></div></div>", id, Path.GetFileName(f), "<b>" + Path.GetFileName(f) + "</b><br>Uploaded: " +
            //        //fileInfo.LastWriteTime.ToString() + "<br>Last Opened: " + fileInfo.LastAccessTime.ToString(), fileTypeFontAwesome, colorFontAwesome);
            //    }
            //    returnString += "</table></div></div>";
            //}
            //catch
            //{
            //    return "<div class='well well-sm' style='margin:2px;'>No Files Yet</div>";
            //}
            //return returnString;

        }


    }
}
