using System;
using Android.Content;
using Android.Telephony;
using mvvmframework;

namespace NewAppyFleet.Droid.Injected
{
    public class DeviceServices : IDeviceServices
    {
        public string GetDeviceID 
        {
            get
            {
                var deviceId = string.Empty;

                try
                {
                    var telMan = (TelephonyManager)MainActivity.Active.GetSystemService(Context.TelephonyService);
                    deviceId = telMan == null ? string.Empty : telMan.DeviceId;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception getting device ID {ex.Message}--{ex.InnerException?.Message}");
                }

                return deviceId;
            }
        }
    }
}
