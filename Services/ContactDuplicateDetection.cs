using AmeriForce.Data;
using AmeriForce.Models.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmeriForce.Services
{
    public class ContactDuplicateDetection
    {
        public List<ContactDuplicateViewModel> DuplicateList { get; set; }
        public List<ContactDuplicateViewModel> DuplicateListFinal { get; set; }
        public string DuplicateReasons { get; set; }
        private readonly ApplicationDbContext _context;

        public ContactDuplicateDetection(ApplicationDbContext context)
        {
            _context = context;
            this.DuplicateList = new List<ContactDuplicateViewModel>();
            this.DuplicateListFinal = new List<ContactDuplicateViewModel>();
        }

        public List<ContactDuplicateViewModel> DetectDuplicateBasedOnContact(ContactDuplicateViewModel contact)
        {
            try
            {
                // Contact Name and Title Checks
                GetExactMatches(contact.FirstName, contact.LastName);
                GetExactMatches(contact.Title);
                GetDistance(contact.FirstName, contact.LastName);
                GetExactMatches(contact.Address);
                GetDistance(contact.Address);


                var sumDuplicates = DuplicateList.GroupBy(d => d.CRMID).Select(d => new { crmid = d.Key, sumscore = d.Sum(c => c.DuplicateScore) });
                foreach (var sumDuplicate in sumDuplicates)
                {
                    if (sumDuplicate.sumscore < 75)
                    {
                        DuplicateList.RemoveAll(d => d.CRMID == sumDuplicate.crmid);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            GetActualDuplicates(contact.CRMID);
            return DuplicateListFinal;
        }

        private void AddDuplicateContact(ContactDuplicateViewModel contact)
        {

            DuplicateList.Add(contact);

        }

        private void GetActualDuplicates(string originalContactID)
        {
            DuplicateList.RemoveAll(d => d.CRMID == originalContactID);
            DuplicateListFinal = DuplicateList.GroupBy(d => d.CRMID).Select(c => c.First()).ToList();

        }

        /// <summary>
        /// Get exact matches for contact first and last names
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        private void GetExactMatches(string firstName, string lastName)
        {
            var duplicateContacts = _context.Contacts.Where(c => (c.FirstName == firstName && c.LastName == lastName) || (c.FirstName == lastName && c.LastName == firstName));
            if (duplicateContacts != null)
            {
                foreach (var duplicateContact in duplicateContacts)
                {
                    var duplicate = new ContactDuplicateViewModel()
                    {
                        FirstName = duplicateContact.FirstName,
                        LastName = duplicateContact.LastName,
                        Title = duplicateContact.Title,
                        CRMID = duplicateContact.Id,
                        DuplicateScore = 100
                    };
                    AddDuplicateContact(duplicate);
                    DuplicateReasons += $"{duplicateContact.FirstName} = Exact Match Name";
                }
            }
        }


        /// <summary>
        /// Get exact matches for contact title
        /// </summary>
        /// <param name="title"></param>
        private void GetExactMatches(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                string[] titleSplit = title.Split(' ');
                string titleAcronym = "";
                foreach (var word in titleSplit)
                {
                    titleAcronym += new string(word.Take(1).ToArray());
                }

                var duplicateContacts = _context.Contacts.Where(c => (c.Title == title) || (c.Title == titleAcronym));
                if (duplicateContacts != null)
                {
                    foreach (var duplicateContact in duplicateContacts)
                    {
                        var duplicate = new ContactDuplicateViewModel()
                        {
                            FirstName = duplicateContact.FirstName,
                            LastName = duplicateContact.LastName,
                            Title = duplicateContact.Title,
                            CRMID = duplicateContact.Id,
                            DuplicateScore = 25
                        };
                        AddDuplicateContact(duplicate);
                        DuplicateReasons += $"{duplicateContact.FirstName} = Title or Acronym";
                    }
                }
            }
            return;
        }



        /// <summary>
        /// Get Jaro Winkler Distance for contact first and last name
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        private void GetDistance(string firstName, string lastName)
        {
            string firstInitial = new string(firstName.Take(3).ToArray());
            string lastInitial = new string(lastName.Take(3).ToArray());

            var duplicateContacts = _context.Contacts.Where(c => (c.FirstName.StartsWith(firstInitial) && c.LastName.StartsWith(lastInitial)) || (c.FirstName.StartsWith(lastInitial) && c.LastName.StartsWith(firstInitial)));
            var count = duplicateContacts.Count();

            if (duplicateContacts != null)
            {
                foreach (var duplicateContact in duplicateContacts)
                {
                    if (JaroWinklerDistance.IsPotentialMatch(firstInitial, duplicateContact.FirstName) ||
                        JaroWinklerDistance.IsPotentialMatch(lastInitial, duplicateContact.LastName))
                    {
                        var duplicate = new ContactDuplicateViewModel()
                        {
                            FirstName = duplicateContact.FirstName,
                            LastName = duplicateContact.LastName,
                            Title = duplicateContact.Title,
                            CRMID = duplicateContact.Id,
                            DuplicateScore = 25
                        };
                        AddDuplicateContact(duplicate);
                        DuplicateReasons += $"{duplicateContact.FirstName} = Distance Name";
                    }
                }
            }
        }



        /// <summary>
        /// Get Jaro Winkler distance from address
        /// </summary>
        /// <param name="address"></param>
        private void GetDistance(string address)
        {
            if (!string.IsNullOrEmpty(address))
            {
                string[] address1 = address.Split(' ');
                string address1a = new string(address1[0].Take(3).ToArray());
                string address1b = new string(address1[1].Take(3).ToArray());

                var duplicateContacts = _context.Contacts.Where(c =>
                        (c.MailingStreet.StartsWith(address1a)) ||
                        (c.MailingStreet.Contains(address1b)));

                var count = duplicateContacts.Count();

                if (duplicateContacts != null)
                {
                    foreach (var duplicateContact in duplicateContacts)
                    {
                        if (JaroWinklerDistance.IsPotentialMatch(address1a, duplicateContact.MailingStreet) ||
                            JaroWinklerDistance.IsPotentialMatch(address1b, duplicateContact.LastName))
                        {
                            var duplicate = new ContactDuplicateViewModel()
                            {
                                FirstName = duplicateContact.FirstName,
                                LastName = duplicateContact.LastName,
                                Title = duplicateContact.Title,
                                CRMID = duplicateContact.Id,
                                DuplicateScore = 25
                            };
                            AddDuplicateContact(duplicate);
                            DuplicateReasons += $"{duplicateContact.FirstName} = Distance Address";
                        }
                    }
                }
            }
            return;
        }
    }



    public static class JaroWinklerDistance
    {
        /* The Winkler modification will not be applied unless the 
         * percent match was at or above the mWeightThreshold percent 
         * without the modification. 
         * Winkler's paper used a default value of 0.7
         */
        private static readonly double mWeightThreshold = 0.7;

        /* Size of the prefix to be concidered by the Winkler modification. 
         * Winkler's paper used a default value of 4
         */
        private static readonly int mNumChars = 4;



        public static bool IsPotentialMatch(string originalValue, string newValue)
        {
            var isProximity = proximity(originalValue, newValue);
            return (isProximity > .75) ? true : false;
        }


        /// <summary>
        /// Returns the Jaro-Winkler distance between the specified  
        /// strings. The distance is symmetric and will fall in the 
        /// range 0 (perfect match) to 1 (no match). 
        /// </summary>
        /// <param name="aString1">First String</param>
        /// <param name="aString2">Second String</param>
        /// <returns></returns>
        public static double distance(string aString1, string aString2)
        {
            return 1.0 - proximity(aString1, aString2);
        }


        /// <summary>
        /// Returns the Jaro-Winkler distance between the specified  
        /// strings. The distance is symmetric and will fall in the 
        /// range 0 (no match) to 1 (perfect match). 
        /// </summary>
        /// <param name="aString1">First String</param>
        /// <param name="aString2">Second String</param>
        /// <returns></returns>
        public static double proximity(string aString1, string aString2)
        {
            int lLen1 = aString1.Length;
            int lLen2 = aString2.Length;
            if (lLen1 == 0)
                return lLen2 == 0 ? 1.0 : 0.0;

            int lSearchRange = Math.Max(0, Math.Max(lLen1, lLen2) / 2 - 1);

            // default initialized to false
            bool[] lMatched1 = new bool[lLen1];
            bool[] lMatched2 = new bool[lLen2];

            int lNumCommon = 0;
            for (int i = 0; i < lLen1; ++i)
            {
                int lStart = Math.Max(0, i - lSearchRange);
                int lEnd = Math.Min(i + lSearchRange + 1, lLen2);
                for (int j = lStart; j < lEnd; ++j)
                {
                    if (lMatched2[j]) continue;
                    if (aString1[i] != aString2[j])
                        continue;
                    lMatched1[i] = true;
                    lMatched2[j] = true;
                    ++lNumCommon;
                    break;
                }
            }
            if (lNumCommon == 0) return 0.0;

            int lNumHalfTransposed = 0;
            int k = 0;
            for (int i = 0; i < lLen1; ++i)
            {
                if (!lMatched1[i]) continue;
                while (!lMatched2[k]) ++k;
                if (aString1[i] != aString2[k])
                    ++lNumHalfTransposed;
                ++k;
            }
            // System.Diagnostics.Debug.WriteLine("numHalfTransposed=" + numHalfTransposed);
            int lNumTransposed = lNumHalfTransposed / 2;

            // System.Diagnostics.Debug.WriteLine("numCommon=" + numCommon + " numTransposed=" + numTransposed);
            double lNumCommonD = lNumCommon;
            double lWeight = (lNumCommonD / lLen1
                             + lNumCommonD / lLen2
                             + (lNumCommon - lNumTransposed) / lNumCommonD) / 3.0;

            if (lWeight <= mWeightThreshold) return lWeight;
            int lMax = Math.Min(mNumChars, Math.Min(aString1.Length, aString2.Length));
            int lPos = 0;
            while (lPos < lMax && aString1[lPos] == aString2[lPos])
                ++lPos;
            if (lPos == 0) return lWeight;
            return lWeight + 0.1 * lPos * (1.0 - lWeight);

        }


    }
}
