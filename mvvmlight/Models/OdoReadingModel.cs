using System;
using SQLite.Net.Attributes;

namespace mvvmframework
{
    public class OdoReadingModel
    {
        public int ID { get; set; }
        public DateTime DateRead { get; set; }
        public string Value { get; set; }
        public string VehicleId { get; set; }
        [Ignore]
        public VehicleModel SelectedVehicle { get; set; }
        public double Reading { get; set; }

        [Ignore]
        public string DateString => DateRead.Date.ToString("dd.MMM.yyyy");
        [Ignore]
        public string OdoReading => Reading.ToString();
    }
}
