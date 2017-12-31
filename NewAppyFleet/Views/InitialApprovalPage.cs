using mvvmframework.ViewModels;
using NewAppyFleet.Views.ContentViews;
using Xamarin.Forms;
using System.ComponentModel;

namespace NewAppyFleet.Views
{
    public class InitialApprovalPage : ContentPage
    {
        InitialApprovalViewModel ViewModel => App.Locator.InternalApproval;
        public StackLayout stack;
        StackLayout innerStack, mainInnerStack;
        ContentView titleBar;

        void RegisterEvents()
        {
            ViewModel.PropertyChanged += async (object s, PropertyChangedEventArgs e) =>
            {
                if (e.PropertyName == "OkGo")
                {
                    if (ViewModel.OkGo)
                        await Navigation.PushAsync(new SignUpPage());
                }
            };
        }

        public InitialApprovalPage()
        {
            CreateUI();
            RegisterEvents();
            BackgroundColor = FormsConstants.AppyLightBlue;
            NavigationPage.SetHasNavigationBar(this, false);
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
                InputTransparent = true,
                IsVisible = false,
                HeightRequest = App.ScreenSize.Height - 48,
            };

            mainInnerStack = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Vertical,
                MinimumWidthRequest = App.ScreenSize.Width,
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = App.ScreenSize.Height - 48,
                InputTransparent = true,
                TranslationX = 6,
                //TranslationY = -8
            };

            var topbar = new TopBar(false, "", this, 1, "", "", innerStack).CreateTopBar();
            stack.HeightRequest = App.ScreenSize.Height - topbar.HeightRequest;

            titleBar = new TitleBar("");

            mainInnerStack.Children.Add(titleBar);

            var view = InitialApprovalView.InitialApproval(titleBar, ViewModel);

            mainInnerStack.Children.Add(view);

            //innerStack.Children.Add(mainInnerStack);

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
                    //innerStack,
                    mainInnerStack
                    //innerStack
                }
            };

            Content = masterStack;
        }
    }
}
