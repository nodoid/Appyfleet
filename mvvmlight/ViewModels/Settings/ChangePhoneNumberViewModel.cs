using System;
using GalaSoft.MvvmLight.Command;

namespace mvvmframework.ViewModels.Settings
{
    public class ChangePhoneNumberViewModel : SettingsViewModel
    {
        IUserSettings settingsService;
        IWebSevices webService;
        IConnection connectionService;

        public ChangePhoneNumberViewModel(IUserSettings settings, IWebSevices web, IConnection con) : base (settings, web,con)
        {
            settingsService = settings;
            webSevice = web;
            connectionService = connectService;
        }

        public string CurrentPhone => settingsService.LoadSetting<string>("Phone", SettingType.String);

        bool canSubmit;
        public bool CanSubmit
        {
            get => canSubmit;
            set { Set(() => CanSubmit, ref canSubmit, value, true); }
        }

        string newPhone;
        public string NewPhone
        {
            get { return newPhone; }
            set 
            {
                if (value != newPhone)
                {
                    if (value.IsValidPhoneNumber())
                    {
                        Set(() => NewPhone, ref newPhone, value, true);
                        CheckSubmit();
                    }
                }
            }
        }

        void CheckSubmit()
        {
            CanSubmit = !NewPhone.Equals(CurrentPhone);
        }

        RelayCommand btnChangePhoneNumber;
        public RelayCommand BtnChangePhoneNumber => btnChangePhoneNumber ??
        (
            btnChangePhoneNumber = new RelayCommand(async () =>
        {
            if (connectService.IsConnected)
            {
                IsBusy = true;
                await webSevice.ChangeMobileNumber(Username, Password, NewPhone).ContinueWith((t) =>
                {
                    if (t.IsCompleted)
                    {
                        IsBusy = false;
                        if (!t.IsFaulted && !t.IsCanceled)
                        {
                            settingsService.SaveSetting("Phone", NewPhone, SettingType.String);
                        }
                    }
                });
            }
        })
        );
    }
}
