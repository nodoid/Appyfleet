using System.Collections.Generic;
using mvvmframework.Interfaces;
using mvvmframework.Models;
using mvvmframework.ViewModels.Common;
using System.Linq;
using System.Threading.Tasks;

namespace mvvmframework.ViewModels
{
    public class MapsViewModel : CommonJourneys
    {
        IWebSevices webService;
        IUserSettings userService;
        IRepository repoService;

        public MapsViewModel(ILocation loc, IRepository repo, ISockets sock, IWebSevices web, IUserSettings user) : base(loc, repo, sock)
        {
            webService = web;
            userService = user;
            repoService = repo;
        }

        int journeyId;
        public int JourneyId
        {
            get => journeyId;
            set { Set(() => JourneyId, ref journeyId, value);
                GetJourneyEvents(); }
        }

        JourneyData thisJourneyData;
        public JourneyData ThisJourneyData
        {
            get => thisJourneyData;
            set { Set(() => ThisJourneyData, ref thisJourneyData, value, true); LocData = value.GPSData; }
        }

        List<JourneyCoordinates> locData;
        public List<JourneyCoordinates> LocData
        {
            get => locData;
            set 
            {
                if (value != null && value.Count != 0)
                    Set(() => LocData, ref locData, value, true);
                else
                    Set(() => LocData, ref locData, GetLocData(), true);
            }
        }

        List<JourneyCoordinates> GetLocData()
        {
            var js = new List<JourneyCoordinates>();
            var done = false;
            IsBusy = true;
            Task.Run(async () => await webService.GetJourneyCoordinates(userService.LoadSetting<string>("Username", SettingType.String),
                                                                userService.LoadSetting<string>("Password", SettingType.String),
                                                                      journeyId)).ContinueWith((_) =>
            {
                if (_.IsCompleted)
                {
                    IsBusy = false;
                    if (!_.IsFaulted && !_.IsCanceled)
                    {
                        if (_.Result.Status.Success)
                        {
                            js.AddRange(_.Result.GeoCoordinates);
                            repoService.SaveData(js);
                            done = true;
                        }
                        else
                            done = true;
                    }
                    else
                        done = true;
                }
            });
            while (!done) {};
            return js;
        }

        List<EventModel> journeyEvents;
        public List<EventModel> JourneyEvents
        {
            get => journeyEvents;
            set { Set(() => JourneyEvents, ref journeyEvents, value, true); }
        }

        void GetJourneyEvents()
        {
            var notification = Notifications.FirstOrDefault(x=>x.JourneyId == JourneyId);
            if (notification != null)
                JourneyEvents = notification.Events;
        }
    }
}
