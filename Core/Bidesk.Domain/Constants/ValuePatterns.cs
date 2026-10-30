using System.Text.RegularExpressions;

namespace Bidesk.Domain.Constants
{
    public static class ValuePatterns
    {
        public static readonly Dictionary<string, Regex> Dictionary = new Dictionary<string, Regex>
        {
            {"Quote", new Regex("^\"([^\"]*)\"$|^'([^']*)'$") },
            {"AsteriskSign", new Regex("^\\*\\w+\\*$|^\\*\\w+|\\w+\\*$") },
            {"CommaNotInQuotes", new Regex(@",(?=(?:[^""]*""[^""]*"")*[^""]*$)") },
            {"SpaceNotInQuotes", new Regex(@"\s(?=(?:[^""]*""[^""]*"")*[^""]*$)") },

            {"Date", new Regex(@"\b(?:\d{4}[-/.]\d{2}[-/.]\d{2}|\d{2}[-/.]\d{2}[-/.]\d{4})\b") },
            {"DateTime", new Regex(@"\b(?:\d{4}[-/.]\d{2}[-/.]\d{2}[T ]\d{2}:\d{2}:\d{2}|\d{2}[-/.]\d{2}[-/.]\d{4} \d{2}:\d{2}:\d{2})\b") },

            //{"Integer", new Regex(@"^-?\d+$") },
            //{"Decimal", new Regex(@"^-?\d+(\.\d+)?$") },

            {"Numeric", new Regex(@"^(-?)(0|([1-9][0-9]*))(\\.[0-9]+)?$") },

            {"BegSign", new Regex(@"^(>=|<=|>|<|==|!=|=).*?(?<!>=|<=|>|<|==|!=|=)$") },
            {"EndSign", new Regex(@"^(?!>=|<=|>|<|==|!=|=).*?(>=|<=|>|<|==|!=|=)$") },

            {"Identification", new Regex(@"^[1-9][0-9]{10}$") },
            {"EmailAddress", new Regex(@"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,7}\b") },

            {"PhoneNumber", new Regex(@"^(?:\+90.?5|0090.?5|905|0?5)(?:[01345][0-9])\s?(?:[0-9]{3})\s?(?:[0-9]{2})\s?(?:[0-9]{2})$") },
            {"HomePhone", new Regex(@"^(0)([2348]{1})([0-9]{2})\s?([0-9]{3})\s?([0-9]{2})\s?([0-9]{2})$") },

            {"VisaCard", new Regex(@"\b4\d{12}(?:\d{3})?\b") },
            {"MasterCard", new Regex(@"\b5[1-5]\d{14}\b|\b2(2[2-9][1-9]|[3-6]\d{2}|7[01]\d|720)\d{12}\b") },
            {"AmericanExpress", new Regex(@"\b3[47]\d{13}\b") },

            {"TaxNumber", new Regex(@"^(0[1-9]|[1-9][0-9])\d{10}$") },
            {"LicensePlate", new Regex(@"\b(0[1-9]|[1-7][0-9]|8[01])\s?[A-Z]{1,3}\s?\d{2,4}\b") },

        };
    }
}
