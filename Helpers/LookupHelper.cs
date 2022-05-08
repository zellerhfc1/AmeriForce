using AmeriForce.Data;
using System.Linq;

namespace AmeriForce.Helpers
{
    public class LookupHelper
    {
        private readonly ApplicationDbContext _context;

        public LookupHelper(ApplicationDbContext context)
        {
            _context = context;
        }


        internal string GetUserNameFromID(string id)
        {
            string userName = "---";
            var user = _context.Users.Where(u => u.Id == id).FirstOrDefault();
            if (user != null)
            {
                userName = $"{user.FirstName} {user.LastName}";
            }

            return userName;
        }
    }
}
