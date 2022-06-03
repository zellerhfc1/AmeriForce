using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AmeriForce.Helpers
{
    public class ValidationHelper
    {
        /// <summary>
        /// Checks for null or empty string after trimming
        /// </summary>
        /// <param name="stringValue">Any string value</param>
        /// <returns>Trimmed string</returns>
        internal string ValidateNonRequiredString(string stringValue)
        {
            if (stringValue == null)
            {
                return "";
            }
            if (stringValue.Trim() != "" && stringValue != null)
            {
                return stringValue.Trim();
            }
            return "";
        }


        /// <summary>
        /// Checks for null or empty string after trimming
        /// </summary>
        /// <param name="stringValue">Any string value</param>
        /// <returns>Trimmed string</returns>
        internal string ValidateRequiredString(string stringValue)
        {
            if (stringValue == null)
            {
                return "";
            }
            if (stringValue.Trim() != "" && stringValue != null)
            {
                return stringValue.Trim();
            }
            return "NA";
        }

        /// <summary>
        /// Removes all non-alphanumeric characters from a string
        /// </summary>
        /// <param name="stringValue">Any string value</param>
        /// <returns>Scrubbed String</returns>
        public string ValidateRequiredStringWithNoSpecialCharacters(string stringValue)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            string stringScrubbed = rgx.Replace(stringValue, "");
            return stringScrubbed;
        }

        /// <summary>
        /// Removes all non-alpha characters from a string
        /// </summary>
        /// <param name="stringValue">Any string value</param>
        /// <returns>Scrubbed String</returns>
        public string ValidateRequiredStringWithNoSpecialCharactersOrNumbers(string stringValue)
        {
            Regex rgx = new Regex("[^a-zA-Z -]");
            string stringScrubbed = rgx.Replace(stringValue, "");
            return stringScrubbed;
        }

        /// <summary>
        /// Checks for valid Employee ID Number and format
        /// </summary>
        /// <param name="stringValue">EIN string</param>
        /// <returns></returns>
        internal string ValidateEIN(string stringValue)
        {
            if (stringValue.Trim() != "" && stringValue != null && stringValue.Contains("-") && stringValue.Length == 10)
            {
                return stringValue.Trim();
            }
            return "NA";
        }

        /// <summary>
        /// Checks for valid 2 character state abbreviation
        /// </summary>
        /// <param name="stringValue">State string</param>
        /// <returns></returns>
        internal string ValidateState(string stringValue)
        {
            if (stringValue.Trim() != "" && stringValue != null && stringValue.Length == 2 && char.IsLetter(stringValue, 0) == true && char.IsLetter(stringValue, 1) == true)
            {
                return stringValue.Trim();
            }
            return "NA";
        }

        /// <summary>
        /// Strips non-digits and checks for valid 10 digit phone number
        /// </summary>
        /// <param name="stringValue">Phone string</param>
        /// <returns></returns>
        internal string ValidatePhone(string stringValue)
        {
            Regex extractNumbers = new Regex(@"[^\d]");
            var numbersOnly = extractNumbers.Replace(stringValue, "");

            if (numbersOnly.Trim() != "" && numbersOnly != null && numbersOnly.Length == 10)
            {
                return stringValue.Trim();
            }

            if (stringValue == "")
            {
                return "";
            }
            return "Invalid Phone";
        }

        /// <summary>
        /// Checks for valid email
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        internal string ValidateEmail(string stringValue)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            Match match = regex.Match(stringValue);
            if (match.Success)
            {
                return stringValue.Trim();
            }
            else if (!match.Success)
            {
                if (stringValue == "")
                {
                    return "";
                }
            }
            return "Invalid Email";
        }

        /// <summary>
        /// Checks for valid 5 digit zip code
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        internal string Validate5DigitZipCode(string stringValue)
        {
            Regex regex = new Regex(@"^(?!0{5})(\d{5})(?!-?0{4})(-?\d{4})?$");
            Match match = regex.Match(stringValue);
            if (match.Success)
            {
                return stringValue.Trim();
            }
            return "NA";
        }

        /// <summary>
        /// Checks for valid decimal value
        /// </summary>
        /// <param name="decimalValue"></param>
        /// <returns></returns>
        internal decimal ValidateDecimal(string decimalValue)
        {
            decimal decimalValueOut;
            bool isDecimal = Decimal.TryParse(decimalValue, out decimalValueOut);
            if (isDecimal)
            {
                return decimalValueOut;
            }
            return 0;
        }

        /// <summary>
        /// Checks for valid decimal value and 
        /// assigns a value of zero to anything less than 1
        /// </summary>
        /// <param name="decimalValue"></param>
        /// <returns></returns>
        internal decimal ValidateDecimalNonNegative(string decimalValue)
        {
            decimal decimalValueOut;
            bool isDecimal = Decimal.TryParse(decimalValue, out decimalValueOut);
            if (isDecimal)
            {
                if (decimalValueOut < 1)
                {
                    return 0;
                }
                return decimalValueOut;
            }
            return 0;
        }

        /// <summary>
        /// Checks for valid double value
        /// </summary>
        /// <param name="doubleValue"></param>
        /// <returns></returns>
        internal double ValidateDouble(string doubleValue)
        {
            double doubleValueOut;
            doubleValue = doubleValue.Trim(new Char[] { ' ', '$' });
            bool isDouble = Double.TryParse(doubleValue, out doubleValueOut);
            if (isDouble)
            {
                return doubleValueOut;
            }
            return 0;
        }


        /// <summary>
        /// Checks for valid double value
        /// and assigns a value of 0 for anything less than 1
        /// </summary>
        /// <param name="doubleValue"></param>
        /// <returns></returns>
        internal double ValidateDoubleNonNegative(string doubleValue = "0")
        {
            double doubleValueOut;
            doubleValue = doubleValue.Trim(new Char[] { ' ', '$' });
            bool isDouble = Double.TryParse(doubleValue, out doubleValueOut);
            if (isDouble)
            {
                if (doubleValueOut < 1)
                {
                    return 0;
                }
                return doubleValueOut;
            }
            return 0;
        }

        /// <summary>
        /// Checks that percentage is an integer between 1 and 100
        /// </summary>
        /// <param name="percentage">integer value</param>
        /// <returns></returns>
        internal int ValidatePercentage(int percentage)
        {
            string strPercentage = percentage.ToString();
            int percentageValue;
            bool isPercentage = Int32.TryParse(strPercentage, out percentageValue);
            if (isPercentage)
            {
                if (percentageValue >= 1 && percentageValue <= 100)
                {
                    return percentageValue;
                }
            }
            return 0;
        }


        /// <summary>
        /// Checks that percentage is an decimal between 1 and 100
        /// </summary>
        /// <param name="percentage">decimal value</param>
        /// <returns></returns>
        internal decimal ValidatePercentageByDecimal(decimal percentage)
        {
            string strPercentage = percentage.ToString();
            decimal percentageValue;
            bool isPercentage = Decimal.TryParse(strPercentage, out percentageValue);
            if (isPercentage)
            {
                if (percentageValue > 0 && percentageValue <= 100)
                {
                    return percentageValue;
                }
            }
            return 0;
        }

        /// <summary>
        /// Checks that credit score is within valid range of 300-850
        /// </summary>
        /// <param name="percentage">integer value</param>
        /// <returns></returns>
        internal int ValidateCreditScore(int creditScore)
        {
            string creditScoreNew = creditScore.ToString();
            int creditScoreValue;
            bool isPercentage = Int32.TryParse(creditScoreNew, out creditScoreValue);
            if (isPercentage)
            {
                if (creditScoreValue >= 300 && creditScoreValue <= 850)
                {
                    return creditScoreValue;
                }
            }
            return 0;
        }

        /// <summary>
        /// Checks for a valid date time value
        /// </summary>
        /// <param name="incomingDate"></param>
        /// <returns></returns>
        internal DateTime ValidateDate(DateTime incomingDate)
        {
            string newDate = incomingDate.ToString();
            DateTime dateValue;
            bool isDate = DateTime.TryParse(newDate, out dateValue);
            if (isDate)
            {
                return dateValue;
            }
            return DateTime.Now;
        }

        /// <summary>
        /// Checks for a valid date time value
        /// </summary>
        /// <param name="incomingDate"></param>
        /// <returns></returns>
        internal DateTime ValidateDateWithString(string incomingDate)
        {
            string newDate = incomingDate.ToString();
            DateTime dateValue;
            bool isDate = DateTime.TryParse(newDate, out dateValue);
            if (isDate)
            {
                return dateValue;
            }
            return Convert.ToDateTime("01/01/1800");
        }


        /// <summary>
        /// Checks for a valid date time value
        /// </summary>
        /// <param name="incomingDate"></param>
        /// <returns></returns>
        internal DateTime? ValidateDateWithString_NotRequired(string incomingDate)
        {
            string newDate = incomingDate.ToString();
            DateTime dateValue;
            bool isDate = DateTime.TryParse(newDate, out dateValue);
            if (isDate)
            {
                return dateValue;
            }
            return null;
        }

        internal int ValidateInteger(string stringValue)
        {
            int intValue;
            stringValue = stringValue.Replace(",", "");
            bool isInteger = Int32.TryParse(stringValue, out intValue);
            if (isInteger)
            {
                return intValue;
            }
            return 0;
        }

        /// <summary>
        /// Checks for a valid BDOID 
        /// </summary>
        /// <param name="bdoID"></param>
        /// <returns></returns>
        internal int ValidateBDOID(string bdoID)
        {
            int bdoValue;
            bool isBDO = Int32.TryParse(bdoID, out bdoValue);
            if (isBDO)
            {
                return bdoValue;
            }
            return 0;
        }

        /// <summary>
        /// Rounds number to the nearest thousand
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        internal decimal NumberRoundToThousands(decimal num)
        {
            decimal result = 0;

            if (num < 1000)
            {
                result = num;
            }
            else if (num < 1000000)
            {
                result = RoundDown(num / 1000, 0);
            }
            else if (num < 100000000)
            {
                result = RoundDown(num / 1000, 6);
            }
            else if (num < 10000000000)
            {
                result = RoundDown(num / 1000, 8);
            }
            else if (num < 1000000000000)
            {
                result = RoundDown(num / 1000, 10);
            }

            return result;
        }

        private static decimal RoundDown(decimal value, Int32 digits)
        {
            var pow = Math.Pow(10, digits);
            var powConverted = Convert.ToDecimal(pow);
            return Math.Truncate(value * powConverted) / powConverted;
        }

        /// <summary>
        /// Checks for a valid SSN number (with formatting)
        /// </summary>
        /// <param name="incomingSSN"></param>
        /// <returns></returns>
        internal string ValidateSSN(string incomingSSN)
        {
            if (incomingSSN.Length == 11 && incomingSSN.Count(i => i == '-') == 2)
            {
                bool ssnFormat = SSNCharactersInPlace(incomingSSN);
                if (ssnFormat)
                {
                    return incomingSSN;
                }
            }
            return null;
        }

        public bool SSNCharactersInPlace(string incomingSSN)
        {
            int len = incomingSSN.Length;
            for (int i = 0; i < len; ++i)
            {
                char c = incomingSSN[i];
                if (i != 3 && i != 6)
                {
                    if (c < '0' || c > '9')
                    {
                        return false;
                    }
                }
                else
                {
                    if (c.ToString() != "-")
                    {
                        return false;
                    }
                }
            }
            return true;

        }

        public string ReplaceCommasWithPipes(string incomingString)
        {
            String returnString = "";
            returnString = incomingString.Replace(',', '|');
            return returnString;
        }

        public string SeparateStringsByCapitalLetter(string incomingString)
        {
            if (incomingString != null)
            {
                var outgoingStringInPieces = Regex.Split(incomingString, @"(?<!^)(?=[A-Z])");
                string outgoingString = "";

                foreach (var o in outgoingStringInPieces)
                {
                    outgoingString += String.Format("{0} ", o);
                }
                return outgoingString.Trim();
            }
            return "";
        }

        public DateTime GetCurrentCentralTime()
        {
            var timeUtc = DateTime.UtcNow;
            TimeZoneInfo centralZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            DateTime centralTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, centralZone);
            return centralTime;
        }

        public bool IsPerson18OrOlder(DateTime dateOfBirth)
        {
            int now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            int dob = int.Parse(dateOfBirth.ToString("yyyyMMdd"));
            int age = (now - dob) / 10000;

            if (age < 18)
            {
                return false;
            }
            return true;
        }

        public string IsUser(string userID)
        {
            if (userID.Substring(0, 3) == "005")
            {
                return userID;
            }
            return "";
        }
    }
}
