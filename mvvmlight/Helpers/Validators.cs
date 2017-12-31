using System;
using System.Text.RegularExpressions;

namespace mvvmframework
{
    public static class Validators
    {
        public static bool IsValidPhoneNumber(this string number) => number.Length == 11;

        public static bool IsValidEmailAddress(this string address) => ValidEmailRegex.IsMatch(address);

        public static bool IsStringValidOdoReading(this string input)
        {
            double ret = -1;
            double.TryParse(input, out ret);
            return IsValidOdoReading(ret);
        }

        public static bool IsValidOdoReading(this double input) => input > 0 && input <= 9999999;

        public static bool IsStringValidDOB(this string input) => Int32.TryParse(input, out int i);

        public static bool IsStringValidPassword(this string input) => input.Length >= 8;

        static Regex ValidEmailRegex = CreateValidEmailRegex;

        static Regex CreateValidEmailRegex
        {
            get
            {
                var validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                    + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                    + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

                return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
            }
        }
    }
}
