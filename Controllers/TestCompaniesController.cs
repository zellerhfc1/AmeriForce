using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AmeriForce.Data;
using AmeriForce.Models.Test;
using System.Diagnostics;

namespace AmeriForce.Controllers
{
    public class TestCompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestCompaniesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TestCompanies
        public async Task<IActionResult> Index()
        {
            return View(await _context.TestCompany.ToListAsync());
        }

        // GET: TestCompanies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testCompany = await _context.TestCompany
                .FirstOrDefaultAsync(m => m.ID == id);

            if (testCompany == null)
            {
                return NotFound();
            }

            return View(testCompany);
        }


        // GET: TestCompanies/Create
        public IActionResult Create3()
        {
            TestCompanyEditModel viewModel = new TestCompanyEditModel();
            TestCompanyIndividualCompany iCompany = new TestCompanyIndividualCompany();
            viewModel._testCompany = iCompany;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create3(TestCompanyEditModel testCompanyEditModel)
        {
            if (ModelState.IsValid)
            {
                var convertCompany = ConvertToCompanyData(testCompanyEditModel._testCompany);

                _context.Add(convertCompany);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(testCompanyEditModel);
        }

        private TestCompany ConvertToCompanyData(TestCompanyIndividualCompany testCompany)
        {
            var returnCompany = new TestCompany()
            {
                ID  = testCompany.ID,
                Name = testCompany.CompanyName,
                Description = testCompany.Description,
                SICCode = testCompany.SICCode,
                CharterState = testCompany.CharterState,
                MailingAddress = testCompany.MailingAddress,
                MailingCity = testCompany.MailingCity,
                MailingState = testCompany.MailingState,
                MailingPostalCode = Convert.ToInt32(testCompany.MailingPostalCode),
                CreatedDate = testCompany.CreatedDate,
                LastModifiedDate = testCompany.LastModifiedDate
            };
            return returnCompany;
        }

        private TestCompanyIndividualCompany ConvertToCompanyViewModel(TestCompany testCompanyVM)
        {
            var returnCompanyVM = new TestCompanyIndividualCompany()
            {
                ID = testCompanyVM.ID,
                CompanyName = testCompanyVM.Name,
                Description = testCompanyVM.Description,
                SICCode = testCompanyVM.SICCode,
                CharterState = testCompanyVM.CharterState,
                MailingAddress = testCompanyVM.MailingAddress,
                MailingCity = testCompanyVM.MailingCity,
                MailingState = testCompanyVM.MailingState,
                MailingPostalCode = testCompanyVM.MailingPostalCode.ToString(),
                CreatedDate = testCompanyVM.CreatedDate,
                LastModifiedDate = testCompanyVM.LastModifiedDate
            };
            return returnCompanyVM;
        }

        //public static explicit operator Ameriforce.Data.TestCompany(AmeriForce.Models.Test.TestCompanyIndividualCompany v)
        //{
        //    TestCompany tco = new TestCompany(v);
        //}

        // GET: TestCompanies/Create
        //public IActionResult Create()
        //{
        //    TestCompanyEditModel model = new TestCompanyEditModel();
        //    return View(model);
        //}

        // POST: TestCompanies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ID,Name,UpdateDate")] TestCompany testCompany)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(testCompany);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(testCompany);
        //}

        // GET: TestCompanies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TestCompanyEditModel viewModel = new TestCompanyEditModel();

            var testCompany = await _context.TestCompany.FindAsync(id);
            var convertedCompany = ConvertToCompanyViewModel(testCompany);

            viewModel._testCompany = convertedCompany;

            if (testCompany == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        // POST: TestCompanies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TestCompanyEditModel returnModel)
        {
            if (id != returnModel._testCompany.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var convertedCompany = ConvertToCompanyData(returnModel._testCompany);
                    _context.Update(convertedCompany);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestCompanyExists(returnModel._testCompany.ID))
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
            return View(returnModel);
        }

        // GET: TestCompanies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testCompany = await _context.TestCompany
                .FirstOrDefaultAsync(m => m.ID == id);
            if (testCompany == null)
            {
                return NotFound();
            }

            return View(testCompany);
        }

        // POST: TestCompanies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testCompany = await _context.TestCompany.FindAsync(id);
            _context.TestCompany.Remove(testCompany);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestCompanyExists(int id)
        {
            return _context.TestCompany.Any(e => e.ID == id);
        }

        #region Contact Validation
        [AcceptVerbs("Get", "Post")]
       public async Task<IActionResult> DoesCompanyExist([Bind(Prefix = "_testCompany.companyname")] string companyname)
        {
            using (_context)
            {
                var company = await _context.TestCompany.FirstOrDefaultAsync(t => t.Name == companyname);
                if (company == null)
                {
                    return Json(true);
                }
                return Json($"{companyname} is already in use...");
            }
        }
        #endregion
    }
}
