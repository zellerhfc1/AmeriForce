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
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using AmeriForce.Helpers;

namespace AmeriForce.Controllers
{
    [Authorize]
    public class TestCompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public TestCompaniesController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }


        public async Task<IActionResult> FileExists()
        {
            var x = new UploadHelper(_context, _environment).FileExists($"images/loriSquare.png");
            var y = new UploadHelper(_context, _environment).FileExists($"images/JZeller.jpg");

            return Content($"{x}<br>{y}");

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

        //public void MergeList()
        //{
        //    var x = new MailMergeHelper(_context, _environment, "00379-ff681a618ac9", "Clients", "Form-Opp - Term Sheet - Standard.doc");

        //    x.WordDocumentMailMerge();

        //    var y = 1;


        //}

        public void MergeTestDevexpress()
        {
            //using (RichEditDocumentServer server = new RichEditDocumentServer())
            //{
            //    server.LoadDocument("Documents//invitation.docx", DocumentFormat.Rtf);
            //    server.Options.MailMerge.DataSource = new SampleData();

            //    MailMergeOptions myMergeOptions = server.Document.CreateMailMergeOptions();
            //    myMergeOptions.FirstRecordIndex = 1;
            //    myMergeOptions.MergeMode = MergeMode.NewParagraph;
            //    server.MailMerge(myMergeOptions, "result.docx", DocumentFormat.OpenXml);
            //}
            //System.Diagnostics.Process.Start("result.docx");
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
