using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using mvvmframework.Interfaces;
using mvvmframework.Models;
using GalaSoft.MvvmLight.Ioc;

namespace mvvmframework.ViewModels
{
    public class BaseLocationViewModel : BaseViewModel
    {
        ILocation locService;
        IRepository repoService;
        ILogFileService logService { get; set; } = SimpleIoc.Default.GetInstance<ILogFileService>();
        IPowerService powerService { get; set; } = SimpleIoc.Default.GetInstance<IPowerService>();
        IJourneyService journeyService { get; set; } = SimpleIoc.Default.GetInstance<IJourneyService>();
        IDeviceServices deviceService { get; set; } = SimpleIoc.Default.GetInstance<IDeviceServices>();
        IInstallData installService { get; set; } = SimpleIoc.Default.GetInstance<IInstallData>();

        public BaseLocationViewModel(ILocation loc, IRepository repo, ISockets sock)
        {
            locService = loc;
            repoService = repo;

            Messenger.Default.Register<NotificationMessage<LocationServiceData>>(this, (message) =>
            {
                // Gets the message object.
                var data = message.Content;

                // Checks the associated action.
                switch (message.Notification)
                {
                    case "NewLocation":
                        GetAndProcessNewLocation();
                        break;
                    case "StartLocation":
                        GetAndProcessLocation(true);
                        break;
                    case "EndLocation":
                        GetAndProcessLocation();
                        break;
                    default:
                        break;
                }
            });

            Messenger.Default.Register<NotificationMessage<IJourneyService>>(this, (message) =>
            {
                var data = message.Content;
                switch (message.Notification)
                {
                    case "JourneyEndedOK":
                        break;
                    case "JourneyEndedNR":
                        break;
                    case "JourneyEndedUK":
                        break;
                }
            });
        }

        long JourneyId { get; set; }

        int DataPoint { get; set; }

        int RecordNumber { get; set; }

        LocationServiceData locPoint { get; set; }

        public void InitialiseService()
        {
            journeyService.InitialiseService(MessageQueue, deviceService.GetDeviceID, Driver.OdoReading, locService.GpsStatus, PairId, RecordNumber, AppStarted, JourneyCount, installService.VersionNumber);
        }

        void StoreSQL()
        {
            repoService.SaveData(JourneyData);
            repoService.SaveData(JourneyData.GPSData);
        }

        void GetAndProcessLocation(bool start = false)
        {
            if (JourneyData == null)
                JourneyData = new JourneyData();

            if (start)
            {
                JourneyData.JourneyStartDate = DateTime.Now;
                JourneyId = JourneyData.JourneyId = JourneyData.JourneyNumber =repoService.Count<JourneyData>();
                var data = locService.GetLocationData;
                data.id = (int)JourneyId;
                data.EventName = "ST";
                data.datapoint = DataPoint = 0;
                JourneyData.GPSData = new List<JourneyCoordinates> { new JourneyCoordinates { Latitude = data.Latitude, Longitude = data.Longitude, JourneyId = data.id } };
                locPoint = data;
                StoreSQL();

                logService.WriteLog("JourneyManager:StartJourney", "Pending journey start logged");

                try
                {
                    if (powerService.CurrentPower < 30)
                    {
                        logService.WriteLog("JourneyManager:StartJourney", "User started journey with power save mode enabled. Unstable results expected.");
                    }
                }
                catch (Exception ex)
                {
                    logService.WriteLog("JourneyManager:StartJourney", "Error in Setting Power Service");
                    logService.WriteLog("JourneyManager:StartJourney", ex.Message);
                }
            }
            else
            {
                JourneyData.JourneyEndDate = DateTime.Now;
                var data = locService.GetLocationData;
                data.id = (int)JourneyId;
                data.EventName = "OFF";
                data.datapoint = DataPoint;
                JourneyData.GPSData.Add(new JourneyCoordinates { Latitude = data.Latitude, Longitude = data.Longitude, JourneyId = data.id });
                StoreSQL();
                MessageQueue = journeyService.EndJourney();
                logService.WriteLog("JourneyManager:EndJourney", "Journey ended");

                if (powerService.CurrentPower < 30)
                {
                    logService.WriteLog("Journey failed to record", "No location could be found. Please ensure sure that power saving mode on your device is disabled whilst driving.");
                }

                var locationsForSave = new List<LocationDetails>();
                lock (new object())
                {
                    JourneyData.GPSData.ForEach(j => locationsForSave.Add(new LocationDetails(j.Latitude, j.Longitude,
                        (new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds(DateTime.Now.TimeOfDay.TotalMilliseconds).ToLocalTime())));
                    StoreSQL();
                    JourneyData.GPSData.Clear();
                }
            }
        }

        void GetAndProcessNewLocation()
        {
            if (DataPoint < 2)
                return;

            if (journeyService.AddLocationUpdate())
                RecordNumber++;

            DataPoint++;

            StoreSQL();
        }


    }
}
