using AmeriForce.Data;
using AmeriForce.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace AmeriForce.Models.Companies
{
    public class CompanyEditViewModel
    {

        public string ID { get; set; }
        public CompanyEditIndividualCompanyViewModel companyVM { get; set; }
        public DateTime UpdateDate { get; set; }

        public IEnumerable<SelectListItem> CompanyTypeList { get; set; }
        public IEnumerable<SelectListItem> StateList { get; set; }
        public IEnumerable<SelectListItem> SICCodesList {get; set; }
    }
}
