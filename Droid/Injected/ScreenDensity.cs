using System;
using Android.Util;
using mvvmframework;
using NewAppyFleet.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(ScreenDensity))]
namespace NewAppyFleet.Droid
{
    public class ScreenDensity : IScreenDensity
    {
        public DisplayDensity GetScreenDensity
        {
            get
            {
                var density = DisplayDensity.Unknown;
                var metrics = new DisplayMetrics();
                MainActivity.Active.WindowManager.DefaultDisplay.GetMetrics(metrics);
                switch(metrics.DensityDpi)
                {
                    case DisplayMetricsDensity.Low:
                        density = DisplayDensity.LDPI;
                        break;
                    case DisplayMetricsDensity.Medium:
                        density = DisplayDensity.MDPI;
                        break;
                    case DisplayMetricsDensity.High:
                        density = DisplayDensity.HDPI;
                        break;
                    case DisplayMetricsDensity.Xhigh:
                        density = DisplayDensity.XHDPI;
                        break;
                    case DisplayMetricsDensity.Xxhigh:
                        density = DisplayDensity.XXHDPI;
                        break;
                    case DisplayMetricsDensity.Xxxhigh:
                        density = DisplayDensity.XXXHDPI;
                        break;
                }
                return density;
            }
        }
    }
}
