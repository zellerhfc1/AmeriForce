using System;

namespace AmeriForce.Models.Clients.Reports
{
    public class CurrentPipelineViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int? Amount { get; set; }
        public string Stage { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public string OwnerName { get; set; }
        public string Description { get; set; }
        public string DaysSinceSubmission { get; set; }
        public string DaysSinceTSIssued { get; set; }
        public string DaysSinceTSReceived { get; set; }

        public string LeadSource { get; set; }
        public string FiscalPeriod { get; set; }
        public string Age { get; set; }
        public string CompanyName { get; set; }
    }
}
