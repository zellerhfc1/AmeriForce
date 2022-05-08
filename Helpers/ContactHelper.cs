using AmeriForce.Data;
using System.Linq;

namespace AmeriForce.Helpers
{
    public class ContactHelper
    {
        private readonly ApplicationDbContext _context;

        public ContactHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetName(string id)
        {
            string clientName = "";

            if (id != "")
            {
                var contact = _context.Contacts.Where(c => c.Id == id).FirstOrDefault();
                clientName = $"{contact.FirstName} {contact.LastName}";

            }
            return clientName;
        }


        public string GetEmailAddress(string id)
        {
            string email = "";

            if (id != "")
            {
                var contact = _context.Contacts.Where(c => c.Id == id).FirstOrDefault();
                email = $"{contact.Email}";

            }
            return email;
        }
    }
}
