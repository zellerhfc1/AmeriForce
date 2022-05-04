using AmeriForce.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace AmeriForce.Models.Test
{
    public class TestCompanyEditModel
    {
        public int ID { get; set; }
        public TestCompanyIndividualCompany _testCompany { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
