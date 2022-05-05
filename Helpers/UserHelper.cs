using AmeriForce.Data;
using System.Linq;

namespace AmeriForce.Helpers
{
    public class UserHelper
    {
        private readonly ApplicationDbContext _context;

        public UserHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetNameFromID(string id)
        {
            string userName = "";

            var user = _context.Users.Where(u => u.Id == id).FirstOrDefault();
            if (user != null)
            {
                userName = $"{user.FirstName} {user.LastName}";
            }

            return userName;
        }


        public string GetIDFromName(string name)
        {
            string userID = "";
            
                var user = _context.Users.Where(u => u.UserName == name).FirstOrDefault();
                if (user != null)
                {
                    userID = $"{user.Id}";
                }
            
            return userID;
        }


        public string GetNameFromEmail(string userName)
        {
            string userFLName = "";
            
                var user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();
                if (user != null)
                {
                    userFLName = $"{user.FirstName} {user.LastName}";
                }
            
            return userFLName;
        }


        public string GetEmailFromUserName(string userName)
        {
            string userEmail = "";
            
                var user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();
                if (user != null)
                {
                    userEmail = $"{user.Email}";
                }
            
            return userEmail;
        }


    }
}
