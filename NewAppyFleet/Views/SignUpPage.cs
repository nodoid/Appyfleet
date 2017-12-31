using mvvmframework;
using NewAppyFleet.Views.ContentViews.SignUp;
using Xamarin.Forms;

namespace NewAppyFleet
{
    public class SignUpPage : ContentPage
    {
        public StackLayout stack;
        StackLayout innerStack, mainInnerStack;
        ContentView titleBar;

        SignUpViewModel ViewModel => App.Locator.Signup;

        void RegisterEvents()
        {
            ViewModel.PropertyChanged += async (sender, e) => 
            {
                switch (e.PropertyName)
                {
                    case "MoveToTwo":
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            ViewModel.EmailAddress = string.Empty;
                            mainInnerStack?.Children.RemoveAt(1);
                            mainInnerStack?.Children.Add(AccountDetails.GenerateAccountDetails(titleBar, ViewModel));
                        });
                        break;
                    case "MoveToThree":
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            mainInnerStack?.Children.RemoveAt(1);
                            mainInnerStack?.Children.Add(SetPasswordDetails.GeneratePasswordDetails(titleBar, ViewModel));
                        });
                        break;
                    case "MoveToFour":
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            mainInnerStack?.Children.RemoveAt(1);
                            mainInnerStack?.Children.Add(FleetDetails.GenerateFleetDetails(titleBar, ViewModel));
                        });
                        break;
                    case "AllDone":
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            mainInnerStack?.Children.RemoveAt(1);
                            mainInnerStack?.Children.Add(SignupCompleted.SignupDetailsCompleted(titleBar, ViewModel));
                        });
                        break;
                    case "MoveToPairing":
                        await Navigation.PushAsync(new PairNewVehiclePage());
                        break;
                }
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = ViewModel;
            ViewModel.ShowPasswords = false;
            RegisterEvents();
            CreateUI();
        }

        public SignUpPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundColor = FormsConstants.AppyBlue;
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

            mainInnerStack = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Vertical,
                MinimumWidthRequest = App.ScreenSize.Width,
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = App.ScreenSize.Height - 48,
                //TranslationX = -6,
                TranslationY = -8
            };

            var topbar = new TopBar(false, "", this, 1, "", "", innerStack).CreateTopBar();
            stack.HeightRequest = App.ScreenSize.Height - topbar.HeightRequest;

            titleBar = new TitleBar("");

            mainInnerStack.Children.Add(titleBar);
            mainInnerStack.Children.Add(PersonalDetails.GeneratePersonalDetails(titleBar, ViewModel));

            innerStack.Children.Add(mainInnerStack);

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
                    mainInnerStack
                    //innerStack
                }
            };

            Content = masterStack;
        }
    }
}

