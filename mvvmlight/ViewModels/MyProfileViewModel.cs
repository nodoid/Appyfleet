using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using mvvmframework.ViewModels.Common;
using GalaSoft.MvvmLight.Messaging;

namespace mvvmframework.ViewModels
{
    public class MyProfileViewModel : CommonBluetooth
    {
        IUserSettings userService;
        IConnection connectService;
        IRepository repoService;
        IWebSevices webService;
        IBluetoothDevices bluetoothService;
        IDialogService diaService;

        public MyProfileViewModel(IUserSettings user, IConnection connect, IRepository repo, IWebSevices web,
                                  IBluetoothDevices blue, IDialogService dia)
        {
            userService = user;
            connectService = connect;
            repoService = repo;
            webService = web;
            bluetoothService = blue;
            diaService = dia;
        }

        public string FullName => userService.LoadSetting<string>("RealName", SettingType.String);
        public string UserName => userService.LoadSetting<string>("Username", SettingType.String);
        public string Password => userService.LoadSetting<string>("Password", SettingType.String);
        public string FleetName => userService.LoadSetting<string>("Fleetname", SettingType.String);
        public bool PrivateMod => userService.LoadSetting<bool>("PrivateMode", SettingType.Bool);

        public void SignOut()
        {
            userService.SaveSetting("Fleetname", string.Empty, SettingType.String);
            userService.SaveSetting("PrivateMode", false, SettingType.Bool);
            userService.SaveSetting("Username", string.Empty, SettingType.String);
            userService.SaveSetting("DriverId", -1, SettingType.Int);
            userService.SaveSetting("Password", string.Empty, SettingType.String);
            userService.SaveSetting("Phone", string.Empty, SettingType.String);
            userService.SaveSetting("FleetCode", string.Empty, SettingType.String);
            userService.SaveSetting("RealName", string.Empty, SettingType.String);
            LoggedIn = false;
            Notifications.Clear();
            JourneysList.Clear();
            HasLoggedOut = true;
        }

        bool hasLoggedOut;
        public bool HasLoggedOut
        {
            get => hasLoggedOut;
            set { Set(() => HasLoggedOut, ref hasLoggedOut, value, true); }
        }

        public NotificationsListModel UserNotifications { get; set; }
        public int NotificationCount => Notifications.Count(t => !t.Read);
        public VehicleListModel Vehicles { get; set; }

        public string day;
        public string Day
        {
            get { return day; }
            set { Set(() => Day, ref day, value, true); }
        }

        public int hour;
        public int Hour
        {
            get { return hour; }
            set { Set(() => Hour, ref hour, value, true); }
        }

        public int minute;
        public int Minute
        {
            get { return minute; }
            set { Set(() => Minute, ref minute, value, true); }
        }

        public List<NotificationModel> GetNotifications => UserNotifications.Notifications;

        public int GetUnreadNotificationNumbers => UserNotifications.Notifications.Count(t => !t.Read);

        public List<VehicleModel> PairedVehicles => VehicleModels.Where(t => t.Paired).ToList();

       public async Task RemovePairedVehicle(string id)
        {
            var vehicle = PairedVehicles.FirstOrDefault(t => t.BluetoothId == id);
            if (vehicle != null)
            {
                if (connectService.IsConnected)
                {
                    IsBusy = true;
                    await webService.RemoveVehicleParing(UserName, Password, vehicle).ContinueWith((w) =>
                    {
                        if (w.IsCompleted)
                        {
                            IsBusy = false;
                            if (!w.IsCanceled && !w.IsFaulted)
                            {
                                vehicle.Paired = false;
                                vehicle.BluetoothId = string.Empty;
                                vehicle.PairingId = 0;
                            }
                            else
                            {
                                Messenger.Default.Send(new NotificationMessage(w.Result.Status.Message));
                            }
                        }
                        else
                            IsBusy = false;
                    });
                }
            }
        }

        RelayCommand cmdGetRegisteredVehicles;
        public RelayCommand CmdGetRegisteredVehicles
        {
            get
            {
                return cmdGetRegisteredVehicles ??
                    (
                        cmdGetRegisteredVehicles = new RelayCommand(async () =>
                        {
                            IsBusy = true;
                            if (connectService.IsConnected)
                            {
                                await webService.GetPairedVehicles(userService.LoadSetting<string>("Username", SettingType.String),
                                                                   userService.LoadSetting<string>("Password", SettingType.String)).ContinueWith((t) =>
                                                                   {
                                                                       if (t.IsCompleted)
                                                                       {
                                                                           IsBusy = false;
                                                                           if (!t.IsFaulted && !t.IsCanceled)
                                                                           {
                                                                               Vehicles = t.Result;
                                                                           }
                                                                           else
                                                                           {
                                                                               Messenger.Default.Send(new NotificationMessage(t.Result.Status.Message));
                                                                           }
                                                                       }
                                                                   });
                            }
                            else
                            {
                                //await diaService.ShowError(GetErrorMessage(""), GetErrorTitle(""), "OK", null);
                            }
                        })
                );
            }
        }

        RelayCommand cmdAddNewVehicle;
        public RelayCommand CmdAddNewVehicle
        {
            get
            {
                return cmdAddNewVehicle ??
                    (
                        cmdAddNewVehicle = new RelayCommand(async () =>
                        {
                            IsBusy = true;
                            if (connectService.IsConnected)
                            {
                                var carmodel = SelectedVehicle ?? new VehicleModel
                                {
                                    BluetoothId = BluetoothId,
                                    Id = Id,
                                    PairingId = PairingId,
                                    Make = Make,
                                    Model = Model,
                                    Nickname = Nickname,
                                    Paired = Paired,
                                    Registration = Registration
                                };
                                await webService.AddVehicle(userService.LoadSetting<string>("Username", SettingType.String),
                                                            userService.LoadSetting<string>("Password", SettingType.String),
                                                            carmodel).ContinueWith((t) =>
                                                            {
                                                                if (t.IsCompleted)
                                                                {
                                                                    IsBusy = false;
                                                                    if (!t.IsFaulted && !t.IsCanceled)
                                                                    {
                                                                        if (t.Result.Status.Success)
                                                                        {
                                                                            RegisteredVehicles.Vehicles.Add(carmodel);
                                                                        }
                                                                        else
                                                                        {
                                                                            Messenger.Default.Send(new NotificationMessage(t.Result.Status.Message));
                                                                        }
                                                                    }
                                                                }
                                                            });
                            }
                            else
                            {
                                IsBusy = false;
                                //await diaService.ShowMessage(GetErrorMessage(""), GetErrorTitle(""), "OK", null);
                            }
                        })
                );
            }
        }

        public void GetBluetoothDeviceList()
        {
            BluetoothDeviceList = bluetoothService.GetBluetoothDevices;
        }

        public void PopulateBasedOnId()
        {
            var device = BluetoothDeviceList.FirstOrDefault(t => t.Id == SelectedBluetoothDevice);
            if (device != null)
            {
                BluetoothId = device.Id.ToString();
                Id = device.Rssi;
                Task.Run(async () =>
                {
                    IsBusy = true;
                    await bluetoothService.ConnectToKnownDevice(device.Id).ContinueWith((t) =>
                    {
                        if (t.IsCompleted)
                        {
                            if (t.Result)
                            {
                                IsBusy = false;
                                Paired = t.Result;
                                PairingId = device.Rssi;
                            }
                        }
                    });
                });
            }
        }
    }
}
