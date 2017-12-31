
using Android.App;
using Android.Content;
using Android.OS;
using Android.Content.PM;

namespace NewAppyFleet.Droid
{
    [Activity(Label = "Appy Fleet", MainLauncher = true, NoHistory = true, Theme = "@style/Theme.Splash",
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }
    }
}