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

        public ContactsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

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
                foreach(var contact in contacts)
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
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccountId,Salutation,FirstName,LastName,MiddleName,OtherStreet,OtherCity,OtherState,OtherPostalCode,OtherCountry,MailingStreet,MailingCity,MailingState,MailingPostalCode,MailingCountry,Phone,Fax,MobilePhone,HomePhone,Email,Title,Department,AssistantName,Birthdate,Description,OwnerId,HasOptedOutOfEmail,CreatedDate,CreatedById,LastModifiedDate,LastModifiedById,LastActivityDate,EmailBouncedReason,EmailBouncedDate,Alt_Email,Alt_Contact,Children,Direct_Line,Extension,Initial_Meeting_Details,LinkedIn_Profile,Mailing_Lists,Opt_Out,Opt_Out_Date,Preferred_Name,Reassigned_Date,Referral_Date,Referral_Partner_Agmnt_Date,Referral_Partner_Agmnt_Details,Referring_Company,Referring_Contact,Relationship_Status,Standard_Pay_Terms,Rating_Sort,Tax_ID,Term_Of_Agreement,Twitter_Profile,Type,Update_Needed,Update_Needed_Date,Alt_Phone_3,OwnershipPercentage,Guarantor,NextActivityID")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,AccountId,Salutation,FirstName,LastName,MiddleName,OtherStreet,OtherCity,OtherState,OtherPostalCode,OtherCountry,MailingStreet,MailingCity,MailingState,MailingPostalCode,MailingCountry,Phone,Fax,MobilePhone,HomePhone,Email,Title,Department,AssistantName,Birthdate,Description,OwnerId,HasOptedOutOfEmail,CreatedDate,CreatedById,LastModifiedDate,LastModifiedById,LastActivityDate,EmailBouncedReason,EmailBouncedDate,Alt_Email,Alt_Contact,Children,Direct_Line,Extension,Initial_Meeting_Details,LinkedIn_Profile,Mailing_Lists,Opt_Out,Opt_Out_Date,Preferred_Name,Reassigned_Date,Referral_Date,Referral_Partner_Agmnt_Date,Referral_Partner_Agmnt_Details,Referring_Company,Referring_Contact,Relationship_Status,Standard_Pay_Terms,Rating_Sort,Tax_ID,Term_Of_Agreement,Twitter_Profile,Type,Update_Needed,Update_Needed_Date,Alt_Phone_3,OwnershipPercentage,Guarantor,NextActivityID")] Contact contact)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
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


        public async Task<IActionResult> MailMergeWordDoc(string id)
        {
            var x = new MailMergeHelper(_context, id, "Contacts", "Form-Opp - Term Sheet - Standard.doc");
            x.WordDocumentMailMerge();

            return Content(x.ToString());
        }




        public ActionResult MergeContacts(string id)
        {
            var contactsForMerge = GetContactsForMerge(id);
            if (contactsForMerge != null)
            {
                return View(contactsForMerge);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MergeContacts(FormCollection collection)
        {
            string contactString = "";
            string x = "";
            foreach (string key in collection.Keys)
            {
                if (key == "IsChecked")
                {
                    contactString = collection[key];
                    List<string> contactList = new List<string>();
                    contactList = contactString.Split(',').ToList();
                }
            }
            return RedirectToAction("MergeChosenContacts", new { chosenContacts = contactString });
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
                var currentContact = _context.Contacts.Where(c=>c.Id == ContactID).FirstOrDefault();
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
            return new ContactHelper().GetName(id);
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

                    returnString += $@"<tr><td style='width:5%;' class='leftTextAlign'>
                                            <i class='fa fa-{fileTypeFontAwesome}' data-toggle='tooltip' title='{1}' style='color:#{colorFontAwesome};font-size:14px;'></i></td>
                                                <td class='leftTextAlign' style='width:60%;'><a href='../../Documents/Contacts/{id}/{Path.GetFileName(f.ToString()).Replace("'",HttpUtility.UrlEncode("'"))}' target='_blank'><span style='font-size:12px;'>{Path.GetFileName(f.ToString())}</span>
                                                </a></td>
                                                <td style='width:35%;'>Uploaded: {fileInfo.LastWriteTime.ToString()}</td>
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


        private List<Contact> GetContactsForMerge(string companyID)
        {
            List<Contact> activeContacts;
            activeContacts = _context.Contacts.Where(c => c.AccountId == companyID && c.Relationship_Status != "Dead").ToList();

            return activeContacts;
        }


    }
}
