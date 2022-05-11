using AmeriForce.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AmeriForce.Models.Contacts
{
    public class ContactEditViewModel
    {
        public Contact contactData { get; set; }
        public Company companyData { get; set; }
        public IEnumerable<Client> clients { get; set; }
        public IEnumerable<CRMTask> contactNotes { get; set; }
        public CRMTask taskData { get; set; }

        public string ReferralCompanyName { get; set; }


        public List<SelectListItem> MailingListIntermediate { get; set; }
        public List<string> UpdateNeededIntermediate { get; set; }


        //public IEnumerable<Contact> mainContactData { get; set; }

        //public Contact referringContactData { get; set; }

        //public Company referringCompanyData { get; set; }

        //public IEnumerable<Facility> clientFacilities { get; set; }



        //LOVs
        public IEnumerable<SelectListItem> CompanyList { get; set; }
        public IEnumerable<SelectListItem> TaskList { get; set; }
        public IEnumerable<SelectListItem> ActiveUserList { get; set; }
        public IEnumerable<SelectListItem> TaskListNotes { get; set; }
        public IEnumerable<SelectListItem> ActiveUserListNotes { get; set; }
        public IEnumerable<SelectListItem> YesNoList { get; set; }

        public IEnumerable<SelectListItem> BDOList { get; set; }
        public IEnumerable<SelectListItem> RelationshipStatusList { get; set; }
        public IEnumerable<SelectListItem> TagGradeSortList { get; set; }
        public IEnumerable<SelectListItem> UpdateNeededList { get; set; }
        public IEnumerable<SelectListItem> MailingLists { get; set; }
        public IEnumerable<SelectListItem> StateList { get; set; }

    }
}
