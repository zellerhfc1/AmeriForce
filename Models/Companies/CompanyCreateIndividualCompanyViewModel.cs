using AmeriForce.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AmeriForce.Models.Companies
{
    public class CompanyCreateIndividualCompanyViewModel
    {

        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Company Name")]
        [Remote(action: "DoesCompanyExist", controller: "Companies", ErrorMessage = "This company already exists.")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Display(Name = "SIC Code")]
        public string SICCode { get; set; }
        public string SICCodeManual { get; set; }

        [Display(Name = "Charter State")]
        public string CharterState { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public string MailingAddress { get; set; }
        public string MailingCity { get; set; }

        public string MailingState { get; set; }

        [Range(00001, 99999)]
        [MinLength(5)]
        [MaxLength(5)]
        public string MailingPostalCode { get; set; }

    }
}
