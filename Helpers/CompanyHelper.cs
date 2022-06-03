using AmeriForce.Data;
using System.Collections.Generic;
using System.Linq;

namespace AmeriForce.Helpers
{
    public class CompanyHelper
    {
        private readonly ApplicationDbContext _context;

        public CompanyHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetCompanyName(string id)
        {
            string companyName = "";
            if (id != "")
            {
                var companyNameTemp = _context.Companies.Where(c => c.ID == id).FirstOrDefault();
                if (companyNameTemp != null)
                {
                    companyName = _context.Companies.Where(c => c.ID == id).FirstOrDefault().Name;
                }
            }
            return companyName;
        }

        public string GetCompanyIDFromName(string companyName)
        {
            string companyID = "";
            if (!string.IsNullOrEmpty(companyName))
            {
                var companyTemp = _context.Companies.Where(c => c.Name == companyName.Trim()).FirstOrDefault();
                if (companyTemp != null)
                {
                    companyID = _context.Companies.Where(c => c.Name == companyName.Trim()).FirstOrDefault().ID;
                }
            }
            return companyID;
        }

        public string GetBusinessDescription(string id)
        {
            string businessDescription = "";
            businessDescription = _context.Companies.Where(c => c.ID == id).FirstOrDefault().Description;

            return businessDescription;
        }


        public string GetSICCode(string id)
        {
            string sicCode = "";
            sicCode = _context.Companies.Where(c => c.ID == id).FirstOrDefault().SICCode;

            return sicCode;
        }


        public string GetCharterState(string id)
        {
            string charterState = "";
            charterState = _context.Companies.Where(c => c.ID == id).FirstOrDefault().CharterState;

            return charterState;
        }

        public void AddNewSICCodeToList(string sicCode)
        {
            if (sicCode != null)
            {
                var existingSICCode = _context.SICCodes.Where(s => s.ID == sicCode).FirstOrDefault();
                if (existingSICCode == null)
                {
                    _context.SICCodes.Add(new SICCode
                    {
                        ID = sicCode,
                        Value = sicCode,
                        CreatedDate = System.DateTime.Now
                    });
                    _context.SaveChanges();
                }
            }
        }

        public IEnumerable<Company> GetAllCompanies()
        {
            var existingCompanies = _context.Companies.OrderBy(c => c.Name);

            return existingCompanies;
        }
    }
}
