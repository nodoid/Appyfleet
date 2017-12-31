﻿using AppyFleet.UWP.CustomRenderers;
using AppyFleet.UWP.UserControls;
using NewAppyFleet.CustomViews;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Maps;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.UWP;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace AppyFleet.UWP.CustomRenderers
{
    public class CustomMapRenderer : MapRenderer
    {
        MapControl nativeMap;
        List<CustomPin> customPins;
        XamarinMapOverlay mapOverlay;
        bool xamarinOverlayShown = false;

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                nativeMap.MapElementClick -= OnMapElementClick;
                nativeMap.Children.Clear();
                mapOverlay = null;
                nativeMap = null;
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                nativeMap = Control as MapControl;
                customPins = formsMap.CustomPins;

                nativeMap.Children.Clear();
                nativeMap.MapElementClick += OnMapElementClick;

                var coordinates = new List<BasicGeoposition>();
                foreach (var position in formsMap.RouteCoordinates)
                {
                    coordinates.Add(new BasicGeoposition { Latitude = position.Latitude, Longitude = position.Longitude });
                }

                var polyline = new MapPolyline
                {
                    StrokeColor = Windows.UI.Color.FromArgb(128, 255, 0, 0),
                    StrokeThickness = 5,
                    Path = new Geopath(coordinates)
                };
                nativeMap.MapElements.Add(polyline);

                foreach (var pin in customPins)
                {
                    var snPosition = new BasicGeoposition { Latitude = pin.Pin.Position.Latitude, Longitude = pin.Pin.Position.Longitude };
                    var snPoint = new Geopoint(snPosition);

                    var mapIcon = new MapIcon
                    {
                        Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Images/pin.png")),
                        CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible,
                        Location = snPoint,
                        NormalizedAnchorPoint = new Windows.Foundation.Point(0.5, 1.0)
                    };

                    nativeMap.MapElements.Add(mapIcon);
                }
            }
        }

        void OnMapElementClick(MapControl sender, MapElementClickEventArgs args)
        {
            var mapIcon = args.MapElements.FirstOrDefault(x => x is MapIcon) as MapIcon;
            if (mapIcon != null)
            {
                if (!xamarinOverlayShown)
                {
                    var customPin = GetCustomPin(mapIcon.Location.Position);
                    if (customPin == null)
                    {
                        throw new Exception("Custom pin not found");
                    }
                    else
                    {
                        if (mapOverlay == null)
                        {
                            mapOverlay = new XamarinMapOverlay(customPin);
                        }
var snPosition = new BasicGeoposition { Latitude = customPin.Pin.Position.Latitude, Longitude = customPin.Pin.Position.Longitude };
                    var snPoint = new Geopoint(snPosition);

                    nativeMap.Children.Add(mapOverlay);
                    MapControl.SetLocation(mapOverlay, snPoint);
                    MapControl.SetNormalizedAnchorPoint(mapOverlay, new Windows.Foundation.Point(0.5, 1.0));
                    xamarinOverlayShown = true;
                    }
                }
                else
                {
                    nativeMap.Children.Remove(mapOverlay);
                    xamarinOverlayShown = false;
                }
            }
        }

        CustomPin GetCustomPin(BasicGeoposition position)
        {
            var pos = new Position(position.Latitude, position.Longitude);
            foreach (var pin in customPins)
            {
                if (pin.Pin.Position == pos)
                {
                    return pin;
                }
            }
            return null;
        }
    }
}
