using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Plugin.CurrentActivity;

namespace NewAppyFleet.Droid
{
    //You can specify additional application information in this attribute
    [Application]
    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
        public static Context AppContext;

        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          :base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);
            /*AppContext = this.ApplicationContext;
            CrossGeofence.Initialize<CrossGeofenceListener>();
            StartService();*/
            //A great place to initialize Xamarin.Insights and Dependency Services!
        }

        /*public static void StartService()
        {
            AppContext.StartService(new Intent(AppContext, typeof(GeofenceService)));

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            {
                var pintent = PendingIntent.GetService(AppContext, 0, new Intent(AppContext, typeof(GeofenceService)), 0);
                var alarm = (AlarmManager)AppContext.GetSystemService(Context.AlarmService);
                alarm.Cancel(pintent);
            }
        }

        public static void StopService()
        {
            AppContext.StopService(new Intent(AppContext, typeof(GeofenceService)));
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            {
                var pintent = PendingIntent.GetService(AppContext, 0, new Intent(AppContext, typeof(GeofenceService)), 0);
                var alarm = (AlarmManager)AppContext.GetSystemService(Context.AlarmService);
                alarm.Cancel(pintent);
            }
        }*/

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }
    }
}