using SQLite.Net.Attributes;

namespace mvvmframework
{
    public class VehicleModel
    {
        public string BluetoothId { get; set; }
        [PrimaryKey]
        public long Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Nickname { get; set; }
        public bool Paired { get; set; }
        public long PairingId { get; set; }
        public string Registration { get; set; }
        public double Value { get; set; }
    }
}
