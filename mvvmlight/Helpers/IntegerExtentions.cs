using System;
namespace mvvmframework
{
    public static class IntegerExtensions
    {
        public static string ToOccurrenceSuffix(this int integer)
        {
            switch (integer % 100)
            {
                case 11:
                case 12:
                case 13:
                    return "th";
            }
            switch (integer % 10)
            {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }
        }
    }
}
