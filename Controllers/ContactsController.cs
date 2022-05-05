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

namespace AmeriForce.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private GuidHelper _guidHelper = new GuidHelper();
        private CompanyHelper _companyHelper;
        private LOVHelper _lovHelper;
        private UserHelper _userHelper;

        public ContactsController(ApplicationDbContext context)
        {
            _context = context;
            _lovHelper = new LOVHelper(_context);
            _companyHelper = new CompanyHelper(_context);
            _userHelper = new UserHelper(_context);
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

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
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


    }
}
