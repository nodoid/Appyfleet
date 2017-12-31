using System.Diagnostics;
using Geofence.Plugin.Abstractions;

namespace NewAppyFleet.Geofence
{
    public class CrossGeofenceListener : IGeofenceListener
        {
            public void OnMonitoringStarted(string region)
            {
            Debug.WriteLine($"Monitoring started in region: {region}");
            }

            public void OnMonitoringStopped()
            {
                Debug.WriteLine("Monitoring stopped for all regions");
            }

            public void OnMonitoringStopped(string identifier)
            {
            Debug.WriteLine($"Monitoring stopped in region {identifier}");
            }

            public void OnError(string error)
            {
            Debug.WriteLine($"Error {error}");
            }

            // Note that you must call CrossGeofence.GeofenceListener.OnAppStarted() from your app when you want this method to run.
            public void OnAppStarted()
            {
                Debug.WriteLine("App started");
            }

            public void OnRegionStateChanged(GeofenceResult result)
            {
                Debug.WriteLine(result.ToString());
            }

        public void OnLocationChanged(GeofenceLocation location)
        {
            Debug.WriteLine($"Location changed : Lat={location.Latitude}, Lng={location.Longitude}");
        }
    }
}
