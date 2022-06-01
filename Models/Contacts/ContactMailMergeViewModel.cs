using AmeriForce.Data;

namespace AmeriForce.Models.Contacts
{
    public class ContactMailMergeViewModel
    {
        public Contact contact { get; set; }
        public Company company { get; set; }
        public ApplicationUser owner { get; set; }
        public string TemplateFileName { get; set; }
    }
}
