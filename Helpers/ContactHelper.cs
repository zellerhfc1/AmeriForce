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
            string contactName = "Not Found";

            if (id != "")
            {
                var contact = _context.Contacts.Where(c => c.Id == id).FirstOrDefault();
                if (contact != null)
                {
                    if (contact.FirstName == null)
                    {
                        contactName = $"{contact.LastName}";
                    } else { 
                        contactName = $"{contact.FirstName} {contact.LastName}";
                    }
                    
                }
            }
            return contactName;
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
