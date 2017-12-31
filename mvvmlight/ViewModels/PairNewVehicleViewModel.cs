using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using mvvmframework.ViewModels.Common;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System;
using mvvmframework.Models.JSon;
using mvvmframework.Models;

namespace mvvmframework
{
    public class PairNewVehicleViewModel : CommonBluetooth
    {
        IRepository repoService;
        IUserSettings userService;
        IWebSevices webService;
        IConnection connectService;
        IDialogService diaService;
        IBluetoothDevices bluetoothService;

        public PairNewVehicleViewModel(IRepository repo, IUserSettings user, IWebSevices web, 
                                       IConnection connect, IDialogService dia, IBluetoothDevices blue)
        {
            repoService = repo;
            userService = user;
            webService = web;
            connectService = connect;
            diaService = dia;
            bluetoothService = blue;
        }

        bool moveToSearch;
        public bool MoveToSearch
        {
            get { return moveToSearch; }
            set { Set(() => MoveToSearch, ref moveToSearch, value, true); }
        }

        bool moveToAdd;
        public bool MoveToAdd
        {
            get { return moveToAdd; }
            set { Set(() => MoveToAdd, ref moveToAdd, value, true); }
        }

        bool moveToPair;
        public bool MoveToPair
        {
            get { return moveToPair; }
            set
            {
                if (value != MoveToPair)
                {
                    Set(() => MoveToPair, ref moveToPair, value, true);
                }
            }
        }

        bool moveToSummary;
        public bool MoveToSummary
        {
            get { return moveToSummary; }
            set { Set(() => MoveToSummary, ref moveToSummary, value, true); }
        }

        bool moveToPairing;
        public bool MoveToPairing
        {
            get { return moveToPairing; }
            set { Set(() => MoveToPairing, ref moveToPairing, value, true); }
        }

        bool moveToComplete;
        public bool MoveToComplete
        {
            get { return moveToComplete; }
            set
            {
                CmdAddNewVehicle.Execute(null);
                Set(() => MoveToComplete, ref moveToComplete, value, true);
                if (!userService.LoadSetting<string>("Progress", SettingType.String).Contains("Pairing"))
                    userService.SaveSetting("Progress", "Initial|Signup|Pairing|", SettingType.String);
            }
        }

        bool moveToLogin;
        public bool MoveToLogin
        {
            get => moveToLogin;
            set => Set(()=>MoveToLogin, ref moveToLogin, value, true);
        }


        public void ResetFlags()
        {
            MoveToAdd = MoveToPair = MoveToLogin = MoveToSearch = MoveToPairing = MoveToSummary = MoveToComplete = false;
        }

        public void CanMoveToPairing()
        {
            if (!string.IsNullOrEmpty(Registration) && !string.IsNullOrEmpty(Nickname) &&
                                !string.IsNullOrEmpty(Make) && !string.IsNullOrEmpty(Model) && !string.IsNullOrEmpty(Odometer))
            {
                CmdAddNewVehicle.Execute(null);
                if (MoveToAdd)
                {
                    MoveToAdd = false;
                    MoveToSearch = true;
                }
                else
                    MoveToPair = true;
            }
        }

        public void GetVehiclesFromDb()
        {
            var t = repoService.GetList<VehicleModel>();
            if (t.Count != 0)
            {
                VehicleModels = t;
            }
            else
                CmdGetRegisteredVehicles.Execute(null);
        }

        public double Value { get; set; }

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
                            RegisteredVehicles = t.Result;
                            VehicleModels = RegisteredVehicles.Vehicles;
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
                                                        if (!t.IsFaulted && !t.IsCanceled)
                                                        {
                                                            if (t.Result.Status.Success)
                                                            {
                                                                if (RegisteredVehicles == null)
                                                                {
                                                                    RegisteredVehicles = new VehicleListModel();
                                                                    RegisteredVehicles.Vehicles = new List<VehicleModel>();
                                                                }
                                        repoService.SaveData(carmodel);
                                                                RegisteredVehicles.Vehicles.Add(carmodel);
                                                                VehicleModels = RegisteredVehicles.Vehicles;
                                                            }
                                                            else
                                                            {
                                                                IsBusy = false;
                                                                Messenger.Default.Send(new NotificationMessage(t.Result.Status.Message));
                                                            }
                                                        }
                                                    }
                        }).ContinueWith(async(r)=>
                        {
                            var odo = new OdoReadingModel
                            {
                                DateRead = DateTime.Now,
                                SelectedVehicle = carmodel,
                                Reading = Convert.ToDouble(Odometer)
                            };
                            await webService.AddOdoReading(userService.LoadSetting<string>("Username", SettingType.String),
                                                           userService.LoadSetting<string>("Password", SettingType.String), 
                                                           odo).ContinueWith((m) =>
                            {
                                if (m.IsCompleted)
                                {
                                    IsBusy = false;
                                    if (!m.IsFaulted && !m.IsCanceled)
                                    {
                                        if (!m.Result.Status.Success)
                                            Messenger.Default.Send(new NotificationMessage(m.Result.Status.Message));
                                    }
                                }
                            });
                        });
                    }
                    else
                    {
                        IsBusy = false;
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
                                MoveToComplete = true;
                            }
                        }
                    });
                });
            }
        }
    }
}
