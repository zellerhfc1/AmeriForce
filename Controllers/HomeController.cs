using AmeriForce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AmeriForce.Services;
using AmeriForce.Models.Email;
using Microsoft.Extensions.Configuration;
using AmeriForce.Data;
using AmeriForce.Models.Home;
using Microsoft.AspNetCore.Http;
using AmeriForce.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace AmeriForce.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly EmailConfigurationViewModel _emailConfig = new EmailConfigurationViewModel();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private UserHelper _userHelper;
        private UploadHelper _uploadHelper;
        private ContactHelper _contactHelper;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _userHelper = new UserHelper(_context);
            _uploadHelper = new UploadHelper(_context, _webHostEnvironment);
            _contactHelper = new ContactHelper(_context);
            configuration.GetSection("EmailConfiguration").Bind(_emailConfig);
        }

        public IActionResult Index()
        {
            ViewBag.UserName = _userHelper.GetNameFromUserName(User.Identity.Name);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        [HttpPost]
        public JsonResult GetUserTasks()
        {
            var tasks = new List<CRMTaskSchedulerViewModel>();
            try
            {
                var currentTasks = _context.CRMTasks.Where(c => c.OwnerId == _userHelper.GetIDFromName(User.Identity.Name)).ToList();
                if (currentTasks.Count > 0)
                {
                    foreach (var currentTask in currentTasks)
                    {
                        var currentDescription = currentTask.Description.Replace("<br>", "\n");
                        tasks.Add(new CRMTaskSchedulerViewModel
                        {
                            text = _contactHelper.GetName(currentTask.WhoId),
                            employeeID = 1,
                            startDate = currentTask.ActivityDate,
                            description = $"{currentTask.Type}\n{currentDescription}",
                        });
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return Json(tasks);
        }


        [HttpPost]
        public JsonResult GetUserInfo()
        {
            var userInfos = new List<UserInformationForHomePageViewModel>();
            try
            {
                var currentUsers = _context.Users.Where(c => c.UserName == User.Identity.Name).ToList();
                if (currentUsers.Count > 0)
                {
                    foreach (var currentUser in currentUsers)
                    {
                        userInfos.Add(new UserInformationForHomePageViewModel
                        {
                            id = 1, // Default user id for the task schedule viewer
                            text = $"{currentUser.FirstName} {currentUser.LastName}",
                            color = $"#005eb8", // Base Amerisource blue
                            avatar = (_uploadHelper.FileExists($"images/{currentUser.Id}.jpg") == true) ? $"images/{currentUser.Id}.jpg": $"images/baseUser.jpg",
                            discipline = $"{currentUser.Department}",
                        }); ;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return Json(userInfos);
        }



        [HttpPost]
        public JsonResult GetChartInfo()
        {
            var userInfos = new List<GetChartInfoViewModel>();
            try
            {
                var currentUsers = _context.Users.Where(u => u.IsActive == true && u.Department == "Business Development").OrderByDescending(c=>c.FirstName).ToList();
                if (currentUsers.Count > 0)
                {
                    foreach (var currentUser in currentUsers)
                    {
                        Random rando = new Random();

                        userInfos.Add(new GetChartInfoViewModel
                        {
                            bdo = $"{currentUser.FirstName} {currentUser.LastName}",
                            valueCount = rando.Next(1, 20)
                        });
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return Json(userInfos.OrderBy(u=>u.valueCount));
        }

        public JsonResult WTFTest()
        {
            var userInfo = new UserInformationForHomePageViewModel();
            try
            {
                var currentUser = _context.Users.Where(c => c.UserName == User.Identity.Name).FirstOrDefault();
                if (currentUser != null)
                {
                    userInfo.id = 1; // Default user id for the task schedule viewer
                    userInfo.text = $"{currentUser.FirstName}{currentUser.LastName}";
                    userInfo.color = $"#005eb8"; // Base Amerisource blue
                    userInfo.avatar = $"images/{currentUser.FirstName.FirstOrDefault()}{currentUser.LastName}.png";
                    userInfo.discipline = $"{currentUser.Department}";
                }
            }
            catch (Exception ex)
            {

            }

            return Json(userInfo);
        }


        public string GetUserNameFromID(string id)
        {
            return _userHelper.GetNameFromID(id);
        }

        public string GetIDFromUserName(string id)
        {
            return _userHelper.GetIDFromName(User.Identity.Name);
        }


    }
}
