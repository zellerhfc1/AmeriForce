namespace AmeriForce.Models.Contacts
{
    public class ContactToBeMergedViewModel
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RelationshipStatus { get; set; }
        public string RatingSort { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }

        /// <summary>
        /// First Contact ID that will not be merged
        /// </summary>
        public string NonMasterIDA { get; set; }

        /// <summary>
        /// Second Contact ID that will not be merged
        /// </summary>
        public string NonMasterIDB { get; set; }
    }
}
