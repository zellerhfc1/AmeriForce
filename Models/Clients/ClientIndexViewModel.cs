using System;

namespace AmeriForce.Models.Clients
{
    public class ClientIndexViewModel
    {
        public string ID { get; set; }
        public string OwnerName { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string StageName { get; set; }
        public int? Amount { get; set; }
        public DateTime? CloseDate { get; set; }
    }
}
