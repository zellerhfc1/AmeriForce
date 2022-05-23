using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AmeriForce.Models.Companies;
using AmeriForce.Data;
using AmeriForce.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace AmeriForce.Controllers
{
    [Authorize]
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private GuidHelper _guidHelper = new GuidHelper();
        private CompanyHelper _companyHelper;
        private UserHelper _userHelper;
        private LOVHelper _lovHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CompaniesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _lovHelper = new LOVHelper(_context);
            _companyHelper = new CompanyHelper(_context);
            _userHelper = new UserHelper(_context);
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Companies
        public async Task<IActionResult> Index(string? id)
        {
            var currentLetterOfAlphabet = (String.IsNullOrEmpty(id)) ? "A" : id.ToUpper();
            return View(await _context.Companies.Where(c=>c.Name.StartsWith(id)).OrderByDescending(c=>c.CreatedDate).ToListAsync());
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var companyDetailsVM = new CompanyDetailsViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.ID == id);
            if (company != null)
            {
                var contacts = await _context.Contacts.Where(c => c.AccountId == company.ID).ToListAsync();
                companyDetailsVM.company = company;
                companyDetailsVM.contacts = contacts;
            }

            if (company == null)
            {
                return NotFound();
            }

            

            return View(companyDetailsVM);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            CompanyCreateViewModel createCompanyVM = new CompanyCreateViewModel();

            createCompanyVM.CompanyTypeList = _lovHelper.GetCompanyTypes();
            createCompanyVM.StateList = _lovHelper.GetStateList();
            createCompanyVM.SICCodesList = _lovHelper.GetSICCodesList();

            return View(createCompanyVM);
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ID,Name,Description,SICCode,CharterState,CreatedDate,LastModifiedDate,MailingAddress,MailingCity,MailingState,MailingPostalCode")] Company company)
        public async Task<IActionResult> Create(CompanyCreateViewModel companyCreateVM)
        {
            if (ModelState.IsValid)
            {
                var convertCompany = CreateCompany_ConvertToDataLayer(companyCreateVM.companyVM);
                if (companyCreateVM.companyVM.SICCodeManual != null)
                {
                    _companyHelper.AddNewSICCodeToList(companyCreateVM.companyVM.SICCodeManual);
                }
                string companyIDGenerated = _guidHelper.GetGUIDString("company");
                convertCompany.ID = companyIDGenerated;

                _context.Add(convertCompany);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(companyCreateVM);
        }

       

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            CompanyEditViewModel editCompanyVM = new CompanyEditViewModel();

            var convertedCompany = EditCompany_ConvertToViewModel(company);
            editCompanyVM.companyVM = convertedCompany;

            editCompanyVM.CompanyTypeList = _lovHelper.GetCompanyTypes();
            editCompanyVM.StateList = _lovHelper.GetStateList();
            editCompanyVM.SICCodesList = _lovHelper.GetSICCodesList();


            return View(editCompanyVM);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string? id, CompanyEditViewModel company)
        {
            if (id != company.companyVM.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingCompany = await _context.Companies.Where(c=>c.ID == id).FirstOrDefaultAsync();
                    if (existingCompany != null)
                    {
                        existingCompany.ID = company.companyVM.ID;
                        existingCompany.Name = company.companyVM.Name;
                        existingCompany.Description = company.companyVM.Description;
                        existingCompany.CompanyType = company.companyVM.CompanyType;
                        existingCompany.SICCode = (company.companyVM.SICCode == null) ? company.companyVM.SICCodeManual : company.companyVM.SICCode;
                        existingCompany.CharterState = company.companyVM.CharterState;
                        existingCompany.MailingAddress = company.companyVM.MailingAddress;
                        existingCompany.MailingCity = company.companyVM.MailingCity;
                        existingCompany.MailingState = company.companyVM.MailingState;
                        existingCompany.MailingPostalCode = company.companyVM.MailingPostalCode;
                        existingCompany.LastModifiedDate = DateTime.Now;
                        existingCompany.LastUpdatedBy = _userHelper.GetIDFromName(User.Identity.Name);
                    }
                    
                    if (company.companyVM.SICCodeManual != null)
                    {
                        _companyHelper.AddNewSICCodeToList(company.companyVM.SICCodeManual);    
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.companyVM.ID))
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
            return View(company);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.ID == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var company = await _context.Companies.FindAsync(id);
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        //public async Task<IActionResult> MailMergeWordDoc(string id)
        //{
        //    var x = new MailMergeHelper(_context, _webHostEnvironment, id, "Contacts", "Form-Opp - Term Sheet - Standard.doc");
        //    x.WordDocumentMailMerge();

        //    return Content(x.ToString());
        //}


        public async Task<IActionResult> MergeContacts(string id)
        {
            ViewBag.CompanyName = _companyHelper.GetCompanyName(id);
            var contactsForMerge = GetContactsForMerge(id);
            if (contactsForMerge != null)
            {
                return View(contactsForMerge);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MergeContacts(IFormCollection collection)
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


        public ActionResult MergeChosenContacts(string chosenContacts)
        {
            List<string> contactList = new List<string>();
            List<Contact> mergeableContacts = new List<Contact>();
            var companyMergeContactsVM = new CompanyMergeContactsViewModel
            {
                ContactIDList = new List<string>(),
                FirstNameList = new List<string>(),
                LastNameList = new List<string>(),
                PhoneList = new List<string>(),
                MobilePhoneList = new List<string>(),
                EmailList = new List<string>(),
                OwnerList = new List<OwnerInfo>()
            };

            if (chosenContacts != null)
            {
                contactList = chosenContacts.Split(',').ToList();
            }

            foreach (string contact in contactList)
            {
                var newContact = _context.Contacts.Where(c => c.Id == contact).FirstOrDefault();
                mergeableContacts.Add(newContact);
            }

            for (int i = 0; i < mergeableContacts.Count(); i++)
            {
                companyMergeContactsVM.ContactIDList.Add(mergeableContacts[i].Id.ToString());
                companyMergeContactsVM.FirstNameList.Add(mergeableContacts[i].FirstName.ToString());
                companyMergeContactsVM.LastNameList.Add(mergeableContacts[i].LastName);
                companyMergeContactsVM.PhoneList.Add(mergeableContacts[i].Phone);
                companyMergeContactsVM.MobilePhoneList.Add(mergeableContacts[i].MobilePhone);
                companyMergeContactsVM.EmailList.Add(mergeableContacts[i].Email);
                var ownerInfo = new OwnerInfo()
                {
                    OwnerID = mergeableContacts[i].OwnerId,
                    OwnerName = _userHelper.GetNameFromID(mergeableContacts[i].OwnerId),
                };
                companyMergeContactsVM.OwnerList.Add(ownerInfo);
            }

            return View(companyMergeContactsVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MergeChosenContacts(FormCollection collection)
        {
            var contactid = collection["cbContactID"];
            var fname = collection["cbFirstName"];
            var lname = collection["cbLastName"];
            var phone = collection["cbPhone"];
            var mobile = collection["cbMobilePhone"];
            var email = collection["cbEmail"];
            var ownerid = collection["cbOwner"];

            var contact = _context.Contacts.Where(c => c.Id == contactid).FirstOrDefault();
            if (contact != null)
            {
                contact.FirstName = fname;
                contact.LastName = lname;
                contact.Phone = phone;
                contact.MobilePhone = mobile;
                contact.Email = email;
                contact.OwnerId = ownerid;

                _context.SaveChanges();
            }

            return Content("asdf");
        }





        #region JSON Calls
        [HttpPost]
        public JsonResult GetCompanyChartInfo()
        {
            var companies = new List<CompanyIndexViewModel>();
            try
            {
                var currentCompanies = _context.Companies.OrderByDescending(c=>c.LastModifiedDate).ToList();
                if (currentCompanies.Count > 0)
                {
                    foreach (var currentCompany in currentCompanies)
                    {
                        var currentDescription = (currentCompany.Description != null) ? currentCompany.Description.Replace("<br>", "\n") : "";
                        companies.Add(new CompanyIndexViewModel
                        {
                            ID = currentCompany.ID,
                            name = currentCompany.Name,
                            companytype = currentCompany.CompanyType,
                            description = $"{currentDescription}",
                            state = currentCompany.CharterState,
                            lastmodifieddate = currentCompany.LastModifiedDate
                        });
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return Json(companies);
        }

        #endregion












        private bool CompanyExists(string id)
        {
            return _context.Companies.Any(e => e.ID == id);
        }



        #region Company Validation
        private Company CreateCompany_ConvertToDataLayer(CompanyCreateIndividualCompanyViewModel company)
        {
            var returnCompany = new Company()
            {
                Name = company.Name,
                Description = company.Description,
                CompanyType = company.CompanyType,
                SICCode = (company.SICCode == null) ? company.SICCodeManual : company.SICCode,
                CharterState = company.CharterState,
                MailingAddress = company.MailingAddress,
                MailingCity = company.MailingCity,
                MailingState = company.MailingState,
                MailingPostalCode = company.MailingPostalCode,
                CreatedBy = _userHelper.GetIDFromName(User.Identity.Name),
                CreatedDate = DateTime.Now
            };
            return returnCompany;
        }

        private CompanyEditIndividualCompanyViewModel EditCompany_ConvertToViewModel(Company company)
        {
            var returnCompanyVM = new CompanyEditIndividualCompanyViewModel()
            {
                ID = company.ID,
                Name = company.Name,
                Description = company.Description,
                CompanyType = company.CompanyType,
                SICCode = company.SICCode,
                CharterState = company.CharterState,
                MailingAddress = company.MailingAddress,
                MailingCity = company.MailingCity,
                MailingState = company.MailingState,
                MailingPostalCode = (company.MailingPostalCode==null)?"": company.MailingPostalCode.ToString(),
                CreatedDate = company.CreatedDate,
                CreatedBy = company.CreatedBy,
                LastModifiedDate = company.LastModifiedDate,
                LastUpdatedBy = _userHelper.GetIDFromName(User.Identity.Name),
            };
            return returnCompanyVM;
        }


        private List<Contact> GetContactsForMerge(string companyID)
        {
            List<Contact> activeContacts;
            activeContacts = _context.Contacts.Where(c => c.AccountId == companyID && c.Relationship_Status != "Dead").ToList();
            return activeContacts;
        }




        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> DoesCompanyExist([Bind(Prefix = "companyVM.name")] string name)
        {
            using (_context)
            {
                var company = await _context.Companies.FirstOrDefaultAsync(t => t.Name == name);
                if (company == null)
                {
                    return Json(true);
                }
                return Json($"{name} is already in use...");
            }
        }
        #endregion





    }
}
