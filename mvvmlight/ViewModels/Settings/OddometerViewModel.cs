using System;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mvvmframework.ViewModels.Settings
{
    public class OddometerViewModel : SettingsViewModel
    {
        public OddometerViewModel(IUserSettings settings, IWebSevices web, IConnection con) : base (settings, web, con)
        {
        }

        public void GetOdoReadings()
        {
            Task.Run(async () =>
            {
                IsBusy = true;
                await webSevice.GetOdoReadings(Username, Password, VehicleId).ContinueWith((t) =>
                {
                    if (t.IsCompleted)
                    {
                        IsBusy = false;
                        if (!t.IsCanceled && !t.IsFaulted)
                        {
                            Readings = t.Result.Readings;
                        }
                    }
                });
            });
        }

        List<OdoReadingModel> readings;
        public List<OdoReadingModel> Readings
        {
            get => readings;
            set { Set(() => Readings, ref readings, value, true); }
        }

        DateTime dateAdded;
        public DateTime DateAdded
        {
            get { return dateAdded; }
            set { Set(() => DateAdded, ref dateAdded, value); }
        }

        TimeSpan timeAdded;
        public TimeSpan TimeAdded
        {
            get => timeAdded;
            set { Set(() => TimeAdded, ref timeAdded, value); }
        }

        double reading;
        public double Reading
        {
            get { return reading; }
            set { Set(() => Reading, ref reading, value); }
        }

        bool showDate;
        public bool ShowDate
        {
            get => showDate;
            set { Set(() => ShowDate, ref showDate, value, true); }
        }

        bool showTime;
        public bool ShowTime
        {
            get => showTime;
            set { Set(() => ShowTime, ref showTime, value, true); }
        }

        public void RemoveOdo(int id)
        {
            var item = Readings.FirstOrDefault(t => t.ID == id);
            if (item != null)
                Readings.Remove(item);
        }

        RelayCommand btnAddOdometer;
        public RelayCommand BtnAddOdometer => btnAddOdometer ??
        (
            btnAddOdometer = new RelayCommand(async () =>
        {
            if (connectService.IsConnected)
            {
                IsBusy = true;
                var odometer = new OdoReadingModel
                {
                    DateRead = new DateTime(DateAdded.Year, DateAdded.Month, DateAdded.Day, TimeAdded.Hours, TimeAdded.Minutes, TimeAdded.Seconds),
                    Reading = Reading,
                    SelectedVehicle = VehicleModels.FirstOrDefault(w => w.Id == CurrentVehicleId),
                };
                await webSevice.AddOdoReading(Username, Password, odometer).ContinueWith((t) =>
                {
                    if (t.IsCompleted)
                    {
                        IsBusy = false;
                        if (!t.IsCanceled && !t.IsFaulted)
                        {
                            // add to the odometer list
                            Readings.Insert(0, odometer);
                        }
                    }
                });
            }
        })
        );
    }
}
