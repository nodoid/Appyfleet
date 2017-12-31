using System;
using System.Collections.Generic;
using mvvmframework.ViewModels;
using NewAppyFleet.CustomViews;
using NewAppyFleet.Views.MapFrames;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace NewAppyFleet.Views
{
    public class NotificationMapPage : ContentPage
    {
        NotificationsMapViewModel ViewModel => App.Locator.NoteMap;
        Frame alertView;
        public StackLayout stack;
        StackLayout innerStack;

        public NotificationMapPage()
        {
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<string,string>("alert", "frame", (s,e)=>
            {
                if (alertView != null)
                    alertView.IsVisible = false;
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<string,string>("alert", "frame");
        }

        public NotificationMapPage(long journeyId, int selectedEventId)
        {
            ViewModel.SelectedEventId = selectedEventId;
            ViewModel.SelectedJourneyId = journeyId;
            ViewModel.GetSelectedJourneyAndEvent();
            BindingContext = ViewModel;
            NavigationPage.SetHasNavigationBar(this, false);
            CreateUI();
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

            var map = new CustomMap
            {
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = App.ScreenSize.Height,
                VerticalOptions = LayoutOptions.FillAndExpand,
                MapType = MapType.Hybrid,

            };
            map.RouteCoordinates = ViewModel.LocData;
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(ViewModel.SelectedJourney.GPSData[0].Latitude,
                                                                      ViewModel.SelectedJourney.GPSData[0].Longitude),
                                                         Distance.FromMiles(1)));

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


            var alertEvent = ViewModel.SelectedEvent;
            var alertJourney = ViewModel.SelectedJourney;
            alertView = new NotificationMapFrame().GenerateMapAlertFrame(alertEvent.Speed, alertEvent.RoadSpeed,
                                                                         alertEvent.Location, alertEvent.DateCreated.TimeOfDay.ToString(), 
                                                                         alertEvent.DateCreated.ToString("ddd dd-MMM-yyyy"), ViewModel.EventType);

            var xpos = App.ScreenSize.Width - (App.ScreenSize.Width * .9) / 2;
            var absLayout = new RelativeLayout();
            absLayout.Children.Add(map,
                                                        Constraint.Constant(0),
                                                        Constraint.Constant(0),
                                                        Constraint.RelativeToParent((parent) => App.ScreenSize.Width),
                                                        Constraint.RelativeToParent((parent) => App.ScreenSize.Height));
            absLayout.Children.Add(alertView,
                                   Constraint.Constant(0),
                                   Constraint.Constant(App.ScreenSize.Height * .1),
                                   Constraint.RelativeToParent((parent) => App.ScreenSize.Width),
                                   Constraint.RelativeToParent((parent) => App.ScreenSize.Height));

            stack.Children.Add(absLayout);
            innerStack.Children.Add(stack);
            innerStack.TranslationY = -6;

            var masterStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                Children =
                {
                    new StackLayout
                    {
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalOptions = LayoutOptions.Start,
                        WidthRequest = App.ScreenSize.Width,
                        Children = { topbar }
                    },
                    innerStack
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

