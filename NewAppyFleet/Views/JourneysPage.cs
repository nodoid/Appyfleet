using System;
using System.Collections.Generic;
using mvvmframework.Languages;
using mvvmframework.Models;
using mvvmframework.ViewModels;
using NewAppyFleet.Views.ListViewCells;
using NewAppyFleet.Views.ViewCells;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace NewAppyFleet.Views
{
    public class JourneyPage : ContentPage
    {
        JourneysViewModel ViewModel => App.Locator.Journeys;
        //ListView journeysList;
        GeneralTopBar titleBar;
        public StackLayout stack;
        StackLayout innerStack;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //ViewModel.CreateSortedJourneys();
            Task.Run(async () => await ViewModel.GetJourneyData().ContinueWith((_)=>Device.BeginInvokeOnMainThread(()=>CreateUI())));
            MessagingCenter.Subscribe<string, string>("JourneyListCell", "JourneyId", async (arg1, arg2) => await Navigation.PushAsync(new MapsPage(Convert.ToInt32(arg2))));
            //CreateUI();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<string, string>("JourneyListCell", "JourneyId");
        }

        public JourneyPage() 
        {
            //RegisterEvents();
            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundColor = FormsConstants.AppyDarkBlue;
            BindingContext = ViewModel;
            //CreateUI();
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

            var topbar = new TopBar(true, "", this, 1, "burger_menu", "refresh_icon", innerStack).CreateTopBar();
            stack.HeightRequest = App.ScreenSize.Height - topbar.HeightRequest;

            var lstElements = new List<UIAttributes>
            {
                new UIAttributes {Text = Langs.Const_Screen_Title_Journeys, Position = 0},
                new UIAttributes { Text = Langs.Const_Label_Show_Private_Off, Position = 1, ScreenLeft = false},
                new UIAttributes { Text = "switch_button_blue_off".CorrectedImageSource(), IsImageSource = true, Position = 0, ScreenLeft = false, 
                    ClickEvent = ToggleButton}
            };
            titleBar = new GeneralTopBar(lstElements);

            var scrollData = new ScrollView
            {
                WidthRequest = App.ScreenSize.Width,
                HorizontalOptions = LayoutOptions.Center
            };

            var dataStack = new StackLayout
            {
                WidthRequest = App.ScreenSize.Width,
                MinimumWidthRequest = App.ScreenSize.Width,
                Padding = new Thickness(0, 4),
            };
            foreach (var t in ViewModel.SortedJourneys)
                dataStack.Children.Add(JourneyListViewCell.JourneyListCell(t));

            scrollData.Content = dataStack;
            innerStack.Children.Add(scrollData);

            var masterStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                HeightRequest = App.ScreenSize.Height,
                IsEnabled = true,
                Children =
                {
                    topbar,
                    new StackLayout
                    {
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalOptions = LayoutOptions.Start,
                        WidthRequest = App.ScreenSize.Width,
                        TranslationY = -6,
                        Children = { titleBar }
                    },
                    innerStack
                }
            };

            //Content = masterStack;
            Content = masterStack;
        }

        void ToggleButton(object sender, EventArgs e)
        {
            var grid = ((GeneralTopBar)sender).Content;
            var _ = grid as StackLayout;
            var content = _.Children[0] as Grid;

            foreach(var child in content.Children)
            {
                if (child is StackLayout)
                {
                        var inner = child as StackLayout;
                        foreach(var inn in inner.Children)
                        {
                            if (inn is Image)
                            {
((Image)inn).Source = !ViewModel.ShowPrivate ? "switch_button_blue_on".CorrectedImageSource() :
                        "switch_button_blue_off".CorrectedImageSource();
                    ViewModel.ShowPrivate = !ViewModel.ShowPrivate;
                            }
                        }
                    }
            }
        }
    }
}

