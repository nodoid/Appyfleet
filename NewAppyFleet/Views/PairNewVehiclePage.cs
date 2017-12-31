using System.Diagnostics;
using mvvmframework;
using NewAppyFleet.Views.ContentViews.ManageVehicles;
using Xamarin.Forms;

namespace NewAppyFleet
{
    public class PairNewVehiclePage : ContentPage
    {
        PairNewVehicleViewModel ViewModel => App.Locator.PairNewVehicle;

        public StackLayout stack;
        StackLayout innerStack, mainInnerStack;
        ContentView titleBar;
        bool FromStart;

        void RegisterEvents()
        {
            int n = 0;
            ViewModel.PropertyChanged += async (sender, e) =>
            {
                switch (e.PropertyName)
                {
                    case "MoveToSearch":
                        Debug.WriteLine($"MoveToSearch = {mainInnerStack?.Children.Count}");
                        if (ViewModel.MoveToSearch)
                        {
                            mainInnerStack?.Children.RemoveAt(1);
                            mainInnerStack?.Children.Add(SearchSelectAddDetails.SearchSelectAddVehicle(titleBar, ViewModel));
                        }
                        break;
                    case "MoveToAdd":
                        Debug.WriteLine($"MoveToSearch = {mainInnerStack?.Children.Count}");
                        if (ViewModel.MoveToAdd)
                        {
                            if (mainInnerStack?.Children.Count > 1)
                            mainInnerStack?.Children.RemoveAt(1);
                            mainInnerStack?.Children.Add(AddVehicleDetails.AddVehicle(titleBar, ViewModel));
                        }
                        break;
                    case "MoveToPair":
                        Debug.WriteLine($"[in] MoveToSearch = {mainInnerStack?.Children.Count}");
                        if (ViewModel.MoveToPair)
                        {
                            mainInnerStack?.Children.RemoveAt(1);
                            mainInnerStack?.Children.Add(FindBluetoothPairingDetails.FindBluetooth(titleBar, ViewModel));
                            ViewModel.PopulateBasedOnId();
                        }
                        break;
                    case "MoveToSummary":
                        if (ViewModel.MoveToSummary)
                        {
                            mainInnerStack?.Children.RemoveAt(1);
                            mainInnerStack?.Children.Add(VehicleSummaryDetails.VehicleSummary(titleBar, ViewModel));
                        }
                        break;
                    case "MoveToPairing":
                        if (ViewModel.MoveToPairing)
                        {
                            mainInnerStack?.Children.RemoveAt(1);
                            mainInnerStack?.Children.Add(PairingToDevice.PairToDevice(titleBar, ViewModel));
                        }
                        break;
                    case "MoveToComplete":
                        if (ViewModel.MoveToComplete)
                        {
                            mainInnerStack?.Children.RemoveAt(1);
                            mainInnerStack?.Children.Add(PairingCompleted.PairingComplete(titleBar, ViewModel));
                        }
                        break;
                    case "MoveToLogin":
                        if (ViewModel.MoveToLogin)
                        {
                            await Navigation.PushAsync(new LoginPage());
                            ViewModel.ResetFlags();
                        }
                        break;
                }
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = ViewModel;
            ViewModel.GetVehiclesFromDb();
            RegisterEvents();
            CreateUI();
        }

        public PairNewVehiclePage(bool fromStart = true)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundColor = FormsConstants.AppyBlue;
            FromStart = fromStart;
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
                IsVisible = false,
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
                InputTransparent = true,
                TranslationY = -6
            };

            var topbar = new TopBar(!FromStart, "", this, 1, "", "", innerStack).CreateTopBar();
            stack.HeightRequest = App.ScreenSize.Height - topbar.HeightRequest;

            titleBar = new TitleBar("");

            mainInnerStack.Children.Add(titleBar);
            if (!FromStart)
                mainInnerStack.Children.Add(SearchSelectAddDetails.SearchSelectAddVehicle(titleBar, ViewModel));
            else
            {
                mainInnerStack.Children.Add(IntroManageDetails.IntroManage(titleBar, ViewModel));
            }

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
                }
            };

            Content = masterStack;

            if (FromStart)
                ViewModel.MoveToSearch = true;
        }
    }
}

