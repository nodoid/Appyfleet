using System;
namespace mvvmframework.ViewModels
{
    public class InitialApprovalViewModel : BaseViewModel
    {
        IUserSettings userService;

        public InitialApprovalViewModel(IUserSettings user)
        {
            userService = user;
        }

        bool optOneTicked;
        public bool OptOneTicked
        {
            get { return optOneTicked; }
            set { Set(() => OptOneTicked, ref optOneTicked, value, true); TestCanClick();}
        }

        bool optTwoTicked;
        public bool OptTwoTicked
        {
            get { return optTwoTicked; }
            set { Set(() => OptTwoTicked, ref optTwoTicked, value, true); TestCanClick();}
        }

        bool optThreeTicked;
        public bool OptThreeTicked
        {
            get { return optThreeTicked; }
            set { Set(() => OptThreeTicked, ref optThreeTicked, value, true); TestCanClick();}
        }

        bool okToGo;
        public bool OkToGo
        {
            get { return okToGo; }
            set
            {
                Set(() => OkToGo, ref okToGo, value, true);
                userService.SaveSetting("Progress", "Initial|", SettingType.String);
            }
        }

        bool okGo;
        public bool OkGo
        {
            get => okGo;
            set { Set(() => OkGo, ref okGo, value, true); }
        }

        void TestCanClick()
        {

            OkToGo = OptOneTicked && OptTwoTicked && OptThreeTicked;    
        }
    }
}
