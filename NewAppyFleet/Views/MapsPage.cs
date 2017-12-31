using mvvmframework.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using NewAppyFleet.CustomViews;
using System.Collections.Generic;

namespace NewAppyFleet.Views
{
    public class MapsPage : ContentPage
    {
        MapsViewModel ViewModel => App.Locator.Maps;
        public StackLayout stack;
        StackLayout innerStack;

        void RegisterEvents()
        {
            ViewModel.PropertyChanged += (sender, e) => 
            {
                if (e.PropertyName == "LocData")
                    CreateUI();
            };
        }

        public MapsPage(int journeyId)
        {
            BindingContext = ViewModel;
            RegisterEvents();
            NavigationPage.SetHasNavigationBar(this, false);
            ViewModel.JourneyId = journeyId;
            ViewModel.JourneyData = ViewModel.GetSpecificJourney(journeyId);
            ViewModel.ThisJourneyData = ViewModel.JourneyData;
        }

        void CreateUI()
        {
            stack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = App.ScreenSize.Height - 48,
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            innerStack = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Orientation = StackOrientation.Horizontal,
                MinimumWidthRequest = App.ScreenSize.Width,
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = App.ScreenSize.Height - 48,
                InputTransparent = true,
            };

            var topbar = new TopBar(true, "", this, 1, "burger_menu", "refresh_icon", innerStack).CreateTopBar();
            stack.HeightRequest = App.ScreenSize.Height - topbar.HeightRequest;

            var spinner = new ActivityIndicator
            {
                BackgroundColor = Color.White,
                HeightRequest = 40,
                IsRunning = true
            };
            spinner.SetBinding(ActivityIndicator.IsVisibleProperty, new Binding("IsBusy"));

            var map = new CustomMap
            {
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = App.ScreenSize.Height,
                VerticalOptions = LayoutOptions.FillAndExpand,
                MapType = MapType.Hybrid,

            };
            map.RouteCoordinates = ViewModel.LocData;
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(ViewModel.JourneyData.GPSData[0].Latitude, ViewModel.JourneyData.GPSData[0].Longitude), Distance.FromMiles(1)));
            map.CustomPins = new List<CustomPin>();
            if (ViewModel.JourneyEvents != null)
            {
                if (ViewModel.JourneyEvents.Count != 0)
                {
                    foreach (var je in ViewModel.JourneyEvents)
                    {
                        var pin = new CustomPin
                        {
                            Pin = new Pin
                            {
                                Position = new Position(je.Latitude, je.Longitude),
                                Type = PinType.Place,
                                Address = je.Location.Replace('\n', ' '),
                                Label = $"Speed {je.Speed} (Road speed {je.RoadSpeed})"
                            },
                            Id = je.Id.ToString()
                        };
                        map.CustomPins.Add(pin);
                        map.Pins.Add(pin.Pin);
                    }
                }
            }

            stack.Children.Add(spinner);
            stack.Children.Add(map);
            innerStack.Children.Add(stack);

            var masterStack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                WidthRequest = App.ScreenSize.Width,
                VerticalOptions = LayoutOptions.Start,
                Spacing = 0,
                Children =
                {
                    topbar,
                    new StackLayout
                    {
                        Children = {innerStack}
                    }
                }
            };

            Content = masterStack;
        }

        static void CalculateBoundingCoordinates(MapSpan region)
        {
            var center = region.Center;
            var halfheightDegrees = region.LatitudeDegrees / 2;
            var halfwidthDegrees = region.LongitudeDegrees / 2;

            var left = center.Longitude - halfwidthDegrees;
            var right = center.Longitude + halfwidthDegrees;
            var top = center.Latitude + halfheightDegrees;
            var bottom = center.Latitude - halfheightDegrees;

            // Adjust for Internation Date Line (+/- 180 degrees longitude)
            if (left < -180) left = 180 + (180 + left);
            if (right > 180) right = (right - 180) - 180;
        }
    }
}

