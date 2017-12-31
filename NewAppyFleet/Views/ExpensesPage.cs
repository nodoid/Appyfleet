using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using mvvmframework.Languages;
using mvvmframework.Models;
using mvvmframework.ViewModels;
using NewAppyFleet.Views;
using NewAppyFleet.Views.ListViewCells;
using NewAppyFleet.Views.ViewCells;
using Xamarin.Forms;

namespace NewAppyFleet
{
    public class ExpensesPage : ContentPage
    {
        ExpensesViewModel ViewModel => App.Locator.Expenses;
        public StackLayout stack;
        StackLayout innerStack;
        GeneralTopBar titleBar;
        ListView lstView;
        Button btnTo, btnFrom;

        void RegisterEvents()
        {
            ViewModel.PropertyChanged += (sender, e) => 
            {
                if (e.PropertyName == "Expenses")
                {
                    if (lstView != null)
                    {
                        Device.BeginInvokeOnMainThread(()=>
                        {
                            lstView.ItemsSource = null;
                            lstView.ItemsSource = ViewModel.Expenses;
                            if (ViewModel.Expenses.Count == 0)
                            {
                                Task.Run(async () => await DisplayAlert(Langs.Const_Title_Error_1, Langs.Const_Msg_No_Journeys, "OK"));
                            }
                        });
                    }
                }
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<JourneyViewCell, string>(this, "JourneyId", 
                                                               async (arg1, arg2) => 
                                                               await Navigation.PushAsync(new MapsPage(Convert.ToInt32(arg2))));

            MessagingCenter.Subscribe<JourneyViewCell, string>(this, "Notifications",
                                                               async (arg1, arg2) =>
                                                               await Navigation.PushAsync(new NotificationsPage(Convert.ToInt32(arg2))));

            MessagingCenter.Subscribe<JourneyViewCell, string>(this, "Selected", (arg1, arg2) => 
            {
                ViewModel.ToggleJourney(Convert.ToInt64(arg2));
            });
            ViewModel.PerformSearch();
            RegisterEvents();
            CreateUI();
        } 

        public ExpensesPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundColor = FormsConstants.AppyDarkBlue;
            BindingContext = ViewModel;
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

            var lstElements = new List<UIAttributes>
            {
                new UIAttributes {Text = Langs.Const_Screen_Title_Journeys, Position = 0},
                new UIAttributes { Text = Langs.Const_Label_Show_Private_Off, Position = 1, ScreenLeft = false},
                new UIAttributes { Text = "switch_button_blue_off".CorrectedImageSource(), IsImageSource = true, Position = 0, ScreenLeft = false,
                    ClickEvent = ToggleButton}
            };
            titleBar = new GeneralTopBar(lstElements);

            var fromDatePicker = new DatePicker
            {
                MinimumDate = DateTime.Now.AddDays(-30),
                //MaximumDate = DateTime.Now.AddDays(-30),
                IsVisible = false
            };
            fromDatePicker.SetBinding(DatePicker.DateProperty, new Binding("StartDate"));

            var toDatePicker = new DatePicker
            {
                MaximumDate = DateTime.Now,
                //MinimumDate = fromDatePicker.MaximumDate,
                IsVisible = false
            };
            toDatePicker.SetBinding(DatePicker.DateProperty, new Binding("EndDate"));

            var spacer = new DatePicker { IsVisible = false };

            var btnSearch = new Button
            {
                BorderRadius = 6,
                BackgroundColor = FormsConstants.AppyBlue,
                TextColor = Color.White,
                Text = Langs.Const_Button_Search,
                HeightRequest = 42
            };
            btnSearch.Clicked += delegate 
            {
                ViewModel.PerformSearch();
            };

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

            lstView = new ListView
            {
                ItemsSource = ViewModel.Expenses,
                WidthRequest = App.ScreenSize.Width * .9,
                ItemTemplate = new DataTemplate(typeof(ExpensesViewCell)),
                SeparatorVisibility = SeparatorVisibility.None,
                HeightRequest = App.ScreenSize.Height * .8,
                HasUnevenRows = true
            };

            if (ViewModel.Expenses.Count == 0)
            {
                Task.Run(async () => await DisplayAlert(Langs.Const_Title_Error_1, Langs.Const_Msg_No_Journeys, "OK"));
            }

            grid.Children.Add(new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = App.ScreenSize.Width,
                Children = { lstView }
            },0,2);

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

        void ToggleButton(object sender, EventArgs e)
        {
            var grid = ((GeneralTopBar)sender).Content;
            var _ = grid as StackLayout;
            var content = _.Children[0] as Grid;

            foreach (var child in content.Children)
            {
                if (child is StackLayout)
                {
                    var inner = child as StackLayout;
                    foreach (var inn in inner.Children)
                    {
                        if (inn is Image)
                        {
                            ((Image)inn).Source = !ViewModel.ShowPrivate ? "switch_button_blue_on".CorrectedImageSource() :
                                                    "switch_button_blue_off".CorrectedImageSource();
                            ViewModel.ShowPrivate = !ViewModel.ShowPrivate;
                            ViewModel.PerformSearch();
                        }
                    }
                }
            }
        }
    }
}

