using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace AmeriForce.Models.Test
{
    public class TestCompanyIndividualCompany
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Company Name")]
        [Remote(action: "DoesCompanyExist", controller: "TestCompanies", ErrorMessage = "This company already exists.")]
        public string CompanyName { get; set; }

        public string Description { get; set; }

        public string SICCode { get; set; }
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
