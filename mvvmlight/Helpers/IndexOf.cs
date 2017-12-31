using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace mvvmframework.Helpers
{
    public static class IndexOf
    {
        public static List<int> IndexOfAll(this string sourceString, string subString)
        {
            return Regex.Matches(sourceString, subString).Cast<Match>().Select(m => m.Index).ToList();
        }
    }
}
