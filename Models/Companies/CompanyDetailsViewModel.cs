using AmeriForce.Data;
using System.Collections.Generic;

namespace AmeriForce.Models.Companies
{
    public class CompanyDetailsViewModel
    {
        public Company company { get; set; }

        public IEnumerable<Contact> contacts { get; set; }
    }
}
