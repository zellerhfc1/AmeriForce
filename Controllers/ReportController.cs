using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AmeriForce.Controllers
{
    public class ReportController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReportController(IWebHostEnvironment webHostEnvironment)
        {
            this._webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
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
    }
}
