using System;
using System.Globalization;
using System.Threading;
using NewAppyFleet.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(Localise))]
namespace NewAppyFleet.Droid
{
    public class Localise : ILocalize
    {
        public void SetLocale(CultureInfo culture = null)
        {
            var androidLocale = Java.Util.Locale.Default;
            var netLocale = androidLocale.ToString().Replace("_", "-");

            var ci = culture == null ? new CultureInfo(netLocale) : culture;
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }

        public string GetCurrent()
        {
            var androidLocale = Java.Util.Locale.Default;
            var netLanguage = androidLocale.Language.Replace("_", "-");
            var netLocale = androidLocale.ToString().Replace("_", "-");

#if DEBUG
            Console.WriteLine("android:  " + androidLocale.ToString());
            Console.WriteLine("netlang:  " + netLanguage);
            Console.WriteLine("netlocale:" + netLocale);
#endif

            var ci = new CultureInfo(netLocale);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

#if DEBUG
            Console.WriteLine("thread:  " + Thread.CurrentThread.CurrentCulture);
            Console.WriteLine("threadui:" + Thread.CurrentThread.CurrentUICulture);
#endif

            return netLocale;
        }
    }
}

