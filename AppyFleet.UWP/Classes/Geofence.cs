using System;
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;

namespace AppyFleet.UWP.Classes
{
    class ExtendedGeofenceFactory
    {
        private double _radius;

        private Geocircle _geocircle;

        private Geofence _geofence;

        private bool _singleUse;

        private MonitoredGeofenceStates _monitoredStates;

        private TimeSpan _dwellTime;

        private TimeSpan _duration;

        private DateTimeOffset _startTime;

        private string _fenceId;

        public Geofence CreateGeofence(string fenceID, double latitude, double longitude, double altitude, double radius, bool singleUse, int dwellTime, int duration)
        {
            _fenceId = fenceID;
            // Define the fence location and radius.
            BasicGeoposition position;
            position.Latitude = latitude;
            position.Longitude = longitude;
            position.Altitude = altitude;
            _radius = radius; // in meters

            // Set the circular region for geofence.
            _geocircle = new Geocircle(position, radius);

            // Remove the geofence after the first trigger.
            _singleUse = singleUse;

            // Set the monitored states.
            _monitoredStates = MonitoredGeofenceStates.Entered | MonitoredGeofenceStates.Exited | MonitoredGeofenceStates.Removed;

            // Set how long you need to be in geofence for the enter event to fire.
            _dwellTime = TimeSpan.FromSeconds(dwellTime);

            // Set how long the geofence should be active.
            _duration = TimeSpan.FromDays(duration);

            // Set up the start time of the geofence.
            _startTime = DateTime.Now;

            // Create the geofence.
            _geofence = new Geofence(_fenceId, _geocircle, _monitoredStates, _singleUse, _dwellTime, _startTime, _duration);
            return _geofence;
        }
    }
}
