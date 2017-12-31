using System;
using System.Collections.Generic;

namespace mvvmframework.Helpers
{
    public static class TokenReplace
    {
        public static string ReplaceToken(this string input, List<string> replacements)
        {
            var message = input;
            var positions = message.Split(new string[] { "%@" }, StringSplitOptions.None);

            var newmessage = string.Empty;
            for (var i = 0; i < positions.Length; ++i)
            {
                newmessage += $"{positions[i]} {replacements[i]} ";
            }
            newmessage.TrimEnd(' ');

            return newmessage;
        }
    }
}
