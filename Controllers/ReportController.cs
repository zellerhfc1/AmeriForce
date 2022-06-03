using AmeriForce.Data;
using AspNetCore.Reporting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmeriForce.Controllers
{

    [Authorize]
    public class ReportController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;

        public ReportController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context)
        {
            this._webHostEnvironment = webHostEnvironment;
            this._context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CurrentPipeline()
        {
            return View();
        }

        public IActionResult GetContactsInSpecificStates()
        {
            return View();
        }

        public IActionResult TermSheetsPending()
        {
            return View();
        }

        public IActionResult Print(int id)
        {
            var rendertype = RenderType.Pdf;
            string mimeType = "application/pdf";
            
            if (id != 0)
            {
                rendertype = RenderType.Excel;
                mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }

            int extension = 1;
            var reportPath = $"{this._webHostEnvironment.WebRootPath}\\Reports\\Test.rdl";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("p1", "Fake Report Heading Bruh");
            LocalReport localReport = new LocalReport(reportPath);
            var result = localReport.Execute(rendertype, extension, parameters, mimeType);


            return File(result.MainStream, mimeType);
        }


        #region JSON Functions




        [HttpPost]
        public JsonResult GetPipelineReports()
        {
            try
            {
                var currentPipeline = _context.Clients
                                            .Where(c => c.StageName != "80-Hold" && c.StageName != "90-Lost" && c.StageName != "95-Declined By ASF" && c.StageName != "50-Funded");
                //if (currentClients.Count > 0)
                //{
                //    foreach (var currentClient in currentClients)
                //    {
                //        clients.Add(new ClientIndexViewModel
                //        {
                //            ID = currentClient.Id,
                //            OwnerName = _userHelper.GetNameFromID(currentClient.OwnerId),
                //            Name = currentClient.Name,
                //            Company = _companyHelper.GetCompanyName(currentClient.CompanyId),
                //            StageName = currentClient.StageName,
                //            Amount = currentClient.Amount,
                //            CloseDate = currentClient.CloseDate
                //        });
                //    }
                //}

                return Json(currentPipeline);
            }
            catch (Exception ex)
            {

            }

            return Json("");
        }





        #endregion




    }
}
