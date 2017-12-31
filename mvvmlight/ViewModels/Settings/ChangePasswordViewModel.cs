using GalaSoft.MvvmLight.Command;

namespace mvvmframework.ViewModels.Settings
{
    public class ChangePasswordViewModel : SettingsViewModel
    {
        public ChangePasswordViewModel(IUserSettings settings, IWebSevices web, IConnection connect) : base(settings, web, connect)
        {
        }

        string currentPassword;
        public string CurrentPassword
        {
            get => currentPassword;
            set
            {
                if (value.Equals(Password))
                {
                    Set(() => CurrentPassword, ref currentPassword, value);
                    PasswordEnabled = true;
                }
            }
        }

        bool passwordEnabled;
        public bool PasswordEnabled
        {
            get => passwordEnabled;
            set { Set(() => PasswordEnabled, ref passwordEnabled, true, value); }
        }


        string newPasswordOne;
        public string NewPasswordOne
        {
            get { return newPasswordOne; }
            set
            {
                if (value.Length >= 8)
                    Set(() => NewPasswordOne, ref newPasswordOne, value, true);
                CheckPasswords();
            }
        }

        string newPasswordTwo;
        public string NewPasswordTwo
        {
            get { return newPasswordTwo; }
            set
            {
                if (value.Length >= 8)
                    Set(() => NewPasswordTwo, ref newPasswordTwo, value, true);
                CheckPasswords();
            }
        }

        bool hidePassword;
        public bool HidePassword
        {
            get { return hidePassword; }
            set { Set(() => HidePassword, ref hidePassword, value, true); }
        }

        void CheckPasswords()
        {
            EnableButton = NewPasswordOne.Equals(NewPasswordTwo);
        }

        bool enableButton;
        public bool EnableButton
        {
            get => enableButton;
            set { Set(() => EnableButton, ref enableButton, value, true); }
        }

        RelayCommand btnChangePassword;
        public RelayCommand BtnChangePassword => btnChangePassword ??
                    (
                        btnChangePassword = new RelayCommand(async () =>
                {
                    if (connectService.IsConnected)
                    {
                        IsBusy = true;
                        await webSevice.ChangePassword(Username, Password, NewPasswordOne, NewPasswordTwo).ContinueWith((t) =>
                        {
                            if (t.IsCompleted)
                            {
                                IsBusy = false;
                                if (!t.IsFaulted && !t.IsCanceled)
                                {
                                    settingsService.SaveSetting("Password", NewPasswordOne, SettingType.String);
                                }
                            }
                        });
                    }
                }));
    }
}
