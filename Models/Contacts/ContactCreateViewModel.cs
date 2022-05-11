using AmeriForce.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AmeriForce.Models.Contacts
{
    public class ContactCreateViewModel
    {
        public Client clientData { get; set; }

        public Contact contactData { get; set; }

        public Company companyData { get; set; }

        public CRMTask taskData { get; set; }

        public List<string> MailingListIntermediate { get; set; }
        public List<string> UpdateNeededIntermediate { get; set; }

        //LOVs
        public IEnumerable<SelectListItem> BaseList { get; set; }
        public IEnumerable<SelectListItem> CompanyList { get; set; }
        public IEnumerable<SelectListItem> OwnerList { get; set; }
        public IEnumerable<SelectListItem> ClientStageList { get; set; }
        public IEnumerable<SelectListItem> BDOList { get; set; }
        public IEnumerable<SelectListItem> StateList { get; set; }
        public IEnumerable<SelectListItem> TaskList { get; set; }
        public IEnumerable<SelectListItem> ActiveUserList { get; set; }
        public IEnumerable<SelectListItem> ReferralTypeList { get; set; }
        public IEnumerable<SelectListItem> RelationshipStatusList { get; set; }
        public IEnumerable<SelectListItem> TagGradeSortList { get; set; }
        public IEnumerable<SelectListItem> UpdateNeededList { get; set; }
        public IEnumerable<SelectListItem> MailingLists { get; set; }


    }
}
