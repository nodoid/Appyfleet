using mvvmframework.Models;
using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace NewAppyFleet.CustomViews
{
    public class CustomPin
    {
        public Pin Pin { get; set; }
        public string Id { get; set; }
    }

    public class CustomMap : Map
    {
        public List<JourneyCoordinates> RouteCoordinates { get; set; }

        public CustomMap()
        {
            RouteCoordinates = new List<JourneyCoordinates>();
        }
            public List<CustomPin> CustomPins { get; set; }
    }
}


