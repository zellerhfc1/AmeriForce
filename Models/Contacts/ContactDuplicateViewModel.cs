namespace AmeriForce.Models.Contacts
{
    public class ContactDuplicateViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string CRMID { get; set; }
        public int DuplicateScore { get; set; }

        public ContactDuplicateViewModel()
        {
            DuplicateScore = 0;
        }
    }
}
