using System;
using System.Diagnostics;
using System.Globalization;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using mvvmframework;
using mvvmframework.Interfaces;
using mvvmframework.Languages;
using mvvmframework.Models;
using mvvmframework.ViewModel;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Xamarin.Forms;
using NewAppyFleet.Views;
using Plugin.LocalNotifications;
using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Linq;

namespace NewAppyFleet
{
    public static class Culture
    {
        public static CultureInfo currentCulture { get; set; }
    }

    public class CulturalInfo : ICultureInfo
    {
        public CultureInfo currentCulture
        {
            get => Culture.currentCulture;
            set { Culture.currentCulture = value; }
        }

        public string currentCultureString => Culture.currentCulture.TwoLetterISOLanguageName;
    }

    public class NetworkConnection : IConnection
    {
        public bool IsConnected
        {
            get
            {
                return CrossConnectivity.Current.IsConnected;
            }
        }
    }

    public class Geolocation : ILocation
    {
        LocationServiceData locData;

        public Geolocation()
        {
            GpsStatus = false;
        }

        public Geolocation(LocationServiceData loc, bool on)
        {
            locData = loc;
            GpsStatus = on;
        }

        public LocationServiceData GetLocationData => locData;

        public bool GpsStatus { get; set; }
    }

    public class App : Application
    {
        public static Size ScreenSize { get; set; }
        public static App Self { get; private set; }

        public static ViewModelLocator locator;
        public static ViewModelLocator Locator { get { return locator ?? (locator = new ViewModelLocator()); } }

        public bool IsConnected { get; private set; }

        public bool GeolocatorListening { get; set; }

        public DisplayDensity Density { get; set; }

        public bool PanelShowing { get; set; }

        public DateTime AppStarted { get; private set; } = DateTime.Now;

        public void StartListening()
        {
            if (!CrossGeolocator.Current.IsListening && Locator.Dashboard.LocationOn)
            {
                CrossGeolocator.Current.DesiredAccuracy = 5;
                CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromMinutes(1), 5, true).ContinueWith((t)=>
                {
                    if (t.IsCompleted)
                    {
                        if (!t.IsFaulted && !t.IsCanceled)
                        {
                            Locator.Dashboard.GpsOn = true;
                            GeolocatorListening = true;
                            CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromMinutes(1), null, true).ContinueWith((w)=>
                            {
                                if (w.IsCompleted)
                                {
                                    var loc = new LocationServiceData
                                    {
                                        Accuracy = w.Result.Accuracy,
                                        Altitude = w.Result.Altitude,
                                        AltitudeAccuracy = w.Result.AltitudeAccuracy,
                                        Heading = w.Result.Heading,
                                        Latitude = w.Result.Latitude,
                                        Longitude = w.Result.Longitude,
                                        Speed = w.Result.Speed,
                                        TimeStamp = w.Result.Timestamp
                                    };
                                    var _ = new Geolocation(loc, true);
                                    Messenger.Default.Send(new NotificationMessage("$StartLocation"));
                                }
                            });
                        }
                    }
                });
                CrossGeolocator.Current.PositionChanged += (object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e) => 
                {
                    var loc = new LocationServiceData
                    {
                        Accuracy = e.Position.Accuracy,
                        Altitude = e.Position.Altitude,
                        AltitudeAccuracy = e.Position.AltitudeAccuracy,
                        Heading = e.Position.Heading,
                        Latitude = e.Position.Latitude,
                        Longitude = e.Position.Longitude,
                        Speed = e.Position.Speed,
                        TimeStamp = e.Position.Timestamp
                    };
                    var _ = new Geolocation(loc, true);
                    Messenger.Default.Send(new NotificationMessage("$NewLocation"));
                    Messenger.Default.Send(new NotificationMessage($"%{loc.Speed}"));
                };
            }
        }

        public void StopListening()
        {
            {
                GeolocatorListening = false;
                CrossGeolocator.Current.StopListeningAsync().ContinueWith((t) =>
                {
                    if (t.IsCompleted)
                    {
                        GeolocatorListening = false;
                        var _ = new Geolocation(null, false);
                        Messenger.Default.Send(new NotificationMessage("$EndLocation"));
                    }
                });
            }
        }

        public App()
        {
            App.Self = this;

            var netLanguage = DependencyService.Get<ILocalize>().GetCurrent();
            Culture.currentCulture = new CultureInfo(netLanguage);
            Langs.Culture = new CultureInfo(netLanguage);
            DependencyService.Get<ILocalize>().SetLocale();

            try
            {
                SimpleIoc.Default.Register<ICultureInfo>(() => new CulturalInfo());
            }
            catch
            {
#if DEBUG
                Debug.WriteLine("cultureinfo restarted exception");
#endif
            }

            if (Device.RuntimePlatform == Device.Android)
            {
                Density = DependencyService.Get<IScreenDensity>().GetScreenDensity;
            }

            // Register the SQL
            DependencyService.Get<ISqLiteConnectionFactory>().GetConnection();

             try
            {
                SimpleIoc.Default.Register<IUserSettings>(() => DependencyService.Get<IUserSettings>());
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine("usersettings restarted exception : {0}--{1}", ex.Message, ex.InnerException);
#endif
            }

            IsConnected = CrossConnectivity.Current.IsConnected;

            try
            {
                SimpleIoc.Default.Register<IConnection>(() => new NetworkConnection());
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine("connection restarted exception : {0}--{1}", ex.Message, ex.InnerException);
#endif
            }
            
            var dialogService = new DialogService();
            try
            {
                SimpleIoc.Default.Register<IDialogService>(() => dialogService);
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine("dialogservice restarted exception : {0}--{1}", ex.Message, ex.InnerException);
#endif
            }
try
                        {
                            SimpleIoc.Default.Register<ILocation>(() => new Geolocation());
                        }
                        catch (Exception ex)
                        {
#if DEBUG
                            Debug.WriteLine("geolocation restarted exception : {0}--{1}", ex.Message, ex.InnerException);
#endif
                        }

            Page firstPage = null;
            if (Locator.Dashboard.LoggedIn)
                firstPage = new DashboardPage();
            else
            firstPage = new LoginPage();

            dialogService.Initialize(firstPage);

            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                Locator.Dashboard.IsConnected = IsConnected = args.IsConnected;
            };

            Locator.Dashboard.IsConnected = IsConnected = CrossConnectivity.Current.IsConnected;
            Locator.Dashboard.AppStarted = App.Self.AppStarted;
            Locator.Dashboard.Offset = new DateTimeOffset().Offset;
            if (Device.RuntimePlatform == Device.Windows)
                Locator.Dashboard.IsConnected = true; /// HACK!
                     
            MainPage = new NavigationPage(firstPage);

            try
            {
                if (Device.RuntimePlatform != Device.Windows)
                {
                    SimpleIoc.Default.Register<IBluetoothDevices>(() => new BluetoothServices());
                }
            }
            catch
            {
#if DEBUG
                Debug.WriteLine("bluetooth restarted exception");
#endif
            }


            Messenger.Default.Register<NotificationMessage>(this, (m) =>
            {
                var message = m.Notification;
                if (m.Notification.StartsWith("$", StringComparison.CurrentCulture))
                {
                    CrossLocalNotifications.Current.Show("", message.Remove(0, 1));
                }
                else
                {
                    if (m.Notification.Contains("%"))
                    {
                        Locator.Login.CurrentSpeed = Convert.ToDouble(m.Notification.TrimStart('%'));
                    }
                    else
                    {
                        if (m.Notification.Contains("BLE"))
                        {
                            if (m.Notification.Contains("discon") || m.Notification.Contains("lost"))
                                CrossLocalNotifications.Current.Show("", Langs.Const_Label_Turn_On_Bluetooth);
                        }
                        else
                            Device.BeginInvokeOnMainThread(async () => await MainPage.Navigation.PushAsync(new ErrorPage(message)));
                    }
                }
            });

            MessagingCenter.Subscribe<string>("refresh", "doit", async (obj) => 
            {
                await Locator.Login.RefreshUserData();
            });

            MessagingCenter.Subscribe<string,string>("display", "error", async (s,t)=>
            {
                if (t == "activity")
                await MainPage.DisplayAlert(Langs.Const_Title_Error_1, Langs.Const_Label_NoPDFViewer, "OK");
            });

            MessagingCenter.Subscribe<string,string>("url", "load", (s,t)=>
            {
                switch (t)
                {
                    case "userguide":
                        Device.BeginInvokeOnMainThread(() => Device.OpenUri(new Uri(Locator.Dashboard.UserGuide)));
                        break;
                    case "tips":
                        if (!Locator.Dashboard.FileExists(Locator.Dashboard.GetDrivingFilename))
                            Task.Run(async () =>
                                     await Locator.Dashboard.GetPDFFile(Locator.Dashboard.DrivingHints, Locator.Dashboard.GetDrivingFilename));
                        break;
                    case "terms":
                        if (!Locator.Dashboard.FileExists(Locator.Dashboard.GetTermsFilename))
                            Task.Run(async () =>
                                     await Locator.Dashboard.GetPDFFile(Locator.Dashboard.TermsConditions, Locator.Dashboard.GetTermsFilename));
                        break;
                }
            });
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
