using System.Collections.Generic;

namespace AmeriForce.Models.Companies
{
    public class CompanyMergeContactsViewModel
    {
        public int ID { get; set; }
        public List<string> ContactIDList { get; set; }
        public List<string> FirstNameList { get; set; }
        public List<string> LastNameList { get; set; }
        public List<string> PhoneList { get; set; }
        public List<string> MobilePhoneList { get; set; }
        public List<string> EmailList { get; set; }
        public List<OwnerInfo> OwnerList { get; set; }
    }

    public class OwnerInfo
    {
        public string OwnerID { get; set; }
        public string OwnerName { get; set; }

    }
}
