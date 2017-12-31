using System;
using System.Globalization;

namespace mvvmframework
{
    public static class DateTimeExtensions
    {
        public static string ToString(this DateTime dateTime, string format, bool useExtendedSpecifiers)
        {
            return dateTime.ToString(format)
                .Replace("nn", dateTime.Day.ToOccurrenceSuffix().ToLower())
                .Replace("NN", dateTime.Day.ToOccurrenceSuffix().ToUpper());
        }

        internal static string ToAppyDatetime(this DateTime date, string currentLangCode, CultureInfo currentCultureInfo)
        {
            string ordinal;

            if (currentLangCode.ToLower() == "fr")
            {
                ordinal = "";
            }
            else
            {
                switch (date.Day)
                {
                    case 1:
                    case 21:
                    case 31:
                        ordinal = "st";
                        break;
                    case 2:
                    case 22:
                        ordinal = "nd";
                        break;
                    case 3:
                    case 23:
                        ordinal = "rd";
                        break;
                    default:
                        ordinal = "th";
                        break;
                }
            }

            return string.Format("{1:d}{2} {0}", date.ToLocalizedString("MMMM yyyy", currentCultureInfo), date.Day, ordinal);
        }

        public static string ToLocalizedString(this DateTime date, string format, CultureInfo currentCultureInfo)
        {
            var ci = currentCultureInfo;
            return date.ToString(format, ci);
        }

        internal static string ToAppyDatetimeShortMonth(this DateTime date, CultureInfo currentCultureInfo, string currentLangCode)
        {
            var ordinal = string.Empty;

            if (currentLangCode.ToLower() == "fr")
            {
                ordinal = "";
            }
            else
            {
                switch (date.Day)
                {
                    case 1:
                    case 21:
                    case 31:
                        ordinal = "st";
                        break;
                    case 2:
                    case 22:
                        ordinal = "nd";
                        break;
                    case 3:
                    case 23:
                        ordinal = "rd";
                        break;
                    default:
                        ordinal = "th";
                        break;
                }
            }

            return string.Format("{1:d}{2} {0}", date.ToLocalizedString("MMM yyyy", currentCultureInfo), date.Day, ordinal);
        }
    }
}
