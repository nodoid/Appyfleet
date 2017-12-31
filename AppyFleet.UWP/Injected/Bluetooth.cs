using GalaSoft.MvvmLight.Messaging;
using mvvmframework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;

namespace AppyFleet.UWP.Injected
{
    public class Bluetooth : IBluetoothDevices
    {
        DeviceInformationCollection Devices { get; set; }

        DeviceWatcher deviceWatcher = null;
        TypedEventHandler<DeviceWatcher, DeviceInformation> handlerAdded = null;
        TypedEventHandler<DeviceWatcher, DeviceInformationUpdate> handlerUpdated = null;
        TypedEventHandler<DeviceWatcher, DeviceInformationUpdate> handlerRemoved = null;
        TypedEventHandler<DeviceWatcher, Object> handlerEnumCompleted = null;
        TypedEventHandler<DeviceWatcher, Object> handlerStopped = null;

        public List<BluetoothDevice> GetBluetoothDevices
        {
            get
            {
                var btDevices = new List<BluetoothDevice>();
                var done = false;
                Task.Run(async () =>
                {
                    Devices = await DeviceInformation.FindAllAsync();
                    if (Devices.Count == 0)
                        done = true;
                    else
                    {
                        int c = 0;
                        var pairable = Devices.Where(t => !t.Name.ToLowerInvariant().Contains("windows")).ToList();
                        var col = new List<DeviceInformation>();
                        foreach(var p in pairable)
                        {
                            var pair = await p.Pairing.PairAsync();
                            if (pair.ProtectionLevelUsed != DevicePairingProtectionLevel.None)
                                col.Add(p);
                        }

                        col = col.DistinctBy(t => t.Name).ToList();

                        if (col.Count == 0)
                            done = true;
                        else
                        {
                            foreach (var d in col)
                            {
                                var rId = d.Id.Split('{').Last().TrimEnd('}');
                                var dev = new BluetoothDevice { Id = new Guid(rId), Name = d.Name, State = d.Pairing.IsPaired ? BluetoothStates.Connected : BluetoothStates.Disconnected };
                                btDevices.Add(dev);
                                c++;
                                if (c == col.Count)
                                    done = true;
                            }
                        }
                    }
                });
                while (!done) { }
                return btDevices;
            }
        }

        void BindDevice(DeviceInformation device)
        {
            deviceWatcher = DeviceInformation.CreateWatcher(
                    device.Id,
                    null,
                    device.Kind);

                handlerStopped = new TypedEventHandler<DeviceWatcher, Object>((watcher, obj) =>
                {
                    var msg = DeviceWatcherStatus.Aborted == watcher.Status ? "lost" : "disconnected";
                    Messenger.Default.Send(new NotificationMessage($"BLE:{msg}"));
                });

            deviceWatcher.Stopped += handlerStopped;

            deviceWatcher.Start();
        }

        public async Task<bool> ConnectToDevice(BluetoothDevice device)
        {
            var done = string.Empty;
            var rv = false;
            await ConnectToKnownDevice(device.Id).ContinueWith((t) =>
            {
                if (t.IsCompleted)
                {
                    done = "f";
                    if (!t.IsCanceled && !t.IsFaulted)
                    {
                        rv = true;
                    }
                }
            });
            while (string.IsNullOrEmpty(done)) { }

            return rv;
        }

        public async Task<bool> ConnectToKnownDevice(Guid deviceId)
        {
            var device = Devices.FirstOrDefault(t => t.Id == deviceId.ToString());
            if (device == null)
                return false;
            var dv = await device.Pairing.PairAsync();
            if (dv.Status == DevicePairingResultStatus.Paired)
                BindDevice(device);
            return device.Pairing.IsPaired;
        }

        public async Task<bool> UnpairToKnownDevice(BluetoothDevice device)
        {
            var d = Devices.FirstOrDefault(t => t.Id == device.Id.ToString());
            if (d == null)
                return false;

            await d.Pairing.UnpairAsync();

            return d.Pairing.IsPaired;
        }

        public async Task<bool> UnpairToDevice(Guid id)
        {
            var d = Devices.FirstOrDefault(t => t.Id == id.ToString());
            if (d == null)
                return false;

            await d.Pairing.UnpairAsync();

            return d.Pairing.IsPaired;
        }
    }
}
