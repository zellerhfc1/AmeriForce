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
