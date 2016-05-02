using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace KottansLvivTask
{
    static class CreditCardAnalizer
    {
        private static readonly Dictionary<string, string> vendors;
        private static readonly List<string> formats;

        static CreditCardAnalizer()
        {
            formats = new List<string>()
            {
                @"\d{12,19}$",
                @"^\d{4}\s\d{4}\s\d{4}$",
                @"^\d{4}\s\d{5}\s\d{4}$",
                @"^\d{4}\s\d{6}\s\d{5}$",
                @"^\d{4}(?:\s\d{4}){3}$"
            };

            vendors = new Dictionary<string, string>
            {
                { @"^3[4,7]\d{13}", "American Express"},
                { @"^5[1-5]\d{14}$", "MasterCard"},
                { @"^35(?:2[8,9]|[3-8]\d)\d{12}$", "JCB"},
                { @"^4\d{12}(?:\d{3}|\d{6})?$", "VISA"},
                { @"^(?:5[0,6-9]|6\d)\d{10,17}$", "Maestro"},
            };
        }

        private static bool IsFormatCorrect(string creditCardNumber)
        {
            foreach (var format in formats)
            {
                if (Regex.IsMatch(creditCardNumber, format))
                {
                    return true;
                }
            }
            return false;
        }
        private static string RemoveAllWhitespaces(string creditCardNumber)
        {
            return creditCardNumber.Replace(" ", string.Empty);
        }

        public static string GetCreditCardVendor(string creditCardNumber)
        {
            if (!IsFormatCorrect(creditCardNumber))
            {
                throw new FormatException("Credit card number has an incorrect format!");
            }
            creditCardNumber = RemoveAllWhitespaces(creditCardNumber);

            foreach (var vendor in vendors)
            {
                if (Regex.IsMatch(creditCardNumber, vendor.Key))
                {
                    return vendor.Value;
                }
            }

            return "Unknown";
        }

        public static bool IsCreditCardNumberValid(string creditCardNumber)
        {
            int sum = 0;

            if (!IsFormatCorrect(creditCardNumber))
            {
                throw new FormatException("Credit card number has an incorrect format!");
            }
            creditCardNumber = RemoveAllWhitespaces(creditCardNumber);

            try
            {
                sum = creditCardNumber.Reverse().Select((digitCharacter, digitPosition) =>
                {
                    int digit = int.Parse(digitCharacter.ToString());
                    if (digitPosition % 2 == 1)
                    {
                        digit *= 2;
                        if (digit > 9)
                        {
                            digit -= 9;
                        }
                    }
                    return digit;
                }).Sum();
            }
            catch
            {
                return false;
            }
            if(sum != 0)
            {
                return sum % 10 == 0;
            }
            return false;
        }

        public static string GenerateNextCreditCardNumber(string sourceCardNumber)
        {
            if (!IsFormatCorrect(sourceCardNumber))
            {
                throw new FormatException("Credit card number has an incorrect format!");
            }

            sourceCardNumber = RemoveAllWhitespaces(sourceCardNumber);

            if(!IsCreditCardNumberValid(sourceCardNumber))
            {
                throw new ArgumentException("Input credit card number is not valid!");
            }

            ulong resultCardNumber = ulong.Parse(sourceCardNumber);


            for (int i = 0; i < 30; i++)
            {
                if (IsCreditCardNumberValid((++resultCardNumber).ToString()))
                    return resultCardNumber.ToString();
            }
            throw new Exception("Next card number not found!");
        }
    }
}
