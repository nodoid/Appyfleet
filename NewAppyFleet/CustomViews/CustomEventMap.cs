using System;
using System.Collections.Generic;
using mvvmframework.Models;
using Xamarin.Forms.Maps;

namespace NewAppyFleet.CustomViews
{
    public class CustomEventMap : Map
    {
        public List<JourneyCoordinates> RouteCoordinates { get; set; }

        public CustomEventMap()
        {
            RouteCoordinates = new List<JourneyCoordinates>();
        }

        CustomPin CustomPin { get; set; }
    }
}
