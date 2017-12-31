using NewAppyFleet;
using AppyFleet.UWP.Injected;
using System.Globalization;
using Xamarin.Forms;
using Windows.Globalization;

[assembly: Dependency(typeof(Localise))]
namespace AppyFleet.UWP.Injected
{
    public class Localise : ILocalize
    {
        public string GetCurrent()
        {
            return CultureInfo.CurrentUICulture.ToString();
        }

        public void SetLocale(CultureInfo ci)
        {
            //ApplicationLanguages.PrimaryLanguageOverride = ci.TwoLetterISOLanguageName.ToString();
            
        }
    }
}

