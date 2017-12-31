using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using mvvmframework.Helpers;
using mvvmframework.Models;
using mvvmframework.Models.JSon;
using System.Linq;
using mvvmframework.Enums;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using mvvmframework.Languages;

namespace mvvmframework
{
    public class BaseViewModel : ViewModelBase
    {
        bool isConnected;
        public bool IsConnected
        {
            get { return isConnected; }
            set { Set(() => IsConnected, ref isConnected, value); }
        }

        static double currentSpeed;
        public double CurrentSpeed
        {
            get => currentSpeed;
            set { Set(() => CurrentSpeed, ref currentSpeed, value, true); }
        }

        static GetDriverScoreData driverScores;
        public GetDriverScoreData DriverScores
        {
            get { return driverScores; }
            set { Set(() => DriverScores, ref driverScores, value, true); }
        }

        static string fleetName;
        public string FleetName
        {
            get => fleetName;
            set { Set(() => FleetName, ref fleetName, value); }
        }

        static double fleetScore;
        public double FleetScore
        {
            get => fleetScore;
            set { Set(() => FleetScore, ref fleetScore, value); }
        }

        static bool loggedIn;
        public bool LoggedIn
        {
            get => loggedIn;
            set { Set(() => LoggedIn, ref loggedIn, value, true); }
        }

        static bool showPrivate;
        public bool ShowPrivate
        {
            get { return showPrivate; }
            set { Set(() => ShowPrivate, ref showPrivate, value); }
        }

        public RefineSearch Current { get; set; }

        static List<NotificationModel> notifications;
        public List<NotificationModel> Notifications
        {
            get => notifications;
            set { Set(() => Notifications, ref notifications, value, true); }
        }

        static List<DBJourneyModel> journeysList;
        public List<DBJourneyModel> JourneysList
        {
            get
            {
                // null check
                if (journeysList == null)
                    return new List<DBJourneyModel>();
                // do private filter first
                var data = journeysList?.Where(t => t.Private == ShowPrivate).ToList();
                // filters
                if (Current == RefineSearch.NoRefine)
                    return data;
                switch (Current)
                {
                    case RefineSearch.CalendarMonth:
                        data = data?.Where(t => t.StartDate.Month == DateTime.Now.Month).ToList();
                        break;
                    case RefineSearch.YearToDate:
                        data = data?.Where(t => t.StartDate.Year == DateTime.Now.Year).ToList();
                        break;
                    case RefineSearch.SevenDays:
                        data = data?.Where(t => t.StartDate.Date >= DateTime.Now.Date.AddDays(-7)).ToList();
                        break;
                    case RefineSearch.ThirtyDays:
                        data = data?.Where(t => t.StartDate >= DateTime.Now.Date.AddDays(-30)).ToList();
                        break;
                    case RefineSearch.CurrentWeek:
                        // assume Monday = day 1
                        var currentDay = DateTime.Now.DayOfWeek;
                        var days = 0;
                        if (currentDay != DayOfWeek.Monday)
                        {
                            if (currentDay > DayOfWeek.Monday && currentDay != DayOfWeek.Sunday)
                            {
                                days = (int)DateTime.Now.DayOfWeek - (int)DayOfWeek.Monday;
                            }
                            else
                                days = 7;
                        }
                        data = data?.Where(t => t.StartDate >= DateTime.Now.AddDays(-days)).ToList();
                        break;
                }
                return data;
            }
            set { Set(() => JourneysList, ref journeysList, value, true); }
        }

        bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { Set(() => IsBusy, ref isBusy, value, true); }
        }

        static DriverModel driver;
        public DriverModel Driver
        {
            get { return driver; }
            set { Set(() => Driver, ref driver, value, true); }
        }

        static string serializedLoginDetails;
        public string SerializedLoginDetails
        {
            get => serializedLoginDetails; 
            set { Set(() => SerializedLoginDetails, ref serializedLoginDetails, Serializer.SerializeToString(value), true); }
        }

        static List<VehicleModel> vehicleModels;
        public List<VehicleModel> VehicleModels
        {
            get { return vehicleModels; }
            set { Set(() => VehicleModels, ref vehicleModels, value, true); }
        }

        static int currentVehicleId;
        public int CurrentVehicleId
        {
            get { return currentVehicleId; }
            set { Set(() => CurrentVehicleId, ref currentVehicleId, value); }
        }

        static JourneyData journeyData;
        public JourneyData JourneyData
        {
            get => journeyData;
            set { Set(() => JourneyData, ref journeyData, value); }
        }

        static int journeyCount;
        public int JourneyCount
        {
            get => journeyCount;
            set { Set(() => JourneyCount, ref journeyCount, value); }
        }

        static DateTime appStarted;
        public DateTime AppStarted
        {
            get => appStarted;
            set { Set(() => AppStarted, ref appStarted, value); }
        }

        static TimeSpan offset;
        public TimeSpan Offset
        {
            get => offset;
            set { Set(() => Offset, ref offset, value); }
        }

        static List<string> messageQueue;
        public List<string> MessageQueue
        {
            get => messageQueue;
            set { Set(() => MessageQueue, ref messageQueue, value); }
        }

        static string pairId;
        public string PairId
        {
            get => pairId;
            set { Set(() => PairId, ref pairId, value); }
        }

        static long vehicleId;
        public long VehicleId
        {
            get => vehicleId;
            set { Set(() => VehicleId, ref vehicleId, value, true); }
        }

        static bool gpsOn;
        public bool GpsOn
        {
            get => gpsOn;
            set { Set(() => GpsOn, ref gpsOn, value); }
        }

        static bool bluetoothOn;
        public bool BluetoothOn
        {
            get => bluetoothOn;
            set { Set(() => BluetoothOn, ref bluetoothOn, value); }
        }

        static bool locationOn;
        public bool LocationOn
        {
            get => locationOn;
            set { Set(() => LocationOn, ref locationOn, value); }
        }

        static string userGuide;
        public string UserGuide
        {
            get => userGuide;
            set { Set(() => UserGuide, ref userGuide, value); }
        }

        static string termsConditions;
        public string TermsConditions
        {
            get => termsConditions;
            set { Set(() => TermsConditions, ref termsConditions, value); }
        }

        static string drivingHints;
        public string DrivingHints
        {
            get => drivingHints;
            set { Set(() => DrivingHints, ref drivingHints, value); }
        }

        // DO NOT CALL THIS METHOD DIRECTLY!
        internal async Task RefreshData(IConnection connectService,IWebSevices webService, 
                                      ICultureInfo cultureService, IUserSettings userService, IRepository repoService)
        {
            var tmpIn = false;
            var allDone = 1;
            var checkVal = 7;
            var done = false;
            var Username = userService.LoadSetting<string>("Username", SettingType.String);
            var Password = userService.LoadSetting<string>("Password", SettingType.String);
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
                            done = true;
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
                                    foreach (var j in w.Result.Journeys)
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
                                done = true;
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
                                done = true;
                            }
                        });
                    }
                }).ContinueWith(async (_) =>
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
                                done = true;
                            }
                        });
                    }
                }).ContinueWith(async (_) =>
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
                                done = true;
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
                                        done = true;
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
                                done = true;
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
        }
    }
}
