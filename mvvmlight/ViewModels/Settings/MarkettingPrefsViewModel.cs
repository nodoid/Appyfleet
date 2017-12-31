using System;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using mvvmframework.Languages;

namespace mvvmframework.ViewModels.Settings
{
    public class MarkettingPrefsViewModel : SettingsViewModel
    {
        public MarkettingPrefsViewModel(IUserSettings settings, IWebSevices web, IConnection con) : base(settings, web, con)
        {
        }

        public string ShowBoilerPlate => Langs.Const_Msg_Marketing;
        public bool ShowCurrentMarketting => settingsService.LoadSetting<bool>("Marketting", SettingType.Bool);

        bool changeMarketting;
        public bool ChangeMarketting
        {
            get { return changeMarketting; }
            set 
            {
                if (changeMarketting != value)
                {
                    Set(() => ChangeMarketting, ref changeMarketting, value, true);
                    settingsService.SaveSetting("Marketting", value, SettingType.Bool);
                    ChangeMarkettingCommand.Execute(null);
                }
            }
        }

        RelayCommand changeMarkettingCommand;
        public RelayCommand ChangeMarkettingCommand => changeMarkettingCommand ??
        (
            changeMarkettingCommand = new RelayCommand(async () =>
        {
            if (connectService.IsConnected)
            {
                IsBusy = true;
                await webSevice.SetMarketingPreference(Username, Password, ChangeMarketting).ContinueWith((t) =>
                    {
                        if (t.IsCompleted)
                        {
                            IsBusy = false;
                            if (!t.IsCanceled && !t.IsFaulted)
                            {
                                if (t.Result.Status.Success)
                                    settingsService.SaveSetting("Marketting", ChangeMarketting, SettingType.Bool);
                                else
                                    Messenger.Default.Send(new NotificationMessage(t.Result.Status.Message));
                            }
                        }
                    });
            }
        })
        );
    }
}
