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
                //var currentTasks = _context.CRMTasks.Where(c => c.OwnerId == _userHelper.GetIDFromName(User.Identity.Name)).ToList();

                var currentTasks = from t in _context.CRMTasks
                               join c in _context.Contacts on t.Id equals c.NextActivityID
                                   where t.OwnerId == _userHelper.GetIDFromName(User.Identity.Name)
                                   select new { 
                                   t, 
                                   c 
                               };

                if (currentTasks != null)
                {
                    foreach (var currentTask in currentTasks)
                    {
                        var taskIcon = "<i class='fa fa-clipboard' title='Unknown' style='font-size:x-large;'></i>";
                        switch (currentTask.t.Type.ToUpper())
                        {
                            case "EMAIL":
                                taskIcon = $"<i class='fa fa-envelope' title='Email' style='font-size:x-large;'></i>";
                                break;
                            case "IN-PERSON MEETING":
                                taskIcon = $"<i class='fa fa-handshake' title='In-Person Meeting' style='font-size:x-large;'></i>";
                                break;
                            case "PHONE CALL":
                                taskIcon = $"<i class='fa fa-mobile-alt' title='Phone Call' style='font-size:x-large;'></i>";
                                break;
                            case "VIRTUAL MEETING":
                                taskIcon = $"<i class='fa fa-video' title='Zoom, Skype, etc.' style='font-size:x-large;'></i>";
                                break;
                            case "OTHER":
                                taskIcon = $"<i class='fa fa-clipboard' title='Other' style='font-size:x-large;'></i>";
                                break;
                            case "WARNING":
                                taskIcon = $"<i class='fa fa-exclamation-triangle' title='Warning' style='font-size:x-large;'></i>";
                                break;
                            default:
                                break;
                        }

                        var currentDescription = currentTask.t.Description.Replace("<br>", "\n");
                        tasks.Add(new CRMTaskSchedulerViewModel
                        {
                            text = $"{currentTask.c.FirstName} {currentTask.c.LastName}",
                            employeeID = 1, //currentTask.c.Id,
                            startDate = currentTask.t.ActivityDate,
                            taskType = $"{currentTask.t.Type}",
                            description = $"{currentDescription}",
                            taskIcon = $"{taskIcon}",
                            contactID = $"{currentTask.c.Id}",
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
        public JsonResult GetNewUnassignedContacts()
        {
            var contactInfos = new List<GetChartInfoViewModel>();
            try
            {
                var currentContacts = _context.Contacts
                                                    .Where(c => c.Relationship_Status.ToUpper() == "NEW")
                                                    .GroupBy(c => c.OwnerId)
                                                    .Select(c => new { c.Key, Count = c.Count() });
                if (currentContacts != null)
                {
                    foreach (var currentContact in currentContacts)
                    {
                        contactInfos.Add(new GetChartInfoViewModel
                        {
                            bdo = $"{_userHelper.GetNameFromID(currentContact.Key)}",
                            valueCount = currentContact.Count
                        });
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return Json(contactInfos.OrderBy(u => u.valueCount));
        }



        [HttpPost]
        public JsonResult GetFundedDealsYTD()
        {
            var clientOwnerInfos = new List<GetChartInfoViewModel>();
            try
            {
                DateTime firstDayOfCurrentYear = new DateTime(DateTime.Now.Year, 1, 1);
                var currentClientOwnersData = _context.Clients
                                                    .Where(c => c.StageName.ToUpper() == "50-FUNDED" && c.Initial_Funding >= firstDayOfCurrentYear)
                                                    .GroupBy(c => c.OwnerId)
                                                    .OrderByDescending(c => c.Sum(x => x.Amount))
                                                    .Select(c => new { c.Key, Count = c.Count(), Amounts = c.Sum(x => x.Amount) })
                                                    .ToList();
                                                    //.Select(c => new { c.Key, Count = c.Count(), Amounts = c. });
                if (currentClientOwnersData != null)
                {
                    foreach (var currentClientOwnerData in currentClientOwnersData)
                    {
                        clientOwnerInfos.Add(new GetChartInfoViewModel
                        {
                            bdo = $"{_userHelper.GetNameFromID(currentClientOwnerData.Key)}",
                            valueCount = Convert.ToInt32(currentClientOwnerData.Amounts)
                        });
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return Json(clientOwnerInfos.OrderBy(u => u.valueCount));
        }


        [HttpPost]
        public JsonResult GetDealsByLeadSourceYTD()
        {
            var clientOwnerInfos = new List<GetChartInfoViewModel>();
            try
            {
                DateTime firstDayOfCurrentYear = new DateTime(DateTime.Now.Year, 1, 1);
                var currentClientOwnersData = _context.Clients
                                                    .Where(c => c.Initial_Funding >= firstDayOfCurrentYear)
                                                    .GroupBy(c => c.LeadSource)
                                                    .OrderByDescending(c => c.Sum(x => x.Amount))
                                                    .Select(c => new { c.Key, Count = c.Count(), Amounts = c.Sum(x => x.Amount) })
                                                    .ToList();
                //.Select(c => new { c.Key, Count = c.Count(), Amounts = c. });
                if (currentClientOwnersData != null)
                {
                    foreach (var currentClientOwnerData in currentClientOwnersData)
                    {
                        clientOwnerInfos.Add(new GetChartInfoViewModel
                        {
                            bdo = $"{currentClientOwnerData.Key}",
                            valueCount = Convert.ToInt32(currentClientOwnerData.Count)
                        });
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return Json(clientOwnerInfos.OrderBy(u => u.valueCount));
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
