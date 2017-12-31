using System;
using System.Reflection;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using GalaSoft.MvvmLight.Ioc;
using mvvmframework;
using mvvmframework.Interfaces;
using NewAppyFleet.Droid.Injected;
using Plugin.Permissions;
using Xamarin.Forms;

namespace NewAppyFleet.Droid
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        public static Activity Active { get; private set; }

        public static ISharedPreferences Prefs { get; set; }

        static string[] PERMISSIONS =
        {
            Manifest.Permission_group.Location,
            Manifest.Permission_group.Sms,
            Manifest.Permission_group.Storage,
            Manifest.Permission_group.AffectsBattery,
            Manifest.Permission_group.Phone,
            Manifest.Permission.Bluetooth,
            Manifest.Permission.BluetoothAdmin
        }; // add battery stats

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnLowMemory()
        {
            base.OnLowMemory();
            GC.Collect();
        }

        protected override void OnCreate(Bundle bundle)
        {
            Prefs = GetSharedPreferences("appyfleet", FileCreationMode.Private);

            Active = this;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);
            OxyPlot.Xamarin.Forms.Platform.Android.PlotViewRenderer.Init();

            // carousel view

            var cv = typeof(Xamarin.Forms.CarouselView);
            var assembly = Assembly.Load(cv.FullName);

            // ioc registration
            SimpleIoc.Default.Register<IInstallData, InstallData>();
            SimpleIoc.Default.Register<IDeviceServices, DeviceServices>();
            SimpleIoc.Default.Register<IEmailSms, EmailSms>();
            SimpleIoc.Default.Register<IExpenses, Expenses>();
            SimpleIoc.Default.Register<IPowerService, PowerService>();
            SimpleIoc.Default.Register<ISockets, SocketService>();
            SimpleIoc.Default.Register<IFileSystem, FileSystem>();

            App.ScreenSize = new Size(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density,
                Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);

            HockeyApp.Android.Metrics.MetricsManager.Register(Application, Constants.HOCKEY_APP_APP_ID);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                // request permissions explicitly for Android 6+
                if (ContextCompat.CheckSelfPermission(Forms.Context, Manifest.Permission.BluetoothAdmin) != (int)Permission.Granted)
                {
                    RequestBluetoothAdminPermission();
                }
                if (ContextCompat.CheckSelfPermission(Forms.Context, Manifest.Permission.Bluetooth) != (int)Permission.Granted)
                {
                    RequestBluetoothPermission();
                }
                if (ContextCompat.CheckSelfPermission(Forms.Context, Manifest.Permission_group.AffectsBattery) != (int)Permission.Granted)
                {
                    RequestBatteryPermission();
                }
                if (ContextCompat.CheckSelfPermission(Forms.Context, Manifest.Permission_group.Location) != (int)Permission.Granted)
                {
                    RequestLocationPermission();
                }
                if (ContextCompat.CheckSelfPermission(Forms.Context, Manifest.Permission_group.Storage) != (int)Permission.Granted)
                {
                    RequestStoragePermission();
                }
                if (ContextCompat.CheckSelfPermission(Forms.Context, Manifest.Permission_group.Phone) != (int)Permission.Granted)
                {
                    RequestPhonePermission();
                }
                if (ContextCompat.CheckSelfPermission(Forms.Context, Manifest.Permission_group.Sms) != (int)Permission.Granted)
                {
                    RequestSmsPermission();
                }

                LoadApplication(new App());
            }

            void RequestSmsPermission()
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission_group.Sms }, 0);
            }

            void RequestBluetoothPermission()
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.Bluetooth }, 1);
            }

            void RequestBluetoothAdminPermission()
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.BluetoothAdmin }, 2);
            }

            void RequestPhonePermission()
            {
                    ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission_group.Phone }, 3);
            }

            void RequestBatteryPermission()
            {
                    ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission_group.AffectsBattery }, 4);
            }

            void RequestStoragePermission()
            {
                    ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission_group.Storage }, 5);
            }

            void RequestLocationPermission()
            {
                    ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission_group.Location }, 6);
            }
        }
    }
}
