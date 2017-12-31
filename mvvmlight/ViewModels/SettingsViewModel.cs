using System;
namespace mvvmframework.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public IUserSettings settingsService;
        public IWebSevices webSevice;
        public IConnection connectService;

        public SettingsViewModel(IUserSettings settings, IWebSevices web, IConnection connect)
        {
            settingsService = settings;
            webSevice = web;
            connectService = connect;
        }

        public string Username => settingsService.LoadSetting<string>("Username", SettingType.String);
        public string Password => settingsService.LoadSetting<string>("Password", SettingType.String);
    }
}
