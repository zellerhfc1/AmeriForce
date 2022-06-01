using System;

namespace AmeriForce.Helpers
{
    internal class GuidHelper
    {
        internal string GetGUIDString(string type)
        {
            string prefix = GetGuidPrefix(type);
            Guid g = Guid.NewGuid();
            string guidResult = g.ToString();
            guidResult = $"{prefix}{guidResult.Substring(guidResult.Length - 15)}";
            return guidResult;
        }

        internal string GetGuidType(string entityType)
        {
            switch (entityType)
            {
                case "001":
                    return "companies";
                case "003":
                    return "contacts";
                case "006":
                    return "clients";
                case "00T":
                    return "tasks";
                case "02s":
                    return "emails";
                case "TE0":
                    return "tempemail";
                default:
                    return "---";
            }

        }

        private string GetGuidPrefix(string entityType)
        {
            switch (entityType)
            {
                case "company":
                    return "001";
                case "contact":
                    return "003";
                case "client":
                    return "006";
                case "task":
                    return "00T";
                case "email":
                    return "02s";
                case "tempemail":
                    return "TE0";
                default:
                    return "---";
            }

        }
    }
}
