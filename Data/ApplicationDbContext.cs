using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using AmeriForce.Models.Test;
using AmeriForce.Data;
using System.Linq;

namespace AmeriForce.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
      
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public override int SaveChanges()
        {
            var modifiedEntities = ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Modified).ToList();

            var addedEntities = ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Added).ToList();

            var now = DateTime.UtcNow;

            foreach (var change in modifiedEntities)
            {
                var entityName = change.Entity.GetType().Name;
                //var primaryKey = GetPrimaryKeyValue(change);

                //foreach (var prop in change.OriginalValues.PropertyNames)
                //{
                //    var originalValue = change.OriginalValues[prop].ToString();
                //    var currentValue = change.CurrentValues[prop].ToString();
                //    if (originalValue != currentValue) //Only create a log if the value changes
                //    {
                //        //Create the Change Log
                //    }
                //}
            }
            return base.SaveChanges();
        }

        public DbSet<TestCompany> TestCompany { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientContactRole> ClientContactRoles { get; set; }
        public DbSet<ClientStage> ClientStages { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<CRMTask> CRMTasks { get; set; }
        public DbSet<EmailMessage> EmailMessages { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<LOV_ClientStatus> LOV_ClientStatus { get; set; }
        public DbSet<LOV_ReferralType> LOV_ReferralType { get; set; }
        public DbSet<LOV_State> LOV_State { get; set; }
        public DbSet<LOV_TaskType> LOV_TaskType { get; set; }
        public DbSet<LOV_TemplateType> LOV_TemplateType { get; set; }
        public DbSet<NewInitialDeal> NewInitialDeals { get; set; }
        public DbSet<SICCode> SICCodes { get; set; }

    }
}
