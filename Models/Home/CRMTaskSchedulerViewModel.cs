using System;

namespace AmeriForce.Models.Home
{
    public class CRMTaskSchedulerViewModel
    {
        public int ID { get; set; }
        public string contactID { get; set; }
        public string text { get; set; }
        public string taskType { get; set; }
        public string taskIcon { get; set; }
        public int employeeID { get; set; }
        public DateTime? startDate { get; set; }
        public string description { get; set; }
    }
}
