using mvvmframework.Languages;
using mvvmframework.ViewModels.Settings;
using NewAppyFleet.Views.ListViewCells;
using System;
using Xamarin.Forms;

namespace NewAppyFleet.Views.Settings
{
    public class ChangeOdometer : ContentPage
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
            MessagingCenter.Subscribe<OdoViewCell, string>(this, "odo", (a,b)=>ViewModel.RemoveOdo(Convert.ToInt32(b)));
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<OdoViewCell, string>(this, "odo");
        }

        public ChangeOdometer()
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
                Text = Langs.Const_Screen_Title_Odometer_Reading
            };

            var height = App.ScreenSize.Height - (48 * 2);
            var grid = new Grid
            {
                WidthRequest = App.ScreenSize.Width * .9,
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = height * .25},
                    new RowDefinition {Height = 1},
                    new RowDefinition {Height = GridLength.Star},
                }
            };

            var addGrid = new Grid
            {
                WidthRequest = App.ScreenSize.Width * .9,
                HeightRequest = 48,
                BackgroundColor = FormsConstants.AppyDarkBlue,
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = (App.ScreenSize.Width * .9) * .8},
                    new ColumnDefinition {Width = GridLength.Auto}
                }
            };

            addGrid.Children.Add(new Label { VerticalTextAlignment = TextAlignment.Center, FontSize = 16, TextColor = Color.White, FontFamily = Helper.RegFont, Text = Langs.Const_Button_Add_Odometer_Reading }, 0, 0);
            addGrid.Children.Add(new Image { HeightRequest = 36, Source = "add_odometer_reading".CorrectedImageSource() }, 1, 0);

            addGrid.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(async () => await Navigation.PushAsync(new AddOdoReading()))
            });

            odoListView = new ListView
            {
                SeparatorVisibility = SeparatorVisibility.None,
                ItemsSource = ViewModel.Readings,
                HeightRequest = App.ScreenSize.Height * .6,
                ItemTemplate = new DataTemplate(typeof(OdoViewCell)),
                HasUnevenRows = true
            };

            grid.Children.Add(
                new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Children = { addGrid }
                }, 0, 0);
            grid.Children.Add(new BoxView { HeightRequest = 1, WidthRequest = App.ScreenSize.Width, BackgroundColor = Color.White },0,1);
            grid.Children.Add(odoListView, 0, 2);

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
}