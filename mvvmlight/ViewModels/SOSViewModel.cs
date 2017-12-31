using System;
using System.Collections.Generic;
using mvvmframework.Helpers;
using mvvmframework.Languages;

namespace mvvmframework.ViewModels
{
    public class SOSViewModel : BaseViewModel
    {
        IUserSettings userService;

        public SOSViewModel(IUserSettings user)
        {
            userService = user;
        }

        int currentViewPage;
        public int CurrentViewPage
        {
            get => currentViewPage;
            set => Set(() => CurrentViewPage, ref currentViewPage, value, true);
        }

        public string GetSOSMainTitle => Langs.Const_Msg_Pair_Vehicle_Description_2;

        List<string> SOSTitles => new List<string> 
        {
            Langs.Const_Msg_SOS_Title_1,
            Langs.Const_Msg_SOS_Title_2,
            Langs.Const_Msg_SOS_Title_3,
            Langs.Const_Msg_SOS_Title_4,
            Langs.Const_Msg_SOS_Title_5,
            Langs.Const_Msg_SOS_Title_6,
            Langs.Const_Msg_SOS_Title_7
        };

        List<string> SOSDescriptions => new List<string>
        {
            Langs.Const_Msg_SOS_Description_1,
            Langs.Const_Msg_SOS_Description_2,
            Langs.Const_Msg_SOS_Description_3,
            Langs.Const_Msg_SOS_Description_4,
            Langs.Const_Msg_SOS_Description_5,
            Langs.Const_Msg_SOS_Description_6,
            Langs.Const_Msg_SOS_Description_7
        };

        List<string> SOSImages => new List<string>
        {
            "icon_0", "icon_1", "icon_2", "icon_3", "icon_4", "icon_5", "icon_6"
        };

        public List<SOSPageModel> GetSOSListModel
        {
            get
            {
                var model = new List<SOSPageModel>();
                for (var _ = 0; _ < SOSTitles.Count; ++_)
                    model.Add(new SOSPageModel { Image = SOSImages[_], Text = SOSDescriptions[_], Title = SOSTitles[_] });
                return model;
            }
        }

        public string GetSOSMessageOne => Langs.Const_Msg_SOS_1.ReplaceToken(new List<string>
                {
                    userService.LoadSetting<string>("FirstName", SettingType.String),
                    userService.LoadSetting<string>("LastName", SettingType.String),
                    Driver.OdoVehicleReg,
                    DateTime.Now.ToString("U")
                });

        public string GetSOSMessageTwo => Langs.Const_Msg_SOS_2.ReplaceToken(new List<string>
                {
                    userService.LoadSetting<string>("FirstName", SettingType.String),
                    userService.LoadSetting<string>("LastName", SettingType.String),
            DateTime.Now.TimeOfDay.ToString()
                });

        public string SendIncidentAlert => Langs.Const_Button_Send_Incident;
    }
}
