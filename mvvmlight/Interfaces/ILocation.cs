using mvvmframework.Models;

namespace mvvmframework.Interfaces
{
    public interface ILocation
    {
        LocationServiceData GetLocationData { get; }
        bool GpsStatus { get; set; }
    }
}
