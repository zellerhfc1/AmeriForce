using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AmeriForce.Data;

namespace AmeriForce.Controllers
{
    public class TestCompanies1Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestCompanies1Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TestCompanies1
        public async Task<IActionResult> Index()
        {
            return View(await _context.TestCompany.ToListAsync());
        }

        // GET: TestCompanies1/Details/5
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

        // GET: TestCompanies1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TestCompanies1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,SICCode,CharterState,CreatedDate,LastModifiedDate,MailingAddress,MailingCity,MailingState,MailingPostalCode")] TestCompany testCompany)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testCompany);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(testCompany);
        }

        // GET: TestCompanies1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testCompany = await _context.TestCompany.FindAsync(id);
            if (testCompany == null)
            {
                return NotFound();
            }
            return View(testCompany);
        }

        // POST: TestCompanies1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,SICCode,CharterState,CreatedDate,LastModifiedDate,MailingAddress,MailingCity,MailingState,MailingPostalCode")] TestCompany testCompany)
        {
            if (id != testCompany.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testCompany);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestCompanyExists(testCompany.ID))
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
            return View(testCompany);
        }

        // GET: TestCompanies1/Delete/5
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

        // POST: TestCompanies1/Delete/5
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
    }
}
