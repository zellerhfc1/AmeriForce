using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AmeriForce.Data;
using AmeriForce.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using AmeriForce.Models.Email;
using Microsoft.Extensions.Configuration;
using AmeriForce.Models.Clients;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Web;
using AmeriForce.Models;

namespace AmeriForce.Controllers
{
    [Authorize]
    public class ClientsController : Controller
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

        private List<Company> _fullCompanyList;
        private List<SelectListItem> _companyList = new List<SelectListItem>();
        //private List<SelectListItem> _sicCodeList = new List<SelectListItem>();


        public ClientsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            configuration.GetSection("EmailConfiguration").Bind(_emailConfig);

            _lovHelper = new LOVHelper(_context);
            _companyHelper = new CompanyHelper(_context);
            _userHelper = new UserHelper(_context);
            _uploadHelper = new UploadHelper(_context, _webHostEnvironment);

            _fullCompanyList = _context.Companies.ToList();
            _companyList = _lovHelper.GetCompanyDropdownList().ToList();
            //_sicCodeList = _lovHelper.GetSICCodesList().ToList();

        }

        // GET: Clients
        public async Task<IActionResult> Index(int? id)
        {
            //return View(await _context.Clients.ToListAsync());

            switch (id)
            {
                case 1:
                    ViewBag.SubTitle = "My Clients In Pipeline";
                    ViewBag.TimeStamp = DateTime.Now.ToString("MMMM dd, yyyy HH:mm:ss");
                    string userID = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault().Id;
                    var clientList = _context.Clients.Where(c => c.StageName != "90-Lost"
                            && c.StageName != "80-Hold"
                            && c.StageName != "95-Declined by ASF"
                            && c.StageName != "50-Funded"
                            && c.OwnerId == userID).Take(200).OrderByDescending(c => c.StageName);
                    ViewBag.ClientCount = clientList.Count();
                    return View(clientList);
                case 2:
                    ViewBag.SubTitle = "My Clients (All)";
                    ViewBag.TimeStamp = DateTime.Now.ToString("MMMM dd, yyyy HH:mm:ss");
                    string userIDAll = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault().Id;
                    var clientListAll = _context.Clients.Where(c => c.OwnerId == userIDAll).Take(1000).OrderBy(c => c.StageName);
                    ViewBag.ClientCount = clientListAll.Count();
                    return View(clientListAll);
                case 3:
                    ViewBag.SubTitle = "All New Clients";
                    ViewBag.TimeStamp = DateTime.Now.ToString("MMMM dd, yyyy HH:mm:ss");
                    //string userIDNewClients = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault().Id;
                    var clientListNewClients = _context.Clients.Where(c => c.StageName == "00-Initial Review").Take(1000).OrderBy(c => c.StageName);
                    ViewBag.ClientCount = clientListNewClients.Count();
                    return View(clientListNewClients);
                //case "Lost":
                //    break;
                default:
                    ViewBag.SubTitle = "All Clients In Pipeline";
                    ViewBag.TimeStamp = DateTime.Now.ToString("MMMM dd, yyyy HH:mm:ss");
                    var clientListDefault = _context.Clients.Where(c => c.StageName != "90-Lost"
                            && c.StageName != "80-Hold"
                            && c.StageName != "95-Declined by ASF"
                            && c.StageName != "50-Funded"
                            && c.StageName != "00-Initial Review").Take(2000).OrderByDescending(c => c.CreatedDate);
                    ViewBag.ClientCount = clientListDefault.Count();
                    return View(clientListDefault);
            }
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                string clientFiles = GetClientFiles(id);
                string mailMergeFiles = GetMailMergeFiles(id);
                ViewBag.ClientFiles = clientFiles;
                ViewBag.MailMergeFiles = mailMergeFiles;

                var clientModel = new ClientDetailsViewModel();
                clientModel.clientData = _context.Clients.Find(id);
                clientModel.companyData = _context.Companies.Where(c => c.ID == clientModel.clientData.CompanyId).FirstOrDefault();
                clientModel.mainContactData = _context.Contacts.Where(c => c.AccountId == clientModel.clientData.CompanyId).ToList();
                clientModel.referringContactData = _context.Contacts.Where(c => c.Id == clientModel.clientData.Referring_Contact).FirstOrDefault();
                clientModel.referringCompanyData = _context.Companies.Where(c => c.ID == clientModel.clientData.Referring_Company).FirstOrDefault();
                clientModel.clientFacilities = _context.Facilities.Where(f => f.ClientID == id);
                clientModel.clientNextTask = _context.CRMTasks.Where(c => c.Id == clientModel.clientData.NextActivityID).FirstOrDefault();
                clientModel.clientNotes = _context.CRMTasks.Where(c => c.WhatId == clientModel.clientData.Id && c.Id != clientModel.clientData.NextActivityID).OrderByDescending(c => c.CreatedDate);

                clientModel.TaskList = _lovHelper.GetTaskTypes();
                clientModel.ActiveUserList = _lovHelper.GetActiveUsers();

                clientModel.TaskListNotes = _lovHelper.GetTaskTypes();
                clientModel.ActiveUserListNotes = _lovHelper.GetActiveUsers();
                clientModel.YesNoList = _lovHelper.GetYesNoList();
                clientModel.MailMergeTemplateList = _lovHelper.GetMailMergeTemplateList();

                clientModel.OwnerName = _userHelper.GetNameFromID(clientModel.clientData.OwnerId);


                if (clientModel.referringContactData == null)
                {
                    clientModel.referringContactData = _context.Contacts.Where(c => c.Id == "003xxxxxxxxxxxxxxx").FirstOrDefault();
                }

                if (clientModel.referringCompanyData == null)
                {
                    clientModel.referringCompanyData = _context.Companies.Where(c => c.ID == "001xxxxxxxxxxxxxxx").FirstOrDefault();
                }

                if (clientModel == null)
                {
                    return NotFound();
                }
                return View(clientModel);
            }

            return View();
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            var clientCreateModel = new ClientCreateViewModel();

            clientCreateModel.BaseList = _lovHelper.GetBaseList();
            clientCreateModel.CompanyList = _lovHelper.GetCompanyDropdownList();
            clientCreateModel.OwnerList = _lovHelper.GetOwnerDropdown();
            clientCreateModel.ClientStageList = _lovHelper.GetClientStages();
            clientCreateModel.BDOList = _lovHelper.GetBDOList();
            clientCreateModel.StateList = _lovHelper.GetStateList();
            clientCreateModel.TaskList = _lovHelper.GetTaskTypes();
            clientCreateModel.ActiveUserList = _lovHelper.GetActiveUsers();
            clientCreateModel.ReferralTypeList = _lovHelper.GetReferralTypes();

            return View(clientCreateModel);
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientCreateViewModel client, IFormCollection collection)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(client);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(client);

            var validationHelper = new ValidationHelper();
            var clientAmount = validationHelper.ValidateInteger(collection["clientData.Amount"].ToString());
            client.clientData.Amount = clientAmount;
            TryValidateModel(client);

            //if (ModelState.IsValid)
            //{
                var transaction = _context.Database.BeginTransaction();
                try
                {
                    // CLIENT DATA
                    var clientOwnerId = validationHelper.IsUser(collection["clientData.OwnerId"].ToString());
                    var clientCloseDate = validationHelper.ValidateDateWithString_NotRequired(collection["clientData_CloseDate"].ToString());
                    var clientLeadSource = collection["clientData.LeadSource"].ToString();
                    var clientReferralDate = validationHelper.ValidateDateWithString_NotRequired(collection["clientData_Referral_Date"].ToString());
                    var clientReferringContact = collection["clientData.Referring_Contact"].ToString();
                    var clientReferringCompany = collection["clientdata.Referring_Company"].ToString();
                    var clientReferringCompanyName = collection["ReferringCompanyName"].ToString();
                    var clientCompanyID = collection["CompanyID"].ToString();
                    var clientStatus = "00-Initial Review";

                    // COMPANY DATA
                    var companyName = validationHelper.ValidateRequiredString(collection["CompanyName"].ToString());
                    var companyDescription = collection["companyData_Description"].ToString();
                    var companySICCode = collection["companyData_SICCode"].ToString();
                    var companyCharterState = collection["companyData.CharterState"].ToString();
                    var companyExistingOrNew = collection["CompanyNameId"].ToString();


                    if (String.IsNullOrEmpty(companyExistingOrNew))
                    {
                        // ADD ABOVE AS NEW COMPANY
                        var newCompany = new Company()
                        {
                            ID = _guidHelper.GetGUIDString("company"),
                            Name = companyName,
                            Description = companyDescription,
                            SICCode = companySICCode,
                            CharterState = companyCharterState,
                            //CreatedById = userHelper.GetNameFromID(User.Identity.Name),
                            CreatedDate = DateTime.Now,
                            //LastModifiedById = userHelper.GetNameFromID(User.Identity.Name),
                            LastModifiedDate = DateTime.Now
                        };
                        _context.Companies.Add(newCompany);
                        _context.SaveChanges();
                        companyExistingOrNew = newCompany.ID;
                    }


                    // CONTACT DATA
                    var contactFirstName = collection["contactData_FirstName"].ToString();
                    var contactLastName = validationHelper.ValidateRequiredString(collection["contactData_LastName"].ToString());
                    var contactTitle = collection["contactData_Title"].ToString();
                    var contactEmail = validationHelper.ValidateEmail(collection["contactData_Email"].ToString());
                    var contactDirectLine = validationHelper.ValidatePhone(collection["contactData_Direct_Line"].ToString());
                    var contactIntialMeetingDetails = collection["contactData_Initial_Meeting_Details"].ToString();


                    // ADD ABOVE TO NEW CONTACT AND TIE TO COMPANY
                    var newContact = new Contact()
                    {
                        Id = _guidHelper.GetGUIDString("contact"),
                        AccountId = companyExistingOrNew,
                        FirstName = contactFirstName,
                        LastName = contactLastName,
                        Title = contactTitle,
                        Email = contactEmail,
                        Direct_Line = contactDirectLine,
                        Initial_Meeting_Details = contactIntialMeetingDetails,
                        CreatedById = _userHelper.GetIDFromName(User.Identity.Name),
                        CreatedDate = DateTime.Now,
                        LastModifiedById = _userHelper.GetIDFromName(User.Identity.Name),
                        LastModifiedDate = DateTime.Now,
                        Relationship_Status = "New"
                    };
                    _context.Contacts.Add(newContact);
                    _context.SaveChanges();


                    // TASK
                    var taskType = validationHelper.ValidateRequiredString(collection["taskData.Type"].ToString());
                    var taskOwnerID = validationHelper.ValidateRequiredString(collection["taskData.OwnerId"].ToString());
                    var taskActivityDate = collection["taskData_ActivityDate"].ToString();
                    var taskDescription = validationHelper.ValidateRequiredString(collection["taskData_Description"].ToString());

                    var newTask = new CRMTask()
                    {
                        Id = _guidHelper.GetGUIDString("task"),
                        Type = taskType,
                        OwnerId = taskOwnerID,
                        ActivityDate = Convert.ToDateTime(taskActivityDate),
                        Description = taskDescription,
                        CreatedById = _userHelper.GetIDFromName(User.Identity.Name),
                        CreatedDate = DateTime.Now,
                        LastModifiedById = _userHelper.GetIDFromName(User.Identity.Name),
                        LastModifiedDate = DateTime.Now
                    };
                    _context.CRMTasks.Add(newTask);
                    _context.SaveChanges();


                    // CLIENT
                    var newClient = new Client()
                    {
                        Id = _guidHelper.GetGUIDString("client"),
                        StageName = clientStatus,
                        OwnerId = clientOwnerId,
                        Amount = clientAmount,
                        CloseDate = clientCloseDate,
                        LeadSource = clientLeadSource,
                        Referral_Date = clientReferralDate,
                        Referring_Contact = clientReferringContact,
                        Referring_Company = clientReferringCompany,
                        CompanyId = companyExistingOrNew,
                        ContactId = newContact.Id,
                        NextActivityID = newTask.Id,
                        CreatedById = _userHelper.GetIDFromName(User.Identity.Name),
                        CreatedDate = DateTime.Now,
                        LastModifiedById = _userHelper.GetIDFromName(User.Identity.Name),
                        LastModifiedDate = DateTime.Now
                        //clientReferringCompanyName, 
                        //clientCompanyID,   
                    };
                    _context.Clients.Add(newClient);
                    _context.SaveChanges();

                    transaction.Commit();
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Content(ex.Data + "<br>" + ex.InnerException);
                }

            //}

            //return View(client);


        }


        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {

                string clientFiles = GetClientFiles(id);
                ViewBag.ClientFiles = clientFiles;
                ViewBag.MergeFiles = clientFiles;

                var clientModel = new ClientEditViewModel();
                clientModel.clientData = _context.Clients.Find(id);
                clientModel.companyData = _context.Companies.Where(c => c.ID == clientModel.clientData.CompanyId).FirstOrDefault();
                clientModel.mainContactData = _context.Contacts.Where(c => c.AccountId == clientModel.clientData.CompanyId).ToList();
                clientModel.referringContactData = _context.Contacts.Where(c => c.Id == clientModel.clientData.Referring_Contact).FirstOrDefault();
                clientModel.referringCompanyData = _context.Companies.Where(c => c.ID == clientModel.clientData.Referring_Company).FirstOrDefault();
                clientModel.clientFacilities = _context.Facilities.Where(f => f.ClientID == id);
                clientModel.clientNextTask = _context.CRMTasks.Where(c => c.Id == clientModel.clientData.NextActivityID).FirstOrDefault();
                clientModel.clientNotes = _context.CRMTasks.Where(c => c.WhatId == clientModel.clientData.Id && c.Id != clientModel.clientData.NextActivityID);

                clientModel.TaskList = _lovHelper.GetTaskTypes();
                clientModel.ActiveUserList = _lovHelper.GetActiveUsers();
                clientModel.BDOList = _lovHelper.GetBDOList();
                clientModel.TaskListNotes = _lovHelper.GetTaskTypes();
                clientModel.ActiveUserListNotes = _lovHelper.GetActiveUsers();
                clientModel.YesNoList = _lovHelper.GetYesNoList();
                clientModel.ClientStageList = _lovHelper.GetClientStages();
                clientModel.StatesList = _lovHelper.GetStateList();
                clientModel.ReferralTypes = _lovHelper.GetReferralTypes();
                clientModel.ApprovalTypes = _lovHelper.GetApprovalDecisionList();

                if (clientModel.referringContactData == null)
                {
                    clientModel.referringContactData = _context.Contacts.Where(c => c.Id == "003xxxxxxxxxxxxxxx").FirstOrDefault();
                }

                if (clientModel.referringCompanyData == null)
                {
                    clientModel.referringCompanyData = _context.Companies.Where(c => c.ID == "001xxxxxxxxxxxxxxx").FirstOrDefault();
                }

                if (clientModel == null)
                {
                    return NotFound();
                }

                return View(clientModel);
            }

            return View();
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       
        //public async Task<IActionResult> Edit(string id, [Bind("Id,CompanyId,Name,Description,StageName,StageSortOrder,Amount,ExpectedRevenue,CloseDate,Type,LeadSource,IsClosed,ForecastCategory,ForecastCategoryName,OwnerId,CreatedDate,CreatedById,LastModifiedDate,LastModifiedById,SystemModstamp,LastActivityDate,LastStageChangeDate,FiscalYear,FiscalQuarter,ContactId,LastAmountChangedHistoryId,LastCloseDateChangedHistoryId,Legal_Docs_Issued,Legal_Docs_Received,Legal_Docs_Time_Out,External_Opportunity_ID,AD_Credit_Review,Delayed_Opportunity,All_Post_Funding_Tasks_Completed,Approval_Decision,Approval_Decision_Time_1,Bankruptcy_Search,BDO_Write_Up_Received,Client_Profile_Out,Client_Profile_In,Corporate_Verification,Criminal_Background_Search,D_B_Prospect,Due_Diligence_Deposit_Received,File_Received_by_UW,File_to_Client_Services,File_to_Mgmt_for_Review,First_Response,FTL_Search_State_County,Initial_Funding,Ins_Verification_Transp_Staff_only,Invoices_Rcd_Ready_to_Move_Fwd,Judgment_Search,Litigation_Search,Lost_to_Competitor,Mgmt_Completes_Post_Funding_File_Review,Pre_Approval_Biz_Hours,Reason_Lost,Referral_Date,Referring_Company,Referring_Contact,Searches_Requested,Searches_Returned,STL_Search,Tax_Guard_Report,Term_Sheet_Issued,Term_Sheet_Received,UCC_Search,UW_Hands_over_for_First_Review,NextActivityID")] Client client)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ClientEditViewModel clientEditVM, IFormCollection collection)
        {

            string clientId = collection["ClientID"];
            string companyId = collection["ClientCompanyID"];

            if (id != clientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    #region Validation
                    var validationHelper = new ValidationHelper();

                    var company = _context.Companies.Where(c => c.ID == companyId).FirstOrDefault();
                    company.SICCode = collection["companyData_SICCode"].ToString();
                    company.CharterState = collection["companyData.CharterState"].ToString();
                    company.Name = collection["companyData_Name"].ToString();
                    company.Description = collection["companyData_Description"].ToString();


                    var client = _context.Clients.Where(c => c.Id == clientId).FirstOrDefault();
                    client.Name = collection["clientData_Name"].ToString();
                    client.StageName = collection["clientData.StageName"].ToString();
                    client.Amount = Convert.ToInt32(collection["clientData.Amount"].ToString());
                    client.CloseDate = validationHelper.ValidateDateWithString_NotRequired(collection["clientData_CloseDate"].ToString());
                    client.OwnerId = collection["clientData.OwnerId"].ToString();

                    client.LeadSource = collection["clientData.LeadSource"].ToString();
                    client.Referral_Date = validationHelper.ValidateDateWithString_NotRequired(collection["clientData_Referral_Date"].ToString());
                    client.Referring_Company = collection["clientData.Referring_Company"].ToString();
                    client.Referring_Contact = collection["clientData.Referring_Contact"].ToString();

                    client.Term_Sheet_Issued = validationHelper.ValidateDateWithString_NotRequired(collection["clientData.Term_Sheet_Issued"].ToString());
                    client.Term_Sheet_Received = validationHelper.ValidateDateWithString_NotRequired(collection["clientData.Term_Sheet_Received"].ToString());
                    client.Due_Diligence_Deposit_Received = validationHelper.ValidateDateWithString_NotRequired(collection["clientData.Due_Diligence_Deposit_Received"].ToString());
                    client.UCC_Search = validationHelper.ValidateDateWithString_NotRequired(collection["clientData.UCC_Search"].ToString());
                    client.Tax_Guard_Report = validationHelper.ValidateDateWithString_NotRequired(collection["clientData.Tax_Guard_Report"].ToString());
                    client.File_Received_by_UW = validationHelper.ValidateDateWithString_NotRequired(collection["clientData.File_Received_by_UW"].ToString());
                    client.FTL_Search_State_County = validationHelper.ValidateDateWithString_NotRequired(collection["clientData.FTL_Search_State_County"].ToString());
                    client.Ins_Verification_Transp_Staff_only = validationHelper.ValidateDateWithString_NotRequired(collection["clientData.Ins_Verification_Transp_Staff_only"].ToString());

                    client.UW_Hands_over_for_First_Review = validationHelper.ValidateDateWithString_NotRequired(collection["clientData.UW_Hands_over_for_First_Review"].ToString());
                    client.Approval_Decision = collection["clientData.Approval_Decision"].ToString();

                    client.Legal_Docs_Issued = validationHelper.ValidateDateWithString_NotRequired(collection["clientData.Legal_Docs_Issued"].ToString());
                    client.Legal_Docs_Received = validationHelper.ValidateDateWithString_NotRequired(collection["clientData.Legal_Docs_Received"].ToString());

                    client.Initial_Funding = validationHelper.ValidateDateWithString_NotRequired(collection["clientData.Initial_Funding"].ToString());
                    client.File_to_Mgmt_for_Review = validationHelper.ValidateDateWithString_NotRequired(collection["clientData.File_to_Mgmt_for_Review"].ToString());
                    client.All_Post_Funding_Tasks_Completed = validationHelper.ValidateDateWithString_NotRequired(collection["clientData.All_Post_Funding_Tasks_Completed"].ToString());
                    client.File_to_Client_Services = validationHelper.ValidateDateWithString_NotRequired(collection["clientData.File_to_Client_Services"].ToString());
                    client.Mgmt_Completes_Post_Funding_File_Review = validationHelper.ValidateDateWithString_NotRequired(collection["clientData.Mgmt_Completes_Post_Funding_File_Review"].ToString());

                    #endregion

                    _context.SaveChanges();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(clientEditVM.clientData.Id))
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
            return View(clientEditVM);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var client = await _context.Clients.FindAsync(id);
            _context.Clients.Remove(client);
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
            var contactID = Convert.ToString(collection["uploadClientID"]);
            var fileUser = User.Identity.Name;
            var userFullName = User.Identity.Name;

            _uploadHelper.CreatedocumentDirectory(contactID, "Clients");
            string webRootPath = _webHostEnvironment.WebRootPath;
            var fileLocation = $"Documents/Clients/{contactID}";
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

                return RedirectToAction("Details", "Clients", new { id = contactID, uploadStatus = "SUCCESS" });
            }
            catch
            {
                ViewBag.FileUploadSuccess = "Fail";
                return View();
            }
        }



        #region C# Functions
        private bool ClientExists(string id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }



        // GET UPLOADED CLIENT FILES
        internal string GetClientFiles(string id)
        {
            var returnString = "";

            string webRootPath = _webHostEnvironment.WebRootPath;
            var fileLocation = $"Documents/Clients/{id}";
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


        // GET UPLOADED MAIL MERGE FILES
        internal string GetMailMergeFiles(string id)
        {
            var returnString = "";

            string webRootPath = _webHostEnvironment.WebRootPath;
            var fileLocation = $"TemplateResults/Clients/{id}";
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

        #endregion


        #region JSON Requests
        public JsonResult GetContactListByCompany(string id)
        {
            List<SelectListItem> contacts = new List<SelectListItem>();
            contacts = _lovHelper.GetContactListByCompany(id).ToList();

            return Json(new SelectList(contacts, "Value", "Text"));
        }

        [HttpPost]
        public JsonResult GetCompanyID(string enteredText)
        {
            var companyInfoEmpty = "";
            if (!String.IsNullOrEmpty(enteredText))
            {
                var companyInfo = (from company in _fullCompanyList
                                   where company.Name.ToUpper().StartsWith(enteredText.ToUpper())
                                   select new { company.ID, company.Name, company.Description, company.SICCode, company.CharterState });

                return Json(companyInfo);
            }
            return Json(companyInfoEmpty);
        }


        [HttpPost]
        public JsonResult GetAutocompleteFeature_Companies(string enteredText)
        {

            var companyName = (from company in _companyList
                               where company.Text.ToUpper().StartsWith(enteredText.ToUpper())
                               select new { company.Text });
            return Json(companyName);
        }


        //[HttpPost]
        //public JsonResult GetAutocompleteFeature_SICCodes(string enteredText)
        //{

        //    var companyName = (from siccode in _sicCodeList
        //                       where siccode.Text.ToUpper().StartsWith(enteredText.ToUpper())

        //                       select new { siccode.Text });
        //    return Json(companyName);
        //}



        [HttpPost]
        public JsonResult UpdateFacility(string FacilityName, string FacilityClientID, string FacilityType, string FacilityCategories, string FacilityID)
        {
                int facilityID = (FacilityID != "") ? Convert.ToInt32(FacilityID) : 0;
                var existingEntry = _context.Facilities.Where(f => f.ID == facilityID).FirstOrDefault();

                if (existingEntry != null)
                {
                    existingEntry.ClientID = FacilityClientID;
                    existingEntry.FacilityName = FacilityName;
                    existingEntry.FacilityType = FacilityType;
                    existingEntry.FacilityCategory = FacilityCategories;
                    existingEntry.CreatedBy = User.Identity.Name;
                _context.SaveChanges();
                }
                else
                {
                    var facilityEntry = new Facility()
                    {
                        ClientID = FacilityClientID,
                        FacilityName = FacilityName,
                        FacilityType = FacilityType,
                        FacilityCategory = FacilityCategories,
                        CreatedBy = User.Identity.Name
                    };
                    _context.Facilities.Add(facilityEntry);
                    _context.SaveChanges();
                }

            return Json(new
            {
                facility = FacilityName
            });
        }


        [HttpPost]
        public JsonResult GetFacility(string FacilityID)
        {
                int convertedFacilityID = Convert.ToInt32(FacilityID);
                var facility = _context.Facilities.Where(f => f.ID == convertedFacilityID).FirstOrDefault();

                return Json(new
                {
                    facilityName = facility.FacilityName,
                    facilityType = facility.FacilityType,
                    facilityCategories = facility.FacilityCategory
                });
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
                        existingCall.CreatedById = User.Identity.Name;
                        existingCall.LastModifiedById = User.Identity.Name;
                        existingCall.LastModifiedDate = DateTime.Now;
                        _context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {

                }

            return Json(new
            {
                callID = NextCallOwnerID
            });
        }



        [HttpPost]
        public JsonResult UpdateClientNote(string ClientID,
                                            string NextNoteType,
                                           string NextNoteOwnerID,
                                           //string NextNoteActivityDate,
                                           string NextNoteDescription)
        {

                try
                {
                    var newNote = new CRMTask()
                    {
                        Id = new GuidHelper().GetGUIDString("task"),
                        WhatId = ClientID,
                        Type = NextNoteType,
                        OwnerId = NextNoteOwnerID,
                        ActivityDate = DateTime.Now, //Convert.ToDateTime(NextNoteActivityDate),
                        Description = NextNoteDescription,
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
        public JsonResult GetContact(string ClientContactID)
        {
            try
            {
                var contact = _context.Contacts.Where(c => c.Id == ClientContactID).FirstOrDefault();

                if (contact != null)
                {
                    return Json(new
                    {
                        clientContactTitle = contact.Title,
                        clientContactOwnershipPercentage = contact.OwnershipPercentage,
                        clientContactGuarantor = contact.Guarantor,
                        clientContactPhone = contact.Phone,
                        clientContactMobilePhone = contact.MobilePhone,
                        clientContactEmail = contact.Email
                    });
                }
            }
            catch (Exception ex)
            {

            }

            return Json(new
            {
                errorMessage = "error"
            });
        }




        [HttpPost]
        public JsonResult UpdateContact(string ClientContactID,
                                        string ClientContactTitle,
                                        string ClientContactOwnershipPercentage,
                                        string ClientContactGuarantor,
                                        string ClientContactPhone,
                                        string ClientContactMobilePhone,
                                        string ClientContactEmail)
        {

                try
                {
                    var existingContact = _context.Contacts.Where(c => c.Id == ClientContactID).FirstOrDefault();

                    if (existingContact != null)
                    {
                        existingContact.Title = ClientContactTitle;
                        existingContact.OwnershipPercentage = ClientContactOwnershipPercentage;
                        existingContact.Guarantor = Convert.ToBoolean(ClientContactGuarantor);
                        existingContact.Phone = ClientContactPhone;
                        existingContact.MobilePhone = ClientContactMobilePhone;
                        existingContact.Email = ClientContactEmail;
                        existingContact.LastModifiedById = User.Identity.Name;
                        existingContact.LastModifiedDate = DateTime.Now;

                        _context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {

                }

            return Json(new
            {
                contactID = ClientContactID
            });
        }



        //[HttpPost]
        //public JsonResult MailMerge(string ClientID, string TemplateType, string TemplateRecipient)
        //{
        //    var mailMergeHelper = new MailMergeHelper(ClientID, "Clients", TemplateType, TemplateRecipient);
        //    mailMergeHelper.WordDocumentMailMerge();

        //    return Json(new
        //    {
        //        resultMessage = "success"
        //    });

        //}




        //[HttpPost]
        //public JsonResult UploadEmailFiles(HttpPostedFileBase[] files)
        //{
        //    //using (var db = new ApplicationDbContext())
        //    //{
        //    //    //int convertedFacilityID = Convert.ToInt32(FacilityID);
        //    //    //var facility = db.Facilities.Where(f => f.ID == convertedFacilityID).FirstOrDefault();

        //    //    return Json(new
        //    //    {
        //    //        bob = "bob"
        //    //        //facilityName = facility.FacilityName,
        //    //        //facilityType = facility.FacilityType,
        //    //        //facilityCategories = facility.FacilityCategory
        //    //    },
        //    //        JsonRequestBehavior.AllowGet);
        //    //}

        //    try
        //    {
        //        foreach (HttpPostedFileBase file in files)
        //        {
        //            if (file != null)
        //            {
        //                var InputFileName = Path.GetFileName(file.FileName);
        //                //var ServerSavePath = Path.Combine(Server.MapPath("~/UploadedFiles/") + InputFileName);
        //                var _FileName = $"{Path.GetFileName(file.FileName)}";


        //                string _path = Path.Combine(Server.MapPath("~/TempFiles"), _FileName);
        //                file.SaveAs(_path);
        //                //assigning file uploaded status to ViewBag for showing message to user.  
        //                ViewBag.UploadStatus = files.Count().ToString() + " files uploaded successfully.";
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return Json(new
        //    {
        //        resultMessage = "success"
        //    });

        //}


        #endregion

    }
}
