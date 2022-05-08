using AmeriForce.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AmeriForce.Models.Contacts
{
    public class ContactDetailsViewModel
    {
        public Contact contact { get; set; }
        public string CompanyName { get; set; }
        public string OwnerName { get; set; }
        public string ReferringCompany { get; set; }
        public string ReferringContact { get; set; }

        public IEnumerable<Client> clients { get; set; }
        public IEnumerable<CRMTask> contactNotes { get; set; }
        public CRMTask contactNextTask { get; set; }
        public string contactNextTaskOwner { get; set; }

        public List<ContactDuplicateViewModel> contactDuplicates { get; set; }


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
        public IEnumerable<SelectListItem> MailMergeTemplateList { get; set; }
    }
}
