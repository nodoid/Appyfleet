using System;
using GalaSoft.MvvmLight.Command;

namespace mvvmframework.ViewModels.Settings
{
    public class FleetCodeViewModel : SettingsViewModel
    {
        public FleetCodeViewModel(IUserSettings settings, IWebSevices web, IConnection con) : base(settings, web, con)
        {
        }

        public string FleetNumber => settingsService.LoadSetting<string>("FleetCode", SettingType.String);

        string newFleetCode;
        public string NewFleetCode
        {
            get { return newFleetCode; }
            set
            {
                if (value != newFleetCode)
                {
                    Set(() => NewFleetCode, ref newFleetCode, value);
                    CheckCanSubmit();
                }
            }
        }

        void CheckCanSubmit()
        {
            CanSubmit = !NewFleetCode.Equals(FleetNumber);
        }

        bool canSubmit;
        public bool CanSubmit
        {
            get => canSubmit;
            set { Set(() => CanSubmit, ref canSubmit, value, true); }
        }

        RelayCommand btnFleetCode;
        public RelayCommand BtnFleetCode => btnFleetCode ??
        (
            btnFleetCode = new RelayCommand(async()=>
        {
            if (connectService.IsConnected)
            {
                IsBusy = true;
                await webSevice.RequestJoinFleet(Username, Password, NewFleetCode).ContinueWith((t) =>
                {
                    if (t.IsCompleted)
                    {
                        IsBusy = false;
                        if (!t.IsFaulted && !t.IsCanceled)
                        {
                            settingsService.SaveSetting("FleetCode", FleetNumber, SettingType.String);
                        }
                    }
                });
            }
        })
        );
    }
}
