using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using mvvmframework.Enums;
using mvvmframework.Languages;
using mvvmframework.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvvmframework
{
    public class LoginViewModel : BaseViewModel
    {
        IDialogService dialogService;
        IUserSettings userService;
        IWebSevices webService;
        IConnection connectService;
        ICultureInfo cultureService;
        IRepository repoService;

        const int checkVal = 7;

        public LoginViewModel(IDialogService dia, IUserSettings user, IWebSevices web, 
            IConnection connect, ICultureInfo culture, IRepository repo)
        {
            dialogService = dia;
            userService = user;
            webService = web;
            connectService = connect;
            cultureService = culture;
            repoService = repo;
            ShowPassword = false;
        }

        public string Progress => userService.LoadSetting<string>("Progress", SettingType.String);

        public void CheckForDetails()
        {
            if (!string.IsNullOrEmpty(userService.LoadSetting<string>("Username", SettingType.String)) &&
                !string.IsNullOrEmpty(userService.LoadSetting<string>("Password", SettingType.String)))
            {
                Username = userService.LoadSetting<string>("Username", SettingType.String);
                Password = userService.LoadSetting<string>("Password", SettingType.String);
                CmdLoginUser.Execute(null);
            }
        }

        bool canLogin;
        public bool CanLogin
        {
            get { return canLogin; }
            set
            {
                Set(() => CanLogin, ref canLogin, value, true);
                CmdLoginUser.CanExecute(value);
            }
        }

        string username;
        public string Username
        {
            get { return username; }
            set { Set(() => Username, ref username, value, true); TestLogin(); }
        }

        string password;
        public string Password
        {
            get { return password; }
            set { Set(() => Password, ref password, value, true); TestLogin(); }
        }

        void TestLogin()
        {
            CanLogin = !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }

        bool showPassword;
        public bool ShowPassword
        {
            get { return showPassword; }
            set 
            { 
                if (value != ShowPassword)
                    Set(() => ShowPassword, ref showPassword, value); 
            }
        }

        public async Task RefreshUserData()
        {
            if (connectService.IsConnected)
            {
                await RefreshData(connectService, webService, cultureService, userService, repoService);
            }
        }

        RelayCommand cmdLoginUser;
        public RelayCommand CmdLoginUser
        {
            get
            {
                return cmdLoginUser ??
                    (
                        cmdLoginUser = new RelayCommand(async () =>
                {
                    var tmpIn = false;
                    var allDone = 1;
                    if (connectService.IsConnected)
                    {
                        IsBusy = true;
                        await webService.Login(Username, Password, string.Empty, cultureService.currentCultureString).ContinueWith((t) =>
                                    {
                                        if (t.IsCompleted)
                                        {
                                            allDone++;
                                            if (!t.IsFaulted && !t.IsCanceled)
                                            {
                                                if (t.Result.Status.Success)
                                                {
                                                    Driver = t.Result;
                                                    UserGuide = Driver.UserGuideURL;
                                                    TermsConditions = Driver.TermsAndConditionsURL;
                                                    DrivingHints = Driver.DriverTipsURL;

                                                    userService.SaveSetting("Username", Username, SettingType.String);
                                                    userService.SaveSetting("DriverId", Driver.DriverId, SettingType.Int);
                                                    userService.SaveSetting("Password", Password, SettingType.String);
                                                    userService.SaveSetting("Phone", Driver.SimNumber, SettingType.String);
                                                    userService.SaveSetting("FleetCode", Driver.FleetName, SettingType.String);
                                                    userService.SaveSetting("RealName", $"{Driver.Forename} {Driver.Surname}", SettingType.String);
                                                    tmpIn = true;
                                                }
                                                else
                                                {
                                                    IsBusy = false;
                                                    Messenger.Default.Send(new NotificationMessage(t.Result.Status.Message));
                                                }
                                            }
                                            if (allDone > checkVal)
                                            {
                                                IsBusy = false;
                                                LoggedIn = true;
                                            }   
                                        }
                                    }).ContinueWith(async (t) =>
                                    {
                                        if (tmpIn)
                                        {
                                                await webService.GetJourneys(Username, Password).ContinueWith(async (w) =>
                                                {
                                                    if (w.IsCompleted)
                                                    {
                                                        allDone++;
                                                        if (!w.IsCanceled && !w.IsFaulted)
                                                        {
                                                            repoService.SaveData(w.Result.Journeys);
                                                            var jd = new List<JourneyData>();
                                                            foreach(var j in w.Result.Journeys)
                                                            {
                                                                jd.Add(new JourneyData
                                                                {
                                                                    JourneyId = j.Id,
                                                                    JourneyEndDate = j.EndDate,
                                                                    JourneyStartDate = j.StartDate,
                                                                    JourneyNumber = j.JourneyNumber
                                                                });
                                                            }
                                                            repoService.SaveData(jd);
                                                            var c = 0;
                                                            var d = false;
                                                            var id = 0L;
                                                            foreach (var dt in w.Result.Journeys)
                                                            {
                                                                id = dt.Id;
                                                                await webService.GetJourneyCoordinates(Username, Password, dt.Id).ContinueWith((l) =>
                                                                {
                                                                    if (l.IsCompleted)
                                                                    {
                                                                        if (!l.IsCanceled && !l.IsFaulted)
                                                                        {
                                                                            if (l.Result.GeoCoordinates.Count != 0)
                                                                            {
                                                                                var m = l.Result.GeoCoordinates;
                                                                                m = m.ForEach(x => x.JourneyId = id).ToList();
                                                                                repoService.SaveData(m);
                                                                            }
                                                                            c++;
                                                                        }
                                                                        else
                                                                        {
                                                                            Messenger.Default.Send(new NotificationMessage(l.Result.Status.Message));
                                                                            c++;
                                                                        }
                                                                    }
                                                                });
                                                            }
                                                            if (c == w.Result.Journeys.Count)
                                                                allDone++;
                                                        }
                                                        else
                                                        {
                                                            IsBusy = false;
                                                            Messenger.Default.Send(new NotificationMessage(w.Result.Status.Message));
                                                        }
                                                    }
                                                    if (allDone > checkVal)
                                                    {
                                                        IsBusy = false;
                                                        LoggedIn = true;
                                                    }
                                                });
                                            }
                                    }).ContinueWith(async (q) =>
                                    {
                                        if (tmpIn)
                                        {
                                            await webService.GetDriverScores(Username, Password, (int)RefineSearch.YearToDate).ContinueWith((w) =>
                                            {
                                                if (w.IsCompleted)
                                                {
                                                    allDone++;
                                                    if (!w.IsFaulted && !w.IsCanceled)
                                                    {
                                                        DriverScores = w.Result;
                                                        repoService.SaveData(DriverScores);
                                                    }
                                                }
                                                if (allDone > checkVal)
                                                {
                                                    IsBusy = false;
                                                    LoggedIn = true;
                                                }
                                            });
                                        }
                                    }).ContinueWith(async(_)=>
                                    {
                                        if (tmpIn)
                                        {
                                            await webService.GetPairedVehicles(Username, Password).ContinueWith((l) =>
                                            {
                                                if (l.IsCompleted)
                                                {
                                                    allDone++;
                                                    if (!l.IsFaulted && !l.IsCanceled)
                                                    {
                                                        var id = l.Result.Vehicles.FirstOrDefault(a => a.PairingId != 0)?.PairingId.ToString();
                                                        PairId = id;
                                                        VehicleId = l.Result.Vehicles.FirstOrDefault().Id;
                                                    }
                                                }
                                    if (allDone > checkVal)
                                    {
                                                    IsBusy = false;
                                                    LoggedIn = true;
                                    }
                                            });
                                        }
                                    }).ContinueWith(async(_)=>
                                    {
                                        if (tmpIn)
                                        {
                                            await webService.GetNotifications(Username, Password).ContinueWith((t) =>
                                                      {
                                                          if (t.IsCompleted)
                                                          {
                                                              allDone++;
                                                              if (!t.IsFaulted && !t.IsCanceled)
                                                              {
                                                                  if (t.Result.Notifications == null)
                                                                      Notifications = new List<NotificationModel>();
                                                                  else
                                                                      Notifications = t.Result.Notifications;
                                                              }
                                                          }
                                    if (allDone > checkVal)
                                                          {
                                                              IsBusy = false;
                                                              LoggedIn = true;
                                                          }
                                                      });
                                        }
                                    }).ContinueWith(async (j) =>
                                    {
                                        if (tmpIn)
                                        {
                                            await webService.GetJourneys(Username, Password).ContinueWith((t) =>
                                            {
                                                if (t.IsCompleted)
                                                {
                                                    allDone++;
                                                    if (!t.IsFaulted && !t.IsCanceled)
                                                    {
                                                        var dbList = new List<DBJourneyModel>();
                                                        var last = repoService.GetID<DBJourneyModel>();
                                                        foreach (var jo in t.Result.Journeys)
                                                        {
                                                            dbList.Add(new DBJourneyModel
                                                            {
                                                                JourneyId = last,
                                                                EndDate = jo.EndDate,
                                                                EndLocation = jo.EndLocation,
                                                                Id = jo.Id,
                                                                JourneyNumber = jo.JourneyNumber,
                                                                Miles = jo.Miles,
                                                                Nickname = jo.Nickname,
                                                                OverallScore = jo.OverallScore,
                                                                PolicyId = jo.PolicyId,
                                                                Private = jo.Private,
                                                                SmoothScore = jo.SmoothScore,
                                                                SpeedScore = jo.SpeedScore,
                                                                StartDate = jo.StartDate,
                                                                StartLocation = jo.StartLocation,
                                                                UsageScore = jo.UsageScore,
                                                                JourneyType = jo.Private ? Langs.Const_Label_Personal_Mileage : Langs.Const_Button_Journeys_Tab2
                                                            });
                                                            last++;
                                                        }
                                                        var data = repoService.Count<DBJourneyModel>();
                                                        if (data == 0)
                                                        {
                                                            repoService.SaveData<DBJourneyModel>(dbList);
                                                            JourneysList = dbList;
                                                        }
                                                        else
                                                        {
                                                            // diff based on the id
                                                            var d = repoService.GetList<DBJourneyModel>();

                                                            var diff = dbList.Where(w => !d.Any(p => p.JourneyId == w.JourneyId)).ToList();
                                                            repoService.SaveData(diff);
                                                            var _ = JourneysList;
                                                            _.AddRange(diff);
                                                            JourneysList = _;
                                                        }
                                            if (allDone > checkVal)
                                                        {
                                                            IsBusy = false;
                                                            LoggedIn = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Messenger.Default.Send(new NotificationMessage(t.Result.Status.Message));
                                                        IsBusy = false;
                                                    }
                                                }
                                    if (allDone > checkVal)
                                                {
                                                    IsBusy = false;
                                                    LoggedIn = true;
                                                }
                                            });
                                        }
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
