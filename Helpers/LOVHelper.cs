using AmeriForce.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace AmeriForce.Helpers
{
    public class LOVHelper
    {
        private readonly ApplicationDbContext _context;

        public LOVHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetBaseList()
        {
            // GET BASE LIST
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

            var defaultItem = new SelectListItem();
            defaultItem.Text = "";
            defaultItem.Value = "";
            sList.Add(defaultItem);

            return sList;

        }

        public IEnumerable<SelectListItem> GetCompanyDropdownList()
        {
            // GET COMPANIES LIST
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

            //using (var db = new ApplicationDbContext())
            //{

                var companies = _context.Companies.OrderBy(l => l.Name).ToList();

                var defaultItem = new SelectListItem();
                defaultItem.Text = "";
                defaultItem.Value = "";
                sList.Add(defaultItem);

                foreach (var company in companies)
                {
                    item = new SelectListItem();
                    item.Text = company.Name;
                    item.Value = company.ID;
                    sList.Add(item);
                }
                return sList;
            //}
        }


        //public IEnumerable<SelectListItem> GetOwnerDropdown()
        //{
        //    // GET COMPANIES LIST
        //    SelectListItem item;
        //    List<SelectListItem> sList = new List<SelectListItem>();

        //    using (var db = new ApplicationDbContext())
        //    {

        //        var owners = db.Users.Where(u => u.IsActive == true && (u.Department == "Underwriting" || u.Department == "Business Development")).OrderBy(l => l.FirstName);

        //        var defaultItem = new SelectListItem();
        //        defaultItem.Text = "";
        //        defaultItem.Value = "";
        //        sList.Add(defaultItem);

        //        foreach (var owner in owners)
        //        {
        //            item = new SelectListItem();
        //            item.Text = $"{owner.FirstName} {owner.LastName}";
        //            item.Value = owner.Id;
        //            sList.Add(item);
        //        }
        //        return sList;
        //    }
        //}


        //public IEnumerable<SelectListItem> GetClientStages()
        //{
        //    // GET CLIENT STAGES LIST
        //    SelectListItem item;
        //    List<SelectListItem> sList = new List<SelectListItem>();

        //    using (var db = new ApplicationDbContext())
        //    {

        //        var clientStages = db.ClientStages.OrderBy(c => c.Name).ToList();

        //        var defaultItem = new SelectListItem();
        //        defaultItem.Text = "";
        //        defaultItem.Value = "";
        //        sList.Add(defaultItem);

        //        foreach (var clientStage in clientStages)
        //        {
        //            item = new SelectListItem();
        //            item.Text = $"{clientStage.Name}";
        //            item.Value = $"{clientStage.Name}";
        //            sList.Add(item);
        //        }
        //        return sList;
        //    }
        //}


        //public IEnumerable<SelectListItem> GetContactListByCompany(string companyID)
        //{
        //    // GET CONTACTS LIST BY COMPANY
        //    SelectListItem item;
        //    List<SelectListItem> sList = new List<SelectListItem>();

        //    using (var db = new ApplicationDbContext())
        //    {

        //        var contacts = db.Contacts.Where(c => c.AccountId == companyID).ToList();

        //        var defaultItem = new SelectListItem();
        //        defaultItem.Text = "";
        //        defaultItem.Value = "";
        //        sList.Add(defaultItem);

        //        foreach (var contact in contacts)
        //        {
        //            item = new SelectListItem();
        //            item.Text = $"{contact.FirstName} {contact.LastName}";
        //            item.Value = $"{contact.Id}";
        //            sList.Add(item);
        //        }
        //        return sList;
        //    }
        //}


        //public IEnumerable<SelectListItem> GetContactEmailListByClient(string clientID)
        //{

        //    var companyID = new ClientHelper().GetCompanyIDFromClientID(clientID);

        //    // GET CONTACTS LIST BY CLIENT
        //    SelectListItem item;
        //    List<SelectListItem> sList = new List<SelectListItem>();

        //    using (var db = new ApplicationDbContext())
        //    {
        //        var contacts = db.Contacts.Where(c => c.AccountId == companyID).ToList();

        //        //var defaultItem = new SelectListItem();
        //        //defaultItem.Text = "";
        //        //defaultItem.Value = "";
        //        //sList.Add(defaultItem);

        //        foreach (var contact in contacts)
        //        {
        //            item = new SelectListItem();
        //            item.Text = $"{contact.FirstName} {contact.LastName}";
        //            item.Value = $"{contact.Id}";
        //            sList.Add(item);
        //        }
        //        return sList;
        //    }
        //}


        //public IEnumerable<SelectListItem> GetBDOList()
        //{
        //    // GET CONTACTS LIST BY COMPANY
        //    SelectListItem item;
        //    List<SelectListItem> sList = new List<SelectListItem>();

        //    using (var db = new ApplicationDbContext())
        //    {

        //        var users = db.Users.Where(c => c.Department == "BDO" && c.IsActive == true).OrderBy(c => c.FirstName);

        //        var defaultItem = new SelectListItem();
        //        defaultItem.Text = "";
        //        defaultItem.Value = "";
        //        sList.Add(defaultItem);

        //        foreach (var user in users)
        //        {
        //            item = new SelectListItem();
        //            item.Text = $"{user.FirstName} {user.LastName}";
        //            item.Value = $"{user.Id}";
        //            sList.Add(item);
        //        }
        //        return sList;
        //    }
        //}


        public IEnumerable<SelectListItem> GetStateList()
        {
            // GET CONTACTS LIST BY COMPANY
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

            //using (var db = new ApplicationDbContext())
            //{

                var states = _context.LOV_State.OrderBy(s => s.Abbreviation);

                var defaultItem = new SelectListItem();
                defaultItem.Text = "";
                defaultItem.Value = "";
                sList.Add(defaultItem);

                foreach (var state in states)
                {
                    item = new SelectListItem();
                    item.Text = $"{state.Name}";
                    item.Value = $"{state.Abbreviation}";
                    sList.Add(item);
                }
                return sList;
            //}
        }


        public IEnumerable<SelectListItem> GetTaskTypes()
        {
            // GET TASK TYPES
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

                var tasks = _context.LOV_TaskType.OrderBy(t => t.TaskType);

                var defaultItem = new SelectListItem();
                defaultItem.Text = "";
                defaultItem.Value = "";
                sList.Add(defaultItem);

                foreach (var task in tasks)
                {
                    item = new SelectListItem();
                    item.Text = $"{task.TaskType}";
                    item.Value = $"{task.TaskType}";
                    sList.Add(item);
                }
                return sList;
        }


        public IEnumerable<SelectListItem> GetActiveUsers()
        {
            // GET CONTACTS LIST BY COMPANY
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

                var users = _context.Users.Where(c => c.IsActive == true).OrderBy(c => c.FirstName);

                var defaultItem = new SelectListItem();
                defaultItem.Text = "";
                defaultItem.Value = "";
                sList.Add(defaultItem);

                foreach (var user in users)
                {
                    item = new SelectListItem();
                    item.Text = $"{user.FirstName} {user.LastName}";
                    item.Value = $"{user.Id}";
                    sList.Add(item);
                }
                return sList;
            
        }


        //public IEnumerable<SelectListItem> GetActiveUsersEmailList()
        //{
        //    // GET CONTACTS LIST BY COMPANY
        //    SelectListItem item;
        //    List<SelectListItem> sList = new List<SelectListItem>();

        //    using (var db = new ApplicationDbContext())
        //    {

        //        var users = db.Users.Where(c => c.IsActive == true).OrderBy(c => c.FirstName);


        //        foreach (var user in users)
        //        {
        //            item = new SelectListItem();
        //            item.Text = $"{user.FirstName} {user.LastName}";
        //            item.Value = $"{user.Email}";
        //            sList.Add(item);
        //        }
        //        return sList;
        //    }
        //}


        //public IEnumerable<SelectListItem> GetReferralTypes()
        //{
        //    // GET CONTACTS LIST BY COMPANY
        //    SelectListItem item;
        //    List<SelectListItem> sList = new List<SelectListItem>();

        //    using (var db = new ApplicationDbContext())
        //    {

        //        var referralTypes = db.LOV_ReferralTypes.OrderBy(c => c.ReferralType);

        //        var defaultItem = new SelectListItem();
        //        defaultItem.Text = "";
        //        defaultItem.Value = "";
        //        sList.Add(defaultItem);

        //        foreach (var referral in referralTypes)
        //        {
        //            item = new SelectListItem();
        //            item.Text = $"{referral.ReferralType}";
        //            item.Value = $"{referral.ReferralType}";
        //            sList.Add(item);
        //        }
        //        return sList;
        //    }
        //}


        //public IEnumerable<SelectListItem> GetClientStatuses()
        //{
        //    // GET CONTACTS LIST BY COMPANY
        //    SelectListItem item;
        //    List<SelectListItem> sList = new List<SelectListItem>();

        //    using (var db = new ApplicationDbContext())
        //    {

        //        var clientStatuses = db.LOV_ClientStatuses.OrderBy(c => c.ClientStatus);

        //        var defaultItem = new SelectListItem();
        //        defaultItem.Text = "";
        //        defaultItem.Value = "";
        //        sList.Add(defaultItem);

        //        foreach (var clientStatus in clientStatuses)
        //        {
        //            item = new SelectListItem();
        //            item.Text = $"{clientStatus.ClientStatus}";
        //            item.Value = $"{clientStatus.ClientStatus}";
        //            sList.Add(item);
        //        }
        //        return sList;
        //    }
        //}


        public IEnumerable<SelectListItem> GetSICCodesList()
        {
            // GET SIC CODES
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

            using (_context)
            {

                var sicCodes = _context.SICCodes.OrderBy(s => s.Value).ToList();

                var defaultItem = new SelectListItem();
                defaultItem.Text = "";
                defaultItem.Value = "";
                sList.Add(defaultItem);

                foreach (var sicCode in sicCodes)
                {
                    item = new SelectListItem();
                    item.Text = $"{sicCode.ID} - {sicCode.Value}";
                    item.Value = $"{sicCode.Value}";
                    sList.Add(item);
                }
                return sList;
            }
        }


        public IEnumerable<SelectListItem> GetYesNoList()
        {
            // GET CONTACTS LIST BY COMPANY
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

            var defaultItem = new SelectListItem();
            defaultItem.Text = "";
            defaultItem.Value = "";
            sList.Add(defaultItem);

            var yesItem = new SelectListItem();
            yesItem.Text = "Yes";
            yesItem.Value = "true";
            sList.Add(yesItem);

            var noItem = new SelectListItem();
            noItem.Text = "No";
            noItem.Value = "false";
            sList.Add(noItem);

            return sList;
        }


        //public IEnumerable<SelectListItem> GetApprovalDecisionList()
        //{
        //    // GET CONTACTS LIST BY COMPANY
        //    SelectListItem item;
        //    List<SelectListItem> sList = new List<SelectListItem>();

        //    var defaultItem = new SelectListItem();
        //    defaultItem.Text = "";
        //    defaultItem.Value = "";
        //    sList.Add(defaultItem);

        //    var yesItem = new SelectListItem();
        //    yesItem.Text = "APPROVED";
        //    yesItem.Value = "APPROVED";
        //    sList.Add(yesItem);

        //    var noItem = new SelectListItem();
        //    noItem.Text = "DECLINED";
        //    noItem.Value = "DECLINED";
        //    sList.Add(noItem);

        //    return sList;
        //}


        //public IEnumerable<SelectListItem> GetMailMergeTemplateList()
        //{
        //    // GET CONTACTS LIST BY COMPANY
        //    SelectListItem item;
        //    List<SelectListItem> sList = new List<SelectListItem>();

        //    using (var db = new ApplicationDbContext())
        //    {

        //        var mailMergeTemplates = db.LOV_TemplateTypes.OrderBy(t => t.TemplateName).ToList();

        //        var defaultItem = new SelectListItem();
        //        defaultItem.Text = "";
        //        defaultItem.Value = "";
        //        sList.Add(defaultItem);

        //        foreach (var mailMergeTemplate in mailMergeTemplates)
        //        {
        //            item = new SelectListItem();
        //            item.Text = $"{mailMergeTemplate.TemplateName}";
        //            item.Value = $"{mailMergeTemplate.TemplateName}";
        //            sList.Add(item);
        //        }
        //        return sList;
        //    }
        //}



        //public IEnumerable<SelectListItem> GetRelationshipStatusList(string selectedOption)
        //{
        //    // GET LIST OF POTENTIAL RELATIONSHIP STATUSES
        //    SelectListItem item;
        //    List<SelectListItem> sList = new List<SelectListItem>();

        //    using (var db = new ApplicationDbContext())
        //    {

        //        var relationshipStatues = db.LOV_RelationshipStatuses.OrderBy(t => t.Status).ToList();

        //        var defaultItem = new SelectListItem();
        //        defaultItem.Text = "";
        //        defaultItem.Value = "";
        //        sList.Add(defaultItem);

        //        foreach (var relationshipStatus in relationshipStatues)
        //        {
        //            item = new SelectListItem();
        //            item.Text = $"{relationshipStatus.Status}";
        //            item.Value = $"{relationshipStatus.Status}";
        //            if (item.Value == selectedOption)
        //            {
        //                item.Selected = true;
        //            }
        //            sList.Add(item);
        //        }
        //        return sList;
        //    }
        //}



        //public IEnumerable<SelectListItem> GetTagGradeSortList()
        //{
        //    // GET LIST OF TAG/GRADE/SORT STATUSES
        //    SelectListItem item;
        //    List<SelectListItem> sList = new List<SelectListItem>();

        //    using (var db = new ApplicationDbContext())
        //    {

        //        var tagGradeSorts = db.LOV_TagGradeSorts.OrderBy(t => t.Grade).ToList();

        //        var defaultItem = new SelectListItem();
        //        defaultItem.Text = "";
        //        defaultItem.Value = "";
        //        sList.Add(defaultItem);

        //        foreach (var tagGradeSort in tagGradeSorts)
        //        {
        //            item = new SelectListItem();
        //            item.Text = $"{tagGradeSort.Grade} - {tagGradeSort.Description}";
        //            item.Value = $"{tagGradeSort.Grade}";
        //            sList.Add(item);
        //        }
        //        return sList;
        //    }
        //}



        //public IEnumerable<SelectListItem> GetUpdateNeededList()
        //{
        //    // GET LIST OF UPDATE NEEDED ITEMS
        //    SelectListItem item;
        //    List<SelectListItem> sList = new List<SelectListItem>();

        //    using (var db = new ApplicationDbContext())
        //    {

        //        var updatesNeeded = db.LOV_UpdateNeededs.OrderBy(t => t.UpdateField).ToList();

        //        var defaultItem = new SelectListItem();
        //        defaultItem.Text = "";
        //        defaultItem.Value = "";
        //        sList.Add(defaultItem);

        //        foreach (var updateNeeded in updatesNeeded)
        //        {
        //            item = new SelectListItem();
        //            item.Text = $"{updateNeeded.UpdateField}";
        //            item.Value = $"{updateNeeded.UpdateField}";
        //            sList.Add(item);
        //        }
        //        return sList;
        //    }
        //}



        //public IEnumerable<SelectListItem> GetMailingLists()
        //{
        //    // GET LIST OF MAILING LISTS
        //    SelectListItem item;
        //    List<SelectListItem> sList = new List<SelectListItem>();

        //    using (var db = new ApplicationDbContext())
        //    {

        //        var mailingLists = db.LOV_MailingLists.OrderBy(t => t.Name).ToList();

        //        var defaultItem = new SelectListItem();
        //        defaultItem.Text = "";
        //        defaultItem.Value = "";
        //        sList.Add(defaultItem);

        //        foreach (var mailingList in mailingLists)
        //        {
        //            item = new SelectListItem();
        //            item.Text = $"{mailingList.Name} - {mailingList.Description}";
        //            item.Value = $"{mailingList.Name}";
        //            sList.Add(item);
        //        }
        //        return sList;
        //    }
        //}



    }
}
