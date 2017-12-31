using AppyFleet.UWP.Injected;
using mvvmframework;

[assembly: Xamarin.Forms.Dependency(typeof(UserSettings))]
namespace AppyFleet.UWP.Injected
{
    public class UserSettings : IUserSettings
    {
         public void SaveSetting<T>(string name, T value, SettingType type)
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
            settings.Values[name] = value;
        }

        public T LoadSetting<T>(string name, SettingType type)
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
            dynamic readout = settings.Values[name];
            return readout == null ? default(T) : (T)readout;
        }
    }
}
