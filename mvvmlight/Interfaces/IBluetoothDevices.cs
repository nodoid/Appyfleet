using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mvvmframework
{
    public interface IBluetoothDevices
    {
        List<BluetoothDevice> GetBluetoothDevices { get; }
        Task<bool> ConnectToDevice(BluetoothDevice device);
        Task<bool> ConnectToKnownDevice(Guid deviceId);
        Task<bool> UnpairToDevice(Guid deviceId);
        Task<bool> UnpairToKnownDevice(BluetoothDevice device);
    }
}
