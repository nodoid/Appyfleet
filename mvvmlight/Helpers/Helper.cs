using System.Collections.Generic;

namespace mvvmframework
{
    public class Helper
    {
        public static float RemapRange(float inFromLow, float inFromHigh, float inValue, float inToLow, float inToHigh)
        {
            return inToLow + ((inValue - inFromLow) * (inToHigh - inToLow) / (inFromHigh - inFromLow));
        }

        public static string ISO3LanguageToAppyLanguage(string iso3Language)
        {
            switch(iso3Language.ToLowerInvariant())
            {
                case "eng":
                    return "en";
                case "fra":
                    return "fr";                   
                default:
                    return "en";
            }
        }
    }
}
