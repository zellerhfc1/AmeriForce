using AmeriForce.Data;
using AmeriForce.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace AmeriForce.Models.Companies
{
    public class CompanyCreateViewModel
    {

        public int ID { get; set; }
        public CompanyCreateIndividualCompanyViewModel companyVM { get; set; }
        public DateTime UpdateDate { get; set; }

        public IEnumerable<SelectListItem> StateList { get; set; }
        public IEnumerable<SelectListItem> SICCodesList {get; set; }
    }
}
