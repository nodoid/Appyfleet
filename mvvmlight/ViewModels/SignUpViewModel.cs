using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using mvvmframework.Languages;
using GalaSoft.MvvmLight.Messaging;

namespace mvvmframework
{
    public class SignUpViewModel : BaseViewModel
    {
        IUserSettings userServices;
        IWebSevices webServices;
        ICultureInfo cultureServices;
        IDialogService diaServices;
        IConnection connectionService;

        public SignUpViewModel(IUserSettings user, IWebSevices web, ICultureInfo culture, IDialogService dia, 
                               IConnection connect)
        {
            userServices = user;
            webServices = web;
            cultureServices = culture;
            diaServices = dia;
            connectionService = connect;

            ShowPasswords = true;
        }

        string firstName;
        public string FirstName 
        {
            get { return firstName; }
            set { Set(() => FirstName, ref firstName, value, true); }
        }

        string lastName;
        public string LastName
        {
            get { return lastName; }
            set { Set(() => LastName, ref lastName, value, true); }
        }

        string yob;
        public string YOB
        {
            get { return yob; }
            set
            {
                if (value.IsStringValidDOB())
                {
                    if (value.Length == 4)
                    {
                        Set(() => YOB, ref yob, value, true);
                        userServices.SaveSetting("YearOfBirth", value, SettingType.String);
                    }
                }
                else
                {
                    if (value.Length > 1)
                        value = value.Remove(value.Length - 1, 1);
                    else
                        value = string.Empty;
                    Set(() => YOB, ref yob, value, true);
                }
            }
        }

        string mobNumber;
        public string MobNumber
        {
            get { return mobNumber; }
            set
            {
                if (value.IsValidPhoneNumber())
                    Set(() => MobNumber, ref mobNumber, value, true);
            }
        }

        public void CanMoveToTwo()
        {
            if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName) &&
                                   !string.IsNullOrEmpty(YOB) && !string.IsNullOrEmpty(MobNumber))
                MoveToTwo = !MoveToTwo;
        }


        bool moveToTwo;
        public bool MoveToTwo
        {
            get { return moveToTwo; }
            set
            {
                Set(() => MoveToTwo, ref moveToTwo, value, true);
            }
        }

        string companyName;
        public string CompanyName
        {
            get { return companyName; }
            set { Set(() => CompanyName, ref companyName, value, true); }
        }

        string position;
        public string Position
        {
            get { return position; }
            set { Set(() => Position, ref position, value, true); }
        }

        bool emailOK;
        public bool EmailOK
        {
            get => emailOK;
            set { Set(() => EmailOK, ref emailOK, value, true); }
        }

        string emailAddress;
        public string EmailAddress
        {
            get { return emailAddress; }
            set
            {
                Set(() => EmailAddress, ref emailAddress, value, true);
            }
        }

        RelayCommand cmdCheckDriver;
        public RelayCommand CmdCheckDriver
        {
            get
            {
                return cmdCheckDriver ??
                    (
                        cmdCheckDriver = new RelayCommand(async () =>
                {
                    if (EmailAddress.IsValidEmailAddress() && !string.IsNullOrEmpty(EmailAddress))
                    {
                        IsBusy = true;
                        if (connectionService.IsConnected)
                        {
                            await webServices.CheckForExistingDriver(EmailAddress, cultureServices.currentCultureString).ContinueWith((t) =>
                            {
                                if (t.IsCompleted)
                                {
                                    IsBusy = false;
                                    if (!t.IsFaulted && !t.IsCanceled)
                                    {
                                        if (t.Result.Exists)
                                        {
                                            Messenger.Default.Send(new NotificationMessage(t.Result.Message));
                                            EmailAddress = string.Empty;
                                            EmailOK = false;
                                        }
                                        else
                                        {
                                            EmailOK = true;
                                            MoveToThree = !MoveToThree;
                                        }
                                    }
                                    else
                                    {
                                        Messenger.Default.Send(new NotificationMessage(t.Result.Status.Message));
                                        MoveToTwo = false;
                                        EmailOK = false;
                                    }
                                }
                            });
                        }
                        else
                        {
                            IsBusy = false;
                            Messenger.Default.Send(new NotificationMessage(Langs.Const_Msg_Disagree_Failed));
                            EmailOK = false;
                        }
                    }
                })
                );
            }
        }

        public void CanMoveToThree()
        {
            if (EmailOK)
                MoveToThree = !MoveToThree;
        }

        bool moveToThree;
        public bool MoveToThree
        {
            get { return moveToThree; }
            set { Set(() => MoveToThree, ref moveToThree, value, true); }
        }

        public void CanMoveToFour()
        {
            if (!string.IsNullOrEmpty(PasswordOne) && !string.IsNullOrEmpty(PasswordTwo))
            {
                if (PasswordOne.Equals(PasswordTwo))
                    MoveToFour = !MoveToFour;
            }
        }

        bool moveToFour;
        public bool MoveToFour
        {
            get => moveToFour;
            set => Set(ref moveToFour, value, true);
        }

        string passwordOne;
        public string PasswordOne
        {
            get { return passwordOne; }
            set 
            {
                Set(() => PasswordOne, ref passwordOne, value, true);
            }
        }

        string passwordTwo;
        public string PasswordTwo
        {
            get { return passwordTwo; }
            set 
            {
                Set(() => PasswordTwo, ref passwordTwo, value, true);
            }
        }

        public bool PasswordsMatch 
        {
            get
            {
                var rv = false;
                if (PasswordOne.Length > 7 && PasswordTwo.Length > 7)
                {
                    rv = PasswordOne.Equals(PasswordTwo);
                }
                else
                    Messenger.Default.Send(new NotificationMessage($"${Langs.Const_Msg_Password_Invalid}"));
                return rv;
            }
        }

        bool showPasswords;
        public bool ShowPasswords 
        {
            get { return showPasswords; }
            set { Set(() => ShowPasswords, ref showPasswords, value, true); }
        }

        bool emailPrefs;
        public bool EmailPrefs
        {
            get { return emailPrefs; }
            set { Set(() => EmailPrefs, ref emailPrefs, value, true); }
        }

        bool hasFleetCode;
        public bool HasFleetCode
        {
            get { return hasFleetCode; }
            set { Set(() => HasFleetCode, ref hasFleetCode, value, true); }
        }

        string fleetCode;
        public string FleetCode
        {
            get { return fleetCode; }
            set { Set(() => FleetCode, ref fleetCode, value); CmdAddFleetCode.Execute(null);}
        }

        bool allDone;
        public bool AllDone
        {
            get { return allDone; }
            set { Set(() => AllDone, ref allDone, value, true); }
        }

        bool moveToPairing;
        public bool MoveToPairing
        {
            get => moveToPairing;
            set => Set(ref moveToPairing, value, true);
        }

        RelayCommand cmdAddFleetCode;
        public RelayCommand CmdAddFleetCode
        {
            get
            {
                return cmdAddFleetCode ??
                    (
                        cmdAddFleetCode = new RelayCommand(async () =>
                {
                    IsBusy = true;
                    await webServices.RequestJoinFleet(userServices.LoadSetting<string>("Username", SettingType.String),
                                                       userServices.LoadSetting<string>("Password", SettingType.String),
                                                       FleetCode).ContinueWith((t) =>
                    {
                        if (t.IsCompleted)
                        {
                            IsBusy = false;
                            if (!t.IsFaulted && !t.IsCanceled)
                            {
                                if (t.Result.Status.Success)
                                {
                                    FleetScore = t.Result.FleetScore;
                                    FleetName = t.Result.FleetName;
                                }
                            }
                            else
                            {
                                Messenger.Default.Send(new NotificationMessage(t.Result.Status.Message));
                            }
                        }
                    });
                })
                );
            }
        }

        RelayCommand cmdRegisterUser;
        public RelayCommand CmdRegisterUser
        {
            get 
            {
                return cmdRegisterUser ??
                    (
                        cmdRegisterUser = new RelayCommand(async () =>
                {
                    IsBusy = true;
                    var user = new RegisterRequestJSon
                    {
                        accessCode = string.Empty,
                        marketingAgreed = EmailPrefs,
                        dateOfBirth = YOB,
                        workEmail = EmailAddress,
                        firstName = FirstName,
                        languageCode = cultureServices.currentCultureString,
                        mobileNumber = MobNumber,
                        companyName = CompanyName,
                        password = PasswordOne,
                        position = Position,
                        surname = LastName
                    };
                    if (connectionService.IsConnected)
                    {
                        await webServices.Register(user).ContinueWith((t) =>
                        {
                            if (t.IsCompleted)
                            {
                                if (!t.IsFaulted && !t.IsCanceled)
                                {
                                    if (t.Result.RegisterV3Result.Status.Success)
                                    {
                                        //AllDone = true;
                                        userServices.SaveSetting("Username", EmailAddress, SettingType.String);
                                        userServices.SaveSetting("DriverId", t.Result.RegisterV3Result.DriverId, SettingType.Int);
                                        userServices.SaveSetting("Password", PasswordOne, SettingType.String);
                                        userServices.SaveSetting("Phone", MobNumber, SettingType.String);
                                        userServices.SaveSetting("Marketting", EmailPrefs, SettingType.Bool);
                                        userServices.SaveSetting("FleetCode", FleetCode, SettingType.String);
                                        userServices.SaveSetting("RealName", $"{FirstName} {LastName}", SettingType.String);

                                        userServices.SaveSetting("Progress", "Initial|Signup|", SettingType.String);
                                    }
                                    else
                                    {
                                        IsBusy = false;
                                        Messenger.Default.Send(new NotificationMessage(t.Result.RegisterV3Result.Status.Message));
                                    }
                                }
                            }
                        }).ContinueWith(async (_) =>
                        {
                            await webServices.GetPairedVehicles(userServices.LoadSetting<string>("Username", SettingType.String),
                                                                userServices.LoadSetting<string>("Password", SettingType.String)).ContinueWith((w) =>
                            {
                                if (w.IsCompleted)
                                {
                                    AllDone = true;
                                    IsBusy = false;
                                    if (!w.IsFaulted && !w.IsCanceled)
                                    {
                                        if (w.Result.Status.Success)
                                        {
                                            VehicleModels = w.Result.Vehicles;
                                        }
                                        else
                                        {
                                            Messenger.Default.Send(new NotificationMessage(w.Result.Status.Message));
                                        }
                                    }
                                }
                            });
                        });
                    }
                    else
                    {
                        IsBusy = false;
                        Messenger.Default.Send(new NotificationMessage(Langs.Const_Msg_Disagree_Failed));
                    }
                })
                  );
            }
        }
    }
}
