using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mvvmframework.Enums;
using mvvmframework.Interfaces;
using mvvmframework.Models;
using mvvmframework.ViewModels.Common;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using mvvmframework.Helpers;

namespace mvvmframework.ViewModels
{
    public class JourneysViewModel : CommonJourneys
    {
        IConnection connectService;
        IWebSevices webService;
        IRepository repoService;
        IUserSettings userService;
        ICultureInfo cultureInfo;

        public JourneysViewModel(IWebSevices web, IConnection connect, IRepository repo, IUserSettings user, 
                                 ICultureInfo cult, ILocation loc, ISockets sock) : base (loc, repo, sock)
        {
            connectService = connect;
            webService = web;
            repoService = repo;
            userService = user;
            cultureInfo = cult;

            JourneyType = JourneyTypes.All;
            SortedJourneys = new ObservableCollection<JourneyDetails>();
            Journeys = repoService.GetList<DBJourneyModel>();
        }

        bool showPrivate;
        public bool ShowPrivate
        {
            get => showPrivate;
            set => Set(() => ShowPrivate, ref showPrivate, value, true);
        }

        public async Task GetNotifications()
        {
            if (connectService.IsConnected)
            {
                IsBusy = true;
                await webService.GetNotifications(userService.LoadSetting<string>("Username", SettingType.String),
                                                  userService.LoadSetting<string>("Password", SettingType.String)).ContinueWith((t) =>
                {
                    if (t.IsCompleted)
                    {
                        IsBusy = false;
                        if (!t.IsFaulted && !t.IsCanceled)
                            Notifications = t.Result.Notifications;
                        else
                            Messenger.Default.Send(new NotificationMessage(t.Result.Status.Message));
                    }
                });
            }
        }

        public JourneyModel FindMatchingJourneyDetails(JourneyModel journey)
        {
            var journeys = Journeys.FirstOrDefault(j => j.JourneyNumber == journey.JourneyNumber);

            var dbModel = default(JourneyModel);

            if (journeys != null)
            {
                var j = journeys;
                dbModel = new JourneyModel(cultureInfo.currentCulture)
                {
                    EndDate = j.EndDate,
                    EndLocation = j.EndLocation,
                    Id = j.Id,
                    PolicyId = j.PolicyId,
                    JourneyId= j.JourneyId,
                    JourneyNumber = j.JourneyNumber,
                    StartLocation = j.StartLocation,
                    Miles = j.Miles,
                    Nickname = j.Nickname,
                    OverallScore = j.OverallScore,
                    Private = j.Private,
                    StartDate = j.StartDate,
                    SpeedScore = j.SpeedScore,
                    SmoothScore = j.SmoothScore,
                    UsageScore = j.UsageScore,
                };
            }

            return dbModel;
        }

        public bool MapJourneyDataAvalible(int journeyNumber)
        {
            return Journeys?.Find(j => j.JourneyNumber == journeyNumber) != null;
        }

        public ObservableCollection<DBJourneyModel> GetJourneysData
        {
            get
            {
                var sj = SortedJourneys;
var dn = new ObservableCollection<DBJourneyModel>();
                if (sj != null)
                {
                    
                    foreach (var s in sj)
                        foreach (var j in s.Journey)
                            dn.Add(j);
                }
                return dn;
            }
        }

        ObservableCollection<JourneyDetails> sortedJourneys;
        public ObservableCollection<JourneyDetails> SortedJourneys
        {
            get => sortedJourneys;
            set { Set(() => SortedJourneys, ref sortedJourneys, value, true);}
        }

        public void CreateSortedJourneys()
        {
            var dates = Journeys.DistinctBy(w => w.StartDate.Date).ToList();
            var sjourney = new ObservableCollection<JourneyDetails>();
            foreach (var d in dates)
            {
                sjourney.Add(new JourneyDetails
                {
                    JourneyDateTime = d.StartDate.ToString("D"),
                    Journey = ShowPrivate ? Journeys.Where(w => w.StartDate.Date == d.StartDate.Date).Where(w => w.JourneyType.ToLowerInvariant() == "private").ToList().ToObservableCollection() :
                                                    Journeys.Where(w => w.StartDate.Date == d.StartDate.Date).ToObservableCollection()               
                });
            }
            SortedJourneys = sjourney;
        }

        public async Task RefreshJourneyData()
        {
            await GetJourneyData().ContinueWith((t) =>
            {
                if (t.IsCompleted)
                {
                    if (!t.IsFaulted && !t.IsCanceled)
                    {
                        var dates = Journeys.DistinctBy(w => w.StartDate.Date).ToList();
                        var sjourney = new ObservableCollection<JourneyDetails>();
                        foreach(var d in dates)
                        {
                            sjourney.Add(new JourneyDetails
                            {
                                JourneyDateTime = d.StartDate.ToString("D"),
                                Journey = ShowPrivate ? Journeys.Where(w => w.StartDate.Date == d.StartDate.Date).Where(w => w.JourneyType.ToLowerInvariant() == "private").ToList().ToObservableCollection() :
                                                    Journeys.Where(w => w.StartDate.Date == d.StartDate.Date).ToList().ToObservableCollection()
                            });
                        }
                        SortedJourneys = sjourney;
                    }
                }
            });
        }

        public async Task GetJourneyData()
        {
            if (connectService.IsConnected)
            {
                IsBusy = true;
                await webService.GetJourneys(userService.LoadSetting<string>("Username", SettingType.String),
                                             userService.LoadSetting<string>("Password", SettingType.String)).ContinueWith((t) =>
                {
                if (t.IsCompleted)
                {
                    IsBusy = false;
                    if (!t.IsFaulted && !t.IsCanceled)
                    {
                        var db = t.Result.Journeys;
                            var currentDb = repoService.GetList<DBJourneyModel>();
                            var newdb = new List<DBJourneyModel>();
                            foreach(var d in db)
                            {
                                newdb.Add(new DBJourneyModel
                                {
                                    EndDate = d.EndDate,
                                    EndLocation = d.EndLocation,
                                    Id = d.Id,
                                    PolicyId = d.PolicyId,
                                    JourneyId = d.JourneyId,
                                    JourneyNumber = d.JourneyNumber,
                                    StartLocation = d.StartLocation,
                                    StartDate = d.StartDate,
                                    SpeedScore = d.SpeedScore,
                                    SmoothScore = d.SmoothScore,
                                    UsageScore = d.UsageScore, 
                                    OverallScore =d.OverallScore,
                                });
                            }
                            var diff = newdb.Where(r => !currentDb.Any(p => p.JourneyId == r.JourneyId)).ToList();
                            repoService.SaveData(diff);
                            Journeys = newdb;
                            CreateSortedJourneys();
                        }
                    }
                else
                    {
                        Messenger.Default.Send(new NotificationMessage(t.Result.Status.Message));
                    }
                });
            }
        }
    }
}
