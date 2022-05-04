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

namespace AmeriForce.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private GuidHelper _guidHelper = new GuidHelper();
        private CompanyHelper _companyHelper;
        private LOVHelper _lovHelper;

        public CompaniesController(ApplicationDbContext context)
        {
            _context = context;
            _lovHelper = new LOVHelper(_context);
            _companyHelper = new CompanyHelper(_context);
        }

        // GET: Companies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Companies.OrderByDescending(c=>c.CreatedDate).ToListAsync());
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Details(string id)
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

        // GET: Companies/Create
        public IActionResult Create()
        {
            CompanyCreateViewModel createCompanyVM = new CompanyCreateViewModel();
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
                    var convertCompany = EditCompany_ConvertToDataLayer(company.companyVM);
                    if (company.companyVM.SICCodeManual != null)
                    {
                        _companyHelper.AddNewSICCodeToList(company.companyVM.SICCodeManual);    
                    }
                    _context.Update(convertCompany);
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
                SICCode = (company.SICCode == null) ? company.SICCodeManual : company.SICCode,
                CharterState = company.CharterState,
                MailingAddress = company.MailingAddress,
                MailingCity = company.MailingCity,
                MailingState = company.MailingState,
                MailingPostalCode = company.MailingPostalCode
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
                SICCode = company.SICCode,
                CharterState = company.CharterState,
                MailingAddress = company.MailingAddress,
                MailingCity = company.MailingCity,
                MailingState = company.MailingState,
                MailingPostalCode = (company.MailingPostalCode==null)?"": company.MailingPostalCode.ToString(),
                CreatedDate = company.CreatedDate,
                LastModifiedDate = company.LastModifiedDate
            };
            return returnCompanyVM;
        }
        private Company EditCompany_ConvertToDataLayer(CompanyEditIndividualCompanyViewModel company)
        {
            var returnCompany = new Company()
            {
                ID= company.ID,
                Name = company.Name,
                Description = company.Description,
                SICCode = (company.SICCode == null) ? company.SICCodeManual : company.SICCode,
                CharterState = company.CharterState,
                MailingAddress = company.MailingAddress,
                MailingCity = company.MailingCity,
                MailingState = company.MailingState,
                MailingPostalCode = company.MailingPostalCode
            };
            return returnCompany;
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
