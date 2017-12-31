using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mvvmframework.Enums;
using mvvmframework.Interfaces;
using mvvmframework.Languages;
using mvvmframework.Models;
using mvvmframework.ViewModels.Common;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Messaging;

namespace mvvmframework.ViewModels
{
    public class DashboardViewModel : CommonJourneys
    {
        IWebSevices webServices;
        IRepository repoService;
        IUserSettings userService;
        IConnection connectService;
        IFileSystem fileService;

        public DashboardViewModel(IWebSevices web, IRepository repo, IUserSettings user,
                                  IConnection connect, ILocation loc, ISockets sock, IFileSystem file)
            : base(loc, repo, sock)
        {
            webServices = web;
            repoService = repo;
            userService = user;
            connectService = connect;
            fileService = file;

            CurrentSearch = RefineSearch.CalendarMonth;
        }

        public void CreateDashboardModel()
        {
             var lm = new DashboardListModel
            {
                Registration = Driver.OdoVehicleReg,
                Odo = Driver.OdoReading.ToString(),
                Score = Driver.Score,
                RotateAngle = SpinnerAngle,
                TotalMiles = DriverScores.PersonalMileage + DriverScores.BusinessMileage,
                PersonalMiles = DriverScores.PersonalMileage,
                BusinessMiles = DriverScores.BusinessMileage,
                NotificationCount = Notifications.Count,
            };

            if (JourneysList != null)
            {
                if (lm.LatestJourney == null)
                    lm.LatestJourney = new DBJourneyModel();
                if (JourneysList.Count >0)
                lm.LatestJourney = JourneysList[0];
            }

            DashboardModel = new List<DashboardListModel> { lm };
        }

        bool driScore;
        public bool DriScore
        {
            get => driScore;
            set { Set(() => DriScore, ref driScore, value, true); }
        }

        void UpdateScores()
        {
            if (CurrentCarousel > -1 && CurrentCarousel < GroupScores.Count)
            {
                CurrentGroupScore = GroupScores[CurrentCarousel];
                CurrentScore = CurrentGroupScore.Score.ToString("F1");
                CurrentName = CurrentGroupScore.GroupName;
                CurrentRankOutOf = CurrentGroupScore.RankOutOf.ToString();
                CurrentId = CurrentGroupScore.GroupId;
                CurrentScoreDifference = Driver.Score - CurrentGroupScore.Score;
                if (CurrentCarousel > 0)
                    GoBackEnabled = true;
                else
                    GoBackEnabled = false;
                if (CurrentCarousel < GroupScores.Count)
                    GoForwardEnabled = true;
                else
                    GoForwardEnabled = false;
            }
        }

        bool goBackEnabled;
        public bool GoBackEnabled
        {
            get => goBackEnabled;
            set { Set(() => GoBackEnabled, ref goBackEnabled, value, true); }
        }

        bool goForwardEnabled;
        public bool GoForwardEnabled
        {
            get => goForwardEnabled;
            set { Set(() => GoForwardEnabled, ref goForwardEnabled, value, true); }
        }

        string currentName;
        public string CurrentName
        {
            get => currentName;
            set { Set(() => CurrentName, ref currentName, value, true); }
        }

        string currentRankOutOf;
        public string CurrentRankOutOf
        {
            get => currentRankOutOf;
            set { Set(() => CurrentRankOutOf, ref currentRankOutOf, value, true); }
        }

        string currentRank;
        public string CurrentRank
        {
            get => currentRank;
            set { Set(() => CurrentRank, ref currentRank, value, true); }
        }

        int currentCarousel;
        public int CurrentCarousel
        {
            get => currentCarousel;
            set
            {
                Set(() => CurrentCarousel, ref currentCarousel, value, true);
                UpdateScores();
            }
        }

        string currentScore;
        public string CurrentScore
        {
            get => currentScore;
            set { Set(() => CurrentScore, ref currentScore, value, true); }
        }

        int currentId;
        public int CurrentId
        {
            get => currentId;
            set { Set(() => CurrentId, ref currentId, value, true); }
        }

        double currentScoreDifference;
        public double CurrentScoreDifference
        {
            get => currentScoreDifference;
            set { Set(() => CurrentScoreDifference, ref currentScoreDifference, value, true); }
        }

        DriverGroupScoreModel currentGroupScore;
        public DriverGroupScoreModel CurrentGroupScore
        {
            get => currentGroupScore;
            set { Set(() => CurrentGroupScore, ref currentGroupScore, value, true); }
        }

        List<DashboardListModel> dashboardModel;
        public List<DashboardListModel> DashboardModel
        {
            get => dashboardModel;
            set { Set(() => DashboardModel, ref dashboardModel, value, true); }
        }

        public string GetDashboardPdfUrl => Driver.NotRegisteredPdf;

        public string GetFindOutMoreUrl => @"https://appyfleet-wp-prod.azurewebsites.net/register/";

        public string GetEmailAddress => @"sales@appyfleet.co.uk";

        public string GetTermsFilename => TermsConditions.Split('/').Last();

        public string GetDrivingFilename => DrivingHints.Split('/').Last();

        public string GetUserGuideFilename => UserGuide.Split('/').Last();

        public bool FileExists(string filename)
        {
            return fileService.FileExists(filename);
        }

        public async Task GetPDFFile(string url, string filename)
        {
            await fileService.DownloadFile(url, filename);
        }

        public double SpinnerAngle
        {
            get
            {
                if (DriverScores == null)
                    return 0;

                return DriverScores.Score > 0 ? DriverScores.Score < 5 ? ((360 - Constants.DashBoardControlsScoreGap) / 10.0) * DriverScores.Score
                    : ((360 - Constants.DashBoardControlsScoreGap) / 10.0) * (DriverScores.Score - 5.0f)
                    : ((360 - Constants.DashBoardControlsScoreGap) / 20.0) * DriverScores.Score;
            }
        }

        public Mileage GetMileage => new Mileage { PersonalMiles = (int)personalMiles, TotalMiles = (int)totalMiles, Title = Langs.Const_Label_Total_Mileage };

        RefineSearch currentSearch;
        public RefineSearch CurrentSearch
        {
            get { return currentSearch; }
            set
            {
                Set(() => CurrentSearch, ref currentSearch, value, true);
                Current = value;
            }
        }

        public void ChangeCalendar()
        {
            var _ = JourneysList;
            Task.Run(async () =>
            {
                await GetDriverScores().ContinueWith(async(t)=>
                {
                    if (t.IsCompleted)
                    {
                        await GetDriverGroupScores().ContinueWith((r) =>
                        {
                            if (r.IsCompleted)
                                CreateDashboardModel();
                        });
                    }
                });
            });
        }

        List<DriverGroupScoreModel> groupScores;
        public List<DriverGroupScoreModel> GroupScores
        {
            get => groupScores;
            set { Set(() => GroupScores, ref groupScores, value, true); }
        }

        string Username => userService.LoadSetting<string>("Username", SettingType.String);
        string Password => userService.LoadSetting<string>("Password", SettingType.String);

        async Task GetDriverScores()
        {
            IsBusy = true;
            await webServices.GetDriverScores(Username, Password, (int)CurrentSearch).ContinueWith((w) =>
            {
                if (w.IsCompleted)
                {
                    IsBusy = false;
                    if (!w.IsFaulted && !w.IsCanceled)
                    {
                        DriverScores = w.Result;
                        Driver.Score = w.Result.Score;
                        repoService.SaveData(DriverScores);
                    }
                }
            });
        }

        public async Task GetDriverGroupScores()
        {
            IsBusy = true;
            await webServices.GetDriverGroupScores(Username, Password, (int)CurrentSearch).ContinueWith((t) =>
            {
                if (t.IsCompleted)
                {
                    IsBusy = false;
                    if (!t.IsFaulted && !t.IsCanceled)
                    {
                        if (t.Result.Status.Success)
                        {
                            GroupScores = t.Result.Data;
                        }
                        else
                            Messenger.Default.Send(new NotificationMessage(t.Result.Status.Message));
                    }
                }
            });
        }

        public string OdoFormattedValue => string.Format("{0:n0}", Driver?.OdoReading);

        public string OdoVehicleReg => Driver?.OdoVehicleReg;

        public List<DBJourneyModel> GetLatestJourney => new List<DBJourneyModel> { JourneysList.FirstOrDefault() };

        DBJourneyModel latestJourny;
        public DBJourneyModel LatestJourney
        {
            get => latestJourny;
            set { Set(() => LatestJourney, ref latestJourny, value, true); }
        }

        double totalMiles;
        public double TotalMiles
        {
            get => totalMiles;
            set { Set(() => TotalMiles, ref totalMiles, value, true); }
        }

        double personalMiles;
        public double PersonalMiles
        {
            get => personalMiles;
            set { Set(() => PersonalMiles, ref personalMiles, value, true); }
        }

        public void GetDriverData()
        {
            var tot = 0D;
            var pm = 0D;
            foreach (var l in JourneysList)
            {
                tot += l.Miles;
                if (l.JourneyType != "Business")
                    pm += l.Miles;
            }
            TotalMiles = tot;
            PersonalMiles = pm;
        }
    }
}
