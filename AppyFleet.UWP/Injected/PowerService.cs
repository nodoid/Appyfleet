using mvvmframework.Interfaces;
using Windows.Devices.Power;

namespace AppyFleet.UWP.Injected
{
    public class PowerService : IPowerService
    {
        public double CurrentPower
        {
            get
            {
                var battery = Battery.AggregateBattery;
                var report = battery.GetReport();
                var finalPercent = -1;

                if (report.RemainingCapacityInMilliwattHours.HasValue && report.FullChargeCapacityInMilliwattHours.HasValue)
                {
                    finalPercent = (int)((report.RemainingCapacityInMilliwattHours.Value /
                        (double)report.FullChargeCapacityInMilliwattHours.Value) * 100);
                }
                return finalPercent;
            }
        }
    }
}
