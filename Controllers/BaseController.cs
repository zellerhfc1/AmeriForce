using AmeriForce.Data;
using AmeriForce.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AmeriForce.Controllers
{
    public class BaseController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private UserHelper _userHelper;
        public string UserID { get; set; }
        private string UserName { get; set; }
        private string UserEmail { get; set; }

        public BaseController(ApplicationDbContext _context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _userHelper = new UserHelper(_context);

            var user = _userManager.GetUserAsync(HttpContext.User);

            if (user != null)
            {
                UserName = $"{user.Result.FirstName} {user.Result.LastName}";     
                UserID = user.Result.UserName;  
                UserEmail = user.Result.Email;  
            }
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
