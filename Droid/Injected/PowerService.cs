using System;

using Android.App;
using Android.Content;
using Android.OS;
using mvvmframework.Interfaces;

namespace NewAppyFleet.Droid.Injected
{
    public class PowerService : IPowerService
    {
        public double CurrentPower
        {
            get
            {
                var battLevel = 0D;
                try
                {
                    using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                    {
                        using (var battery = Application.Context.RegisterReceiver(null, filter))
                        {
                            var level = battery.GetIntExtra(BatteryManager.ExtraLevel, -1);
                            var scale = battery.GetIntExtra(BatteryManager.ExtraScale, -1);
                            
                            battLevel = Math.Floor(level * 100D / scale);
                        }
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Ensure you have android.permission.BATTERY_STATS");
                }
                return battLevel;
            }
        }
    }
}