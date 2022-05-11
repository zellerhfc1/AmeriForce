using AmeriForce.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AmeriForce.Models.Clients
{
    public class ClientEditViewModel
    {
        public Client clientData { get; set; }

        public Contact referringContactData { get; set; }

        public Company referringCompanyData { get; set; }

        public CRMTask clientNextTask { get; set; }

        public IEnumerable<Contact> mainContactData { get; set; }

        public IEnumerable<Facility> clientFacilities { get; set; }

        public IEnumerable<CRMTask> clientNotes { get; set; }

        public Company companyData { get; set; }

        //LOVs
        public IEnumerable<SelectListItem> CompanyList { get; set; }
        public IEnumerable<SelectListItem> TaskList { get; set; }
        public IEnumerable<SelectListItem> ActiveUserList { get; set; }
        public IEnumerable<SelectListItem> TaskListNotes { get; set; }
        public IEnumerable<SelectListItem> ActiveUserListNotes { get; set; }
        public IEnumerable<SelectListItem> YesNoList { get; set; }
        public IEnumerable<SelectListItem> BDOList { get; set; }
        public IEnumerable<SelectListItem> ClientStageList { get; set; }
        public IEnumerable<SelectListItem> StatesList { get; set; }
        public IEnumerable<SelectListItem> ReferralTypes { get; set; }
        public IEnumerable<SelectListItem> ApprovalTypes { get; set; }
    }
}
