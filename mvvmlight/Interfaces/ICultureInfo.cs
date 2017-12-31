using System;
using System.Globalization;

namespace mvvmframework
{
    public interface ICultureInfo
    {
        CultureInfo currentCulture { get; set; }
        string currentCultureString { get; }
    }
}
