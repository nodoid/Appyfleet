using System;
using mvvmframework.Languages;

namespace mvvmframework
{
    public class EventModel
    {
        public long Id { get; set; }
        public int Type { get; set; }
        public int Speed { get; set; }
        public int RoadSpeed { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Location { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateRead { get; set; }
        public bool Read { get; set; }

        public string EventType => Constants.EventNames[Type];
        public string SpeedInfo => $"{Speed}{Langs.Const_Label_Speed_Unit}/{RoadSpeed}{Langs.Const_Label_Speed_Unit} {Langs.Const_Label_Warning}";
    }
}
