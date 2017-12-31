using System;
using System.Collections.Generic;

namespace mvvmframework
{
    public class AdvertisingRecords
    {
        public byte[] Data { get; set; }
        public AdvertisingRecordType Type { get; set; }
    }

    public class BluetoothDevice
    {
        public IList<AdvertisingRecords> AdvertisementRecords { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public object NativeDevice { get; set; }
        public int Rssi { get; set; }
        public BluetoothStates State {get;set;}
    }
}
