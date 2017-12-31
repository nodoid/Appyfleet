using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using mvvmframework.Interfaces;
using mvvmframework.Services;
using mvvmframework.ViewModels;
using mvvmframework.ViewModels.Settings;

namespace mvvmframework.ViewModel
{
    public class ViewModelLocator
    {
        public const string SignUpKey = "Signup";
        public const string PairNewVehicleKey = "PairNewVehicle";
        public const string LoginKey = "Login";
        public const string InternalApprovalKey = "InternalApproval";
        public const string DashboardKey = "Dashboard";
        public const string JourneysKey = "Journeys";
        public const string MapsKey = "Maps";
        public const string NotificationsKey = "Notifications";
        public const string MyProfileKey = "MyProfile";
        public const string ScoreHistoryKey = "ScoreHistory";
        public const string SettingsKey = "Settings";
        public const string SOSKey = "SOS";
        public const string ForgottenKey = "Forgotten";
        public const string AboutKey = "About";
        public const string PasswordKey = "Password";
        public const string PhoneKey = "Phone";
        public const string FleetCodeKey = "FleetCode";
        public const string LogFilesKey = "LogFiles";
        public const string MarkettingKey = "Marketting";
        public const string OdoKey = "Odo";
        public const string NoteMapKey = "NoteMap";
        public const string EmergencyKey = "Emergency";
        public const string LanguageKey = "Language";

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IRepository, SqLiteRepository>();
            SimpleIoc.Default.Register<IWebSevices, WebInterface>();
            SimpleIoc.Default.Register<ILogFileService, LogFileService>();
            SimpleIoc.Default.Register<IJourneyService, JourneyService>();

            SimpleIoc.Default.Register<DashboardViewModel>();
            SimpleIoc.Default.Register<ExpensesViewModel>();
            SimpleIoc.Default.Register<InitialApprovalViewModel>();
            SimpleIoc.Default.Register<JourneysViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<MapsViewModel>();
            SimpleIoc.Default.Register<MyProfileViewModel>();
            SimpleIoc.Default.Register<NotificationsViewModel>();
            SimpleIoc.Default.Register<PairNewVehicleViewModel>();
            SimpleIoc.Default.Register<ScoreHistoryViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<SignUpViewModel>();
            SimpleIoc.Default.Register<SOSViewModel>();
            SimpleIoc.Default.Register<AboutViewModel>();
            SimpleIoc.Default.Register<ChangePasswordViewModel>();
            SimpleIoc.Default.Register<ChangePhoneNumberViewModel>();
            SimpleIoc.Default.Register<FleetCodeViewModel>();
            SimpleIoc.Default.Register<LogFilesViewModel>();
            SimpleIoc.Default.Register<MarkettingPrefsViewModel>();
            SimpleIoc.Default.Register<OddometerViewModel>();
            SimpleIoc.Default.Register<NotificationsMapViewModel>();
            SimpleIoc.Default.Register<ForgottenPasswordViewModel>();
            SimpleIoc.Default.Register<EmergencyAdviceViewModel>();
            SimpleIoc.Default.Register<ChangeLanguageViewModel>();
        }

        public DashboardViewModel Dashboard => ServiceLocator.Current.GetInstance<DashboardViewModel>();
        public ExpensesViewModel Expenses => ServiceLocator.Current.GetInstance<ExpensesViewModel>();
        public ForgottenPasswordViewModel Forgotten => ServiceLocator.Current.GetInstance<ForgottenPasswordViewModel>();
        public InitialApprovalViewModel InternalApproval => ServiceLocator.Current.GetInstance<InitialApprovalViewModel>();
        public JourneysViewModel Journeys => ServiceLocator.Current.GetInstance<JourneysViewModel>();
        public LoginViewModel Login => ServiceLocator.Current.GetInstance<LoginViewModel>(); 
        public MapsViewModel Maps => ServiceLocator.Current.GetInstance<MapsViewModel>();
        public MyProfileViewModel MyProfile => ServiceLocator.Current.GetInstance<MyProfileViewModel>();
        public NotificationsViewModel Notifications => ServiceLocator.Current.GetInstance<NotificationsViewModel>(); 
        public NotificationsMapViewModel NoteMap => ServiceLocator.Current.GetInstance<NotificationsMapViewModel>();
        public PairNewVehicleViewModel PairNewVehicle => ServiceLocator.Current.GetInstance<PairNewVehicleViewModel>();
        public ScoreHistoryViewModel ScoreHistory => ServiceLocator.Current.GetInstance<ScoreHistoryViewModel>();
        public SettingsViewModel Settings => ServiceLocator.Current.GetInstance<SettingsViewModel>();
        public SignUpViewModel Signup => ServiceLocator.Current.GetInstance<SignUpViewModel>();
        public SOSViewModel SOS => ServiceLocator.Current.GetInstance<SOSViewModel>();
        public EmergencyAdviceViewModel Emergency => ServiceLocator.Current.GetInstance<EmergencyAdviceViewModel>();

        public AboutViewModel About => ServiceLocator.Current.GetInstance<AboutViewModel>();
        public ChangePasswordViewModel Password => ServiceLocator.Current.GetInstance<ChangePasswordViewModel>();
        public ChangePhoneNumberViewModel Phone => ServiceLocator.Current.GetInstance<ChangePhoneNumberViewModel>();
        public FleetCodeViewModel FleetCode => ServiceLocator.Current.GetInstance<FleetCodeViewModel>();
        public LogFilesViewModel LogFiles => ServiceLocator.Current.GetInstance<LogFilesViewModel>();
        public MarkettingPrefsViewModel Marketting => ServiceLocator.Current.GetInstance<MarkettingPrefsViewModel>();
        public OddometerViewModel Odo => ServiceLocator.Current.GetInstance<OddometerViewModel>();
        public ChangeLanguageViewModel Language => ServiceLocator.Current.GetInstance<ChangeLanguageViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}