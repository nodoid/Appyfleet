using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace mvvmframework.ViewModels.Settings
{
    public class ChangeLanguageViewModel : SettingsViewModel
    {
        public ChangeLanguageViewModel(IUserSettings settings, IWebSevices web, IConnection connect) : base(settings, web, connect)
        {
            Task.Run(async()=>await GetLanguages());
        }

        async Task GetLanguages()
        {
            IsBusy = true;
            await webSevice.GetLanguageList(settingsService.LoadSetting<string>("Username", SettingType.String),
                                            settingsService.LoadSetting<string>("Password", SettingType.String)).ContinueWith((_) =>
            {
                if (_.IsCompleted)
                {
                    IsBusy = false;
                    if (!_.IsCanceled && !_.IsFaulted)
                    {
                        if (_.Result.Status.Success)
                        {
                            Languages = _.Result.Languages;
                        }
                        else
                        {
                            Messenger.Default.Send(new NotificationMessage(_.Result.Status.Message));
                        }
                    }
                }
            });
        }

        List<LanguageModel> languages;
        public List<LanguageModel> Languages
        {
            get => languages;
            set { Set(() => Languages, ref languages, value, true); }
        }

        string newLanguage;
        public string NewLanguage
        {
            get => newLanguage;
            set 
            {
                if (!NewLanguage.Equals(value))
                {
                    Set(() => NewLanguage, ref newLanguage, value, true);
                    Task.Run(async () => await ChangeLanguage());
                }
            }
        }

        async Task ChangeLanguage()
        {
            IsBusy = true;
            await webSevice.SetLanguagePreference(settingsService.LoadSetting<string>("Username", SettingType.String),
                                            settingsService.LoadSetting<string>("Password", SettingType.String),
                                                  Languages.FirstOrDefault(t => t.Name == NewLanguage).Code).ContinueWith((_) =>
              {
                  if (_.IsCompleted)
                  {
                      IsBusy = false;
                      if (!_.IsFaulted && !_.IsCanceled)
                      {
                          if (!_.Result.Status.Success)
                              Messenger.Default.Send(new NotificationMessage(_.Result.Status.Message));
                      }
                  }
              });
        }
    }
}
