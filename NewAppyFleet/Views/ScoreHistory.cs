using System;
using System.Collections.Generic;
using mvvmframework.Languages;
using mvvmframework.ViewModels;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Xamarin.Forms;
using Xamarin.Forms;

namespace NewAppyFleet.Views
{
    public class ScoreHistory : ContentPage
    {
        ScoreHistoryViewModel ViewModel => App.Locator.ScoreHistory;
        public StackLayout stack;
        StackLayout innerStack;
        Button btnTo, btnFrom;
        PlotView plotView;

        void RegisterEvents()
        {
            ViewModel.PropertyChanged += (sender, e) => 
            {
                if (e.PropertyName == "StartDate")
                {
                    if (btnFrom != null)
                    {
                        Device.BeginInvokeOnMainThread(() => btnFrom.Text = ViewModel.StartDateText);
                    }
                }
                if (e.PropertyName == "EndDate")
                {
                    if (btnTo != null)
                        Device.BeginInvokeOnMainThread(() => btnTo.Text = ViewModel.EndDateText);
                }
            };

        }

        public ScoreHistory()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundColor = FormsConstants.AppyDarkBlue;
            BindingContext = ViewModel;
            RegisterEvents();
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
            };

            var topbar = new TopBar(true, "", this, 1, "back_arrow", "", innerStack, true).CreateTopBar();
            stack.HeightRequest = App.ScreenSize.Height - topbar.HeightRequest;

            var titleBar = new Label
            {
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = 48,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                BackgroundColor = FormsConstants.AppyBlue,
                TextColor = Color.White,
                Text = Langs.Const_Menu_ScoreHistory
            };

            var fromDatePicker = new DatePicker
            {
                MinimumDate = DateTime.Now.AddDays(-30),
                //MaximumDate = DateTime.Now.AddDays(-30),
                IsVisible = false
            };
            fromDatePicker.DateSelected += (sender, e) => ViewModel.StartDate = e.NewDate;

            var toDatePicker = new DatePicker
            {
                MaximumDate = DateTime.Now,
                //MinimumDate = fromDatePicker.MaximumDate,
                IsVisible = false
            };
            toDatePicker.DateSelected += (sender, e) => ViewModel.EndDate = e.NewDate;

            var spacer = new DatePicker { IsVisible = false };

            var btnSearch = new Button
            {
                BorderRadius = 6,
                BackgroundColor = FormsConstants.AppyBlue,
                TextColor = Color.White,
                Text = Langs.Const_Button_Search,
                HeightRequest = 42
            };
            btnSearch.Clicked += (sender, e) => plotView.Model = new DataModel().BarModel;

            btnFrom = new Button
            {
                BackgroundColor = FormsConstants.AppyDarkShade,
                TextColor = Color.White,
                BorderRadius = 6,
                HeightRequest = 42
            };
            btnFrom.SetBinding(Button.TextProperty, new Binding("StartDateText"));
            btnFrom.Clicked += delegate 
            {
                fromDatePicker.Focus();
            };

            btnTo = new Button
            {
                BackgroundColor = FormsConstants.AppyDarkShade,
                TextColor = Color.White,
                HeightRequest = 42,
                BorderRadius = 6
            };
            btnTo.SetBinding(Button.TextProperty, new Binding("EndDateText"));
            btnTo.Clicked += delegate {
                toDatePicker.Focus();
            };

            plotView = new PlotView
            {
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = App.ScreenSize.Height - 154,
                Model = new DataModel().BarModel,
            };

            var grid = new Grid
            {
                WidthRequest = App.ScreenSize.Width,
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = App.ScreenSize.Height * .1},
                    new RowDefinition {Height = 1},
                    new RowDefinition {Height = 36},
                    new RowDefinition {Height = GridLength.Star}
                }
            };

            grid.Children.Add(new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new StackLayout
                    {
                        Orientation = StackOrientation.Vertical,
                        Children= 
                        {
                            btnFrom,
                            fromDatePicker
                        }
                    },
                    new Image {Source = "date_arrow".CorrectedImageSource(), HeightRequest = 16, WidthRequest = 16},
                    new StackLayout
                    {
                        Orientation = StackOrientation.Vertical,
                        Children = 
                        {
                            btnTo,
                            toDatePicker
                        }
                    },
                    new StackLayout
                    {
                        Orientation = StackOrientation.Vertical,
                        Children =
                        {
                            btnSearch,
                            spacer
                        }
                    }
                }
            }, 0, 0);
            grid.Children.Add(new BoxView { HeightRequest = 1, WidthRequest = App.ScreenSize.Width, BackgroundColor = Color.White }, 0, 1);

            var lblDate = new Label
            {
                FontFamily = Helper.RegFont,
                FontSize = 12,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Center
            };
            lblDate.SetBinding(Label.TextProperty, new Binding("DateRange"));

            grid.Children.Add(new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Children = { lblDate }
            }, 0, 2);

            grid.Children.Add(plotView, 0, 3);

            stack.Children.Add(grid);
            innerStack.Children.Add(stack);

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
    }

    public class DataModel
    {
        ScoreHistoryViewModel ViewModel => App.Locator.ScoreHistory;

        public PlotModel BarModel { get; set; }
        public DataModel()
        {
            BarModel = CreateBarModel();
        }

        PlotModel CreateBarModel()
        {
            var plot = new PlotModel 
            { 
                LegendTextColor = OxyColors.White,
                LegendFont = Helper.RegFont,
                Series = 
                {
                    new LineSeries
                    {
                        ItemsSource = DataSeries,
                        MarkerType = MarkerType.Circle, 
                        MarkerFill = OxyColors.Green,
                    }
                }
            };
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            return plot;
        }

        List<DataPoint> DataSeries
        {
            get
            {
                var t = new List<DataPoint>();
                var data = ViewModel.GetScoreData;

                if (data.Count != 0)
                {
                    foreach (var dp in data)
                        t.Add(new DataPoint(dp.Day, dp.Score));
                }

                return t;
            }
        }
    }
}

