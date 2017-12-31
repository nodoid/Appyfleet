// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

using AppyFleet.UWP.Classes;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Xamarin.Forms;
using System;
using System.Linq;
using mvvmframework.Models;
using System.Threading.Tasks;

namespace AppyFleet.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        IList<Geofence> _geofences;
        List<Tuple<double, double>> data;

        public MainPage()
        {
            this.InitializeComponent();

            /// TODO: Change to use AppyFleet's own Init code
            Xamarin.FormsMaps.Init("kz5PUX2i7NMGH205sMOc~K2q-p8T4_JRT6zilDG0ltg~AtbFbBXMl1cEd8ZgX6_XlCx6YBoxAeOSdMKtB7haup95cDJywrP7ZX3DEeFsFU0h");
            var bounds = ApplicationView.GetForCurrentView().VisibleBounds;
            NewAppyFleet.App.ScreenSize = new Size(bounds.Width, bounds.Height);

            LoadApplication(new NewAppyFleet.App());

            RequestLocationAccess().ConfigureAwait(true);

            Messenger.Default.Register<GeofenceData>(this, (m) =>
            {
                data = m.FenceData;
            });
        }

        async Task RequestLocationAccess()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();

            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    // Create Geolocator and define perodic-based tracking (2 second interval).
                    System.Diagnostics.Debug.WriteLine("Access to location is allowed.");
                    ConfigureGeofenceMonitor();
                    GetCurrentLocation();
                    break;

                case GeolocationAccessStatus.Denied:
                    System.Diagnostics.Debug.WriteLine("Access to location is denied.");
                    break;

                case GeolocationAccessStatus.Unspecified:
                    System.Diagnostics.Debug.WriteLine("Unspecificed error!");
                    break;
            }
        }

        private void GetCurrentLocation()
        {
            var _geolocator = new Geolocator { ReportInterval = 2000 };

            // Subscribe to the PositionChanged event to get location updates.
            _geolocator.PositionChanged += (s, e) =>
            {
                System.Diagnostics.Debug.WriteLine("Position changed: " + e.Position.Coordinate.Point.Position.Latitude + " " + e.Position.Coordinate.Point.Position.Longitude);

                //showUserLocation(e.Position.Coordinate.Point.Position.Latitude, e.Position.Coordinate.Point.Position.Longitude);
            };

            // Subscribe to StatusChanged event to get updates of location status changes.
            _geolocator.StatusChanged += (s, e) =>
            {
                System.Diagnostics.Debug.WriteLine("StatusChanged: " + e.Status);

                if (e.Status == PositionStatus.Ready)
                {
                }
            };
        }

        void ConfigureGeofenceMonitor()
        {
            _geofences = GeofenceMonitor.Current.Geofences;
            GeofenceMonitor.Current.GeofenceStateChanged += OnGeofenceStateChanged;
            GeofenceMonitor.Current.StatusChanged += OnGeofenceStatusChanged;
            CreateGeofence();
        }

        private void OnGeofenceStatusChanged(GeofenceMonitor sender, object args)
        {
            System.Diagnostics.Debug.WriteLine(sender.Status + "");
        }

        private async void OnGeofenceStateChanged(GeofenceMonitor sender, object args)
        {
            var reports = sender.ReadReports();
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                foreach (var report in reports)
                {
                    var state = report.NewState;

                    var geofence = report.Geofence;

                    if (state == GeofenceState.Removed)
                    {
                        // Remove the geofence from the geofences collection.
                        GeofenceMonitor.Current.Geofences.Remove(geofence);
                    }
                    else if (state == GeofenceState.Entered)
                    {
                        // Your app takes action based on the entered event.

                        // NOTE: You might want to write your app to take a particular
                        // action based on whether the app has internet connectivity.
                        System.Diagnostics.Debug.WriteLine("You have entered geofence!");
                        var message = $"GeoFenceEntered:{report.Geoposition.Coordinate.Point.Position.Latitude},{report.Geoposition.Coordinate.Point.Position.Longitude}";
                        Messenger.Default.Send(new NotificationMessage(message));
                    }
                    else if (state == GeofenceState.Exited)
                    {
                        // Your app takes action based on the exited event.
                        // NOTE: You might want to write your app to take a particular
                        // action based on whether the app has internet connectivity.
                        System.Diagnostics.Debug.WriteLine("You have exited geofence!");
                        Messenger.Default.Send(new NotificationMessage("GeoFenceExit"));
                    }
                }
            });
        }

        void CreateGeofence()
        {
            var extendedGeofence = new ExtendedGeofenceFactory();
            if (data != null)
            {
                foreach (var t in data)
                {
                    var createdGeofence = extendedGeofence.CreateGeofence("FENCE", t.Item1, t.Item2, 0, 40, false, 2, 1);
                    if (_geofences != null)
                        if (!_geofences.Any(geofence => geofence.Id.Equals(createdGeofence.Id)))
                            _geofences.Add(createdGeofence);
                }
            }
        }
        
    }
}
