using System;
using mvvmframework.Interfaces;
using mvvmframework.Languages;

namespace mvvmframework.ViewModels.Settings
{
    public class AboutViewModel : SettingsViewModel
    {
        IInstallData installService;

        public AboutViewModel(IUserSettings settings, IWebSevices web, IInstallData inst, IConnection con) : base(settings, web,con)
        {
            installService = inst;
        }

        public string VersionNumber => installService.VersionNumber;
        public string VersionDate => installService.Installed;

        public string Trading = Langs.Const_Label_About_Description;
        public string RegAddress = Langs.Const_Label_About_Company_Number;
    }
}
