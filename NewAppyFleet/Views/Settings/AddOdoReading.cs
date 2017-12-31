using mvvmframework;
using mvvmframework.Languages;
using mvvmframework.ViewModels.Settings;
using NewAppyFleet.Views.ListViewCells;
using NewAppyFleet.Views.ViewCells;
using System;

using Xamarin.Forms;

namespace NewAppyFleet.Views.Settings
{
    public class AddOdoReading : ContentPage
    {
        public StackLayout stack;
        StackLayout innerStack;
        ListView odoListView;

        OddometerViewModel ViewModel => App.Locator.Odo;

        void RegisterEvents()
        {
            ViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "Readings")
                {
                    if (odoListView != null)
                    {
                        Device.BeginInvokeOnMainThread(() => { odoListView.ItemsSource = null; odoListView.ItemsSource = ViewModel.Readings; });
                    }
                }
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RegisterEvents();
            ViewModel.GetOdoReadings();
            MessagingCenter.Subscribe<OdoViewCell, string>(this, "odo", (a, b) => ViewModel.RemoveOdo(Convert.ToInt32(b)));
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<OdoViewCell, string>(this, "odo");
        }

        public AddOdoReading()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundColor = FormsConstants.AppyDarkShade;
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
                BackgroundColor = FormsConstants.AppyLightBlue,
                TextColor = Color.White,
                Text = Langs.Const_Button_Add_Odometer_Reading
            };

            var height = App.ScreenSize.Height - (48 * 2);
            var grid = new Grid
            {
                WidthRequest = App.ScreenSize.Width * .9,
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = height * .4},
                    new RowDefinition {Height = 1},
                    new RowDefinition {Height = GridLength.Star},
                }
            };

            var addGrid = new Grid
            {
                WidthRequest = App.ScreenSize.Width * .8,
                HeightRequest = 48,
                BackgroundColor = FormsConstants.AppyDarkBlue,
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = (App.ScreenSize.Width * .8) * .4},
                    new ColumnDefinition {Width = 1},
                    new ColumnDefinition {Width = GridLength.Star}
                }
            };

            var enterOdo = UniversalEntry.GeneralEntryCell("", App.ScreenSize.Width * .8, Keyboard.Numeric, "XXXXX", ReturnKeyTypes.Done);
            enterOdo.SetBinding(Entry.TextProperty, new Binding("Reading"));

            addGrid.Children.Add(new Label { VerticalTextAlignment = TextAlignment.Center, FontSize = 16, TextColor = Color.White, FontFamily = Helper.RegFont, Text = Langs.Const_Label_Odometer }, 0, 0);
            addGrid.Children.Add(new BoxView { WidthRequest = 1, HeightRequest = 40, VerticalOptions = LayoutOptions.Center, BackgroundColor = Color.White }, 1, 0);
            addGrid.Children.Add(enterOdo, 2, 0);

            odoListView = new ListView
            {
                SeparatorVisibility = SeparatorVisibility.None,
                ItemsSource = ViewModel.Readings,
                ItemTemplate = new DataTemplate(typeof(OdoViewCell)),
                HasUnevenRows = true
            };

            var datePicker = new DatePicker();
            datePicker.SetBinding(DatePicker.DateProperty, new Binding("DateAdded"));
            datePicker.SetBinding(DatePicker.IsVisibleProperty, new Binding("ShowDate"));
            datePicker.DateSelected += (s, e) => { ViewModel.DateAdded = e.NewDate; };
            var timePicker = new TimePicker();
            timePicker.SetBinding(TimePicker.TimeProperty, new Binding("TimeAdded"));
            timePicker.SetBinding(TimePicker.IsVisibleProperty, new Binding("ShowTime"));

            var btnDate = new Button
            {
                WidthRequest = (App.ScreenSize.Width * .8) * .5,
                BackgroundColor = FormsConstants.AppyDarkShade,
                BorderRadius = 6,
                Text = Langs.Const_Label_Date,
                TextColor = Color.White
            };
            btnDate.Clicked += delegate { datePicker.Focus(); };
            var btnTime = new Button
            {
                WidthRequest = (App.ScreenSize.Width * .8) * .5,
                BackgroundColor = FormsConstants.AppyDarkShade,
                BorderRadius = 6,
                Text = Langs.Const_Button_Set_Time,
                TextColor = Color.White
            };
            btnTime.Clicked += delegate { timePicker.Focus(); };

            var btnConfirm = new Button
            {
                WidthRequest = App.ScreenSize.Width * .8,
                BorderRadius = 8,
                TextColor = Color.White,
                Text = Langs.Const_Button_Add_Odometer_Reading
            };
            btnConfirm.Clicked += delegate { ViewModel.BtnAddOdometer.Execute(null); };

            grid.Children.Add(
                new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Padding = new Thickness(0,8),
                    Children =
                    {
                        addGrid,
                        new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children = {btnDate, btnTime}
                        },
                        btnConfirm
                    }
                }, 0, 0);
            grid.Children.Add(new BoxView { HeightRequest = 1, WidthRequest = App.ScreenSize.Width, BackgroundColor = Color.White }, 0, 1);
            grid.Children.Add(odoListView, 0, 2);

            stack.Children.Add(grid);
            stack.Children.Add(odoListView);
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
}