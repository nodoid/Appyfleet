using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using mvvmframework;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace NewAppyFleet
{
    public class BluetoothServices : IBluetoothDevices
    {
        static List<IDevice> Devices { get; set; } = new List<IDevice>();

        public List<BluetoothDevice> GetBluetoothDevices
        {
            get
            {
                var btd = new List<BluetoothDevice>();

                var done = false;
                var connectedDevices = CrossBluetoothLE.Current.Adapter.GetSystemConnectedOrPairedDevices();
                            if (connectedDevices.Count != 0)
                            {
                                foreach (var cd in connectedDevices)
                                    btd.Add(new BluetoothDevice
                                    {
                                    NativeDevice = cd.NativeDevice,
                                    Name = cd.Name,
                                    Rssi = cd.Rssi,
                                    Id = cd.Id,
                                    State = (BluetoothStates)cd.State
                                    });
                            }

                CrossBluetoothLE.Current.Adapter.ScanTimeout = 3000; 
                CrossBluetoothLE.Current.Adapter.ScanMode = ScanMode.LowLatency;
                var granted = false;
                /*Task.Run(async () =>
                {*/
                var status = CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location).Result;
                if (status != PermissionStatus.Granted)
                {
                    var permission = CrossPermissions.Current.RequestPermissionsAsync(Permission.Location).Result;
                    if (permission.First().Value == PermissionStatus.Granted)
                        granted = true;
                    else
                        return btd;
                }
                else
                    granted = true;

                if (granted)
                {
                        if (CrossBluetoothLE.Current.State == BluetoothState.On)
                        {
                            CrossBluetoothLE.Current.Adapter.StartScanningForDevicesAsync();

                            CrossBluetoothLE.Current.Adapter.ScanTimeoutElapsed += (sender, e) =>
                            {
                                Debug.WriteLine("Timed out");
                                foreach (var d in Devices)
                                {
                                    Debug.WriteLine($"Device name {d.Name} : id {d.Id}");
                                    if (Devices.Count != 0)
                                    {
                                        foreach (var cd in connectedDevices)
                                            btd.Add(new BluetoothDevice
                                            {
                                                NativeDevice = cd.NativeDevice,
                                                Name = cd.Name,
                                                Rssi = cd.Rssi,
                                                Id = cd.Id,
                                                State = (BluetoothStates)cd.State
                                            });
                                    }
                                }
                                done = true;
                            };

                            CrossBluetoothLE.Current.Adapter.DeviceDiscovered += (object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e) =>
                            {
                                if (!string.IsNullOrEmpty(e.Device.Name))
                                {
                                    Debug.WriteLine($"Device discovered - {e.Device.Id} : {e.Device.Name}");
                                    Devices.Add(e.Device);
                                }
                            };

                            CrossBluetoothLE.Current.Adapter.StartScanningForDevicesAsync();
                        }
                }

                return btd;
            }
        }

        public async Task<bool> ConnectToDevice(BluetoothDevice device)
        {
            var done = string.Empty;
            var rv = false;
            await ConnectToKnownDevice(device.Id).ContinueWith((t) =>
            {
                if (t.IsCompleted)
                {
                    done = "foo";
                    if (!t.IsCanceled && !t.IsFaulted)
                        rv = true;
                }
            });
            while (string.IsNullOrEmpty(done)) {}
            return rv;
        }

        public async Task<bool> ConnectToKnownDevice(Guid deviceId)
        {
            var adapter = CrossBluetoothLE.Current.Adapter;
            var res = false;
            try
            {
                await adapter.ConnectToKnownDeviceAsync(deviceId).ContinueWith((t) =>
                {
                    res |= t.IsCompleted;
                    adapter.DeviceDisconnected += (sender, e) => 
                    {
                        Messenger.Default.Send(new NotificationMessage($"BLE:disconnected"));
                    };
                    adapter.DeviceConnectionLost += (sender, e) => 
                    {
                        Messenger.Default.Send(new NotificationMessage($"BLE:lost"));
                    };
                    adapter.DeviceConnected += (sender, e) => 
                    {
                        Messenger.Default.Send(new NotificationMessage($"BLE:connected"));
                    };
                });
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine("Exception connecting to known device {0}--{1}", ex.Message, ex.InnerException?.Message);
#endif
            }
            return res;
        }

        public async Task<bool> UnpairToKnownDevice(BluetoothDevice device)
        {
            return await UnpairToDevice(device.Id);
        }

        public async Task<bool> UnpairToDevice(Guid deviceId)
        {
            var adapter = CrossBluetoothLE.Current.Adapter;
            var res = false;
            try
            {
                await adapter.DisconnectDeviceAsync(Devices.FirstOrDefault(t => t.Id == deviceId)).ContinueWith((t) =>
                {
                    res |= t.IsCompleted;
                });
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine("Exception unpairing {0}--{1}", ex.Message, ex.InnerException?.Message);
#endif
            }
            return res;
        }
    }
}
