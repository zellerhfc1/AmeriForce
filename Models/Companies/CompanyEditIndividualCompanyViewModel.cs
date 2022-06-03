using AmeriForce.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AmeriForce.Models.Companies
{
    public class CompanyEditIndividualCompanyViewModel
    {

        public string ID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Company Name")]
        //[Remote(action: "DoesCompanyExist", controller: "Companies", ErrorMessage = "This company already exists.")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "SIC Code")]
        public string SICCode { get; set; }

        [Display(Name = "SIC Code Override")]
        public string SICCodeManual { get; set; }

        [Display(Name = "Referral Type")]
        public string CompanyType { get; set; }

        [Display(Name = "Charter State")]
        public string CharterState { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Last Update Date")]

        public DateTime LastModifiedDate { get; set; }

        public string CreatedBy { get; set; }

        public string LastUpdatedBy { get; set; }

        [Display(Name = "Address")]
        public string MailingAddress { get; set; }

        [Display(Name = "City")]
        public string MailingCity { get; set; }

        [Display(Name = "State")]
        public string MailingState { get; set; }

        [Range(00001, 99999)]
        [MinLength(5)]
        [MaxLength(5)]
        [Display(Name = "ZipCode")]
        public string MailingPostalCode { get; set; }

    }
}
