using mvvmframework;
using System;

namespace AppyFleet.UWP.Injected
{
    public class DeviceServices : IDeviceServices
    {
        public string GetDeviceID
        {
            get
            {
                var t = Windows.System.Profile.HardwareIdentification.GetPackageSpecificToken(null).Id;
                var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(t);

                var bytes = new byte[t.Length];
                dataReader.ReadBytes(bytes);

                return BitConverter.ToString(bytes).Replace("-", "");
            }
        }
    }
}
