using System;
using System.Collections.Generic;
using mvvmframework;
using mvvmframework.Enums;
using mvvmframework.Languages;
using mvvmframework.Models;
using mvvmframework.ViewModels;
using NewAppyFleet.Views.ListViewCells;
using NewAppyFleet.Views.ViewCells;
using Xamarin.Forms;
using System.ComponentModel;

namespace NewAppyFleet.Views
{
    public class DashboardPage : ContentPage
    {
        DashboardViewModel ViewModel => App.Locator.Dashboard;

        static bool running { get; set; } = false;
        bool firstRun { get; set; } = true;

        public StackLayout stack;
        StackLayout innerStack;
        StackLayout scrollLayout = new StackLayout();
        ContentView titleBar;
        ListView journey;
        bool Calendar { get; set; } = false;

        void RegisterEvents()
        {
            ViewModel.PropertyChanged += (object s, PropertyChangedEventArgs e) =>
            {
                switch (e.PropertyName)
                {
                    case "JourneysList":
                        if (journey != null)
                            Device.BeginInvokeOnMainThread(() => { journey.ItemsSource = null; journey.ItemsSource = ViewModel.GetLatestJourney; });
                        break;
                    case "DashboardModel":
                        if (scrollLayout != null)
                        {
                            if (ViewModel.DriverScores != null)
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    if (!Calendar)
                                    {
                                        scrollLayout.Children.Insert(1, ScoreDialView.ScoreDial(ViewModel.DriverScores.Score, ViewModel.SpinnerAngle));
                                        scrollLayout.Children.Insert(3, MileageCell.MileageView(ViewModel.GetMileage));
                                        scrollLayout.Children.Add(journey);
                                        //scrollLayout.Children.Add(NoticeOdometer.NoticeOdometerCells(ViewModel.Notifications.Count, ViewModel.OdoFormattedValue, ViewModel.OdoVehicleReg));
                                    }
                                    else
                                    {
                                    scrollLayout.Children.RemoveAt(1);
                                    scrollLayout.Children.RemoveAt(3);
                                    scrollLayout.Children.RemoveAt(4);
                                        //scrollLayout.Children.RemoveAt(5);
                                        scrollLayout.Children.Insert(1, ScoreDialView.ScoreDial(ViewModel.DriverScores.Score, ViewModel.SpinnerAngle));
                                        scrollLayout.Children.Insert(3, MileageCell.MileageView(ViewModel.GetMileage));
                                        scrollLayout.Children.Add(journey);    
                                    //scrollLayout.Children.Add(NoticeOdometer.NoticeOdometerCells(ViewModel.Notifications.Count, ViewModel.OdoFormattedValue, ViewModel.OdoVehicleReg));

                                    }
                                });
                        }
                        break;
                    case "GroupScores":
                        if (firstRun)
                        {
                            ViewModel.CurrentCarousel = 0;
                            firstRun = !firstRun;
                        }
                        break;
                    case "CurrentGroupScore":
                        if (scrollLayout != null)
                        {
                            if (ViewModel.GroupScores != null)
                                Device.BeginInvokeOnMainThread(() =>
                            {
                                if (Calendar)
                                    scrollLayout.Children.Insert(8, Carousel.GroupScores(ViewModel));
                                else
                                {
                                    //scrollLayout.Children.RemoveAt(8);
                                    if (scrollLayout.Children.Count < 9)
                                        scrollLayout.Children.Insert(8, Carousel.GroupScores(ViewModel));
                                    else
                                        scrollLayout.Children[8].Focus();
                                    Calendar = false;
                                }
                                scrollLayout.Children.Add(NoticeOdometer.NoticeOdometerCells(ViewModel.Notifications.Count, ViewModel.OdoFormattedValue, ViewModel.OdoVehicleReg));
                            });
                        }
                        break;
                }
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<string>("notification", "open", async (t)=> await Navigation.PushAsync(new NotificationsPage()));
            MessagingCenter.Subscribe<JourneyViewCell>(this, "ShowAllJourneys", async (o) => await Navigation.PushAsync(new JourneyPage()));
            MessagingCenter.Subscribe<JourneyViewCell, string>(this, "JourneyId", async (arg1, arg2) =>
            {
                await Navigation.PushAsync(new MapsPage(Convert.ToInt32(arg2)));
            });
            MessagingCenter.Subscribe<string, string>("Carousel", "Movement", (s,t) =>
            {
                if (t == "Forward")
                    ViewModel.CurrentCarousel++;
                else
                    ViewModel.CurrentCarousel--;
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<string>("notification", "open");
            MessagingCenter.Unsubscribe<JourneyViewCell>(this, "ShowAllJourneys");
            MessagingCenter.Unsubscribe<JourneyViewCell, string>(this, "JourneyId");
            MessagingCenter.Unsubscribe<string,string>("Carousel", "Movement");
        }

        public DashboardPage(bool fromTop = false)
        {
            BindingContext = ViewModel;
            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundColor = FormsConstants.AppyDarkShade;
            journey = new ListView
            {
                ItemsSource = ViewModel.GetLatestJourney,
                SeparatorVisibility = SeparatorVisibility.None,
                ItemTemplate = new DataTemplate(typeof(JourneyViewCell)),
                IsPullToRefreshEnabled = false,
                HasUnevenRows = true,
                HeightRequest = 140,
            };
            if (fromTop)
                running = false;

            RegisterEvents();

            //if (!running)
            //{
            //    running = true;
                ViewModel.InitialiseService();
                ViewModel.GetDriverData();
                ViewModel.CreateDashboardModel();
                ViewModel.EndDate = DateTime.Now;
                ViewModel.StartDate = DateTime.Now.AddDays(-30);
            ViewModel.GetDriverGroupScores().GetAwaiter();
                CreateUI();
            //}
        }

        void CreateUI()
        {
            App.Self.StartListening();
            stack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = App.ScreenSize.Height - 52,
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            innerStack = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Orientation = StackOrientation.Horizontal,
                MinimumWidthRequest = App.ScreenSize.Width,
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = App.ScreenSize.Height - 52,
            };

            var topbar = new TopBar(true, "", this, 1, "burger_menu", "refresh_icon", innerStack).CreateTopBar();
            //stack.HeightRequest = App.ScreenSize.Height - topbar.HeightRequest;

            var spinner = new ActivityIndicator
            {
                BackgroundColor = Color.Transparent,
                HeightRequest = 40,
                IsRunning = true
            };
            spinner.SetBinding(ActivityIndicator.IsVisibleProperty, new Binding("IsBusy"));

            DBJourneyModel top = null;
            if (ViewModel.JourneysList.Count == 0)
                top = new DBJourneyModel { StartDate = DateTime.Now };
            else
                top = ViewModel.JourneysList[0];
            var date = top.StartDate.ToString("Y");
            var lstElements = new List<UIAttributes>
            {
                new UIAttributes {Text = date, Position = 0, TextBold = true},
                new UIAttributes { Text = Langs.Const_Label_Calendar, Position = 1, ScreenLeft = false},
                new UIAttributes { Text = "calendar".CorrectedImageSource(), IsImageSource = true, Position = 0, ScreenLeft = false, ClickEvent = ShowRefineOptions}
            };

            var btnHistory = new Button
            {
                BorderRadius = 4,
                HeightRequest = Device.RuntimePlatform == Device.Android ? 40 : 32,
                WidthRequest = App.ScreenSize.Width * .4,
                BackgroundColor = FormsConstants.AppyLightBlue,
                TextColor = Color.White,
                FontFamily = Helper.RegFont,
                Text = Langs.Const_Menu_ScoreHistory,
                VerticalOptions = LayoutOptions.Center
            };
            btnHistory.Clicked += async (sender, e) =>
            {
                await Navigation.PushAsync(new ScoreHistory(), true);
            };

            scrollLayout = new StackLayout
            {
                WidthRequest = App.ScreenSize.Width,
                MinimumWidthRequest = App.ScreenSize.Width,
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Start,
                IsEnabled = true,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                    {
                    new StackLayout
                    {
                        BackgroundColor = FormsConstants.AppyDarkBlue,
                        HeightRequest = 36,
                        VerticalOptions =LayoutOptions.Center,
                        Padding = new Thickness(8,0),
                        Children =
                        {
                            new Label
                            {
                                HeightRequest = 36,
                                VerticalTextAlignment = TextAlignment.Center,
                                TextColor = Color.White,
                                FontFamily = Helper.RegFont,
                                Text = Langs.Const_Label_Overall_Driver_Score
                            }
                        }
                    },
                        new StackLayout
                        {
                            HeightRequest = 48,
                            Orientation = StackOrientation.Horizontal,
                            Children =
                            {
                                new BoxView{WidthRequest = App.ScreenSize.Width * .3, BackgroundColor = Color.Transparent},
                                btnHistory,
                                new BoxView{WidthRequest = App.ScreenSize.Width * .3, BackgroundColor = Color.Transparent}
                            }
                        },
                        new StackLayout
                        {
                            HeightRequest = 28,
                            WidthRequest = App.ScreenSize.Width,
                            Padding = new Thickness(8,0),
                            BackgroundColor= FormsConstants.AppyRed,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            Children =
                            {
                                new Label {Text = Langs.Const_Label_Latest_Journey, TextColor = Color.White, FontFamily = Helper.RegFont}
                            }
                        },
                    new BoxView { HeightRequest = 1, WidthRequest = App.ScreenSize.Width, BackgroundColor = Color.White },
                    journey,
                    new StackLayout
            {
                WidthRequest = App.ScreenSize.Width,
                BackgroundColor = FormsConstants.AppyDarkBlue,
                Children =
                {
                    new StackLayout
                    {
                        Padding = new Thickness(12,0),
                        Children =
                        {
                            new Label
                            {
                                Text = "Ranking", TextColor = Color.White, HeightRequest = 20, VerticalTextAlignment = TextAlignment.Center,
                                FontFamily= Helper.BoldFont
                            }
                        }
                    }
                }
            }
                }
            };

            var dashScroll = new ScrollView
            {
                WidthRequest = App.ScreenSize.Width,
                Orientation = ScrollOrientation.Vertical,
                VerticalOptions = LayoutOptions.Fill,
                //HeightRequest = App.ScreenSize.Height * 2,
                IsEnabled = true,
                Content = scrollLayout,
                TranslationY = - 10
            };

            stack.Children.Add(spinner);
            stack.Children.Add(dashScroll);
            innerStack.Children.Add(stack);

            titleBar = new GeneralTopBar(lstElements);

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
                    new StackLayout
                    {
                        TranslationY = -6,
                        Children = {titleBar}
                    },
                    innerStack
                }
            };

            Content = masterStack;
        }

        void ShowRefineOptions(object s, EventArgs e)
        {
            var strings = new string[] {Langs.Const_Label_Calendar_5, Langs.Const_Label_Calendar_1,
                Langs.Const_Label_Calendar_2, Langs.Const_Label_Calendar_3, Langs.Const_Label_Calendar_4};
            var alert = DisplayActionSheet(Langs.Const_Button_Refine, Langs.Const_Label_Cancel, null, strings
                                           ).ContinueWith((t) =>
                                           {
                                               if (t.IsCompleted)
                                               {
                                                   ViewModel.CurrentSearch = (RefineSearch)Array.IndexOf(strings, t.Result);
                                                   ViewModel.ChangeCalendar();
                                                   Calendar = true;
                                                   var content = titleBar.Content.FindByName<StackLayout>("leftStack");
                    var lbl = content.Children[0] as Label;
                                                   lbl.Text = strings[Array.IndexOf(strings, t.Result)];
                                               }
                                               else
                                                   ViewModel.CurrentSearch = RefineSearch.CalendarMonth;
                                           });
        }
    }
}