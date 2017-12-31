using mvvmframework.ViewModels;
using NewAppyFleet.Views.CarouselViewCells;
using NewAppyFleet.Views.ViewCells;
using Xamarin.Forms;

namespace NewAppyFleet.Views
{
    public class SOSPage : ContentPage
    {
        SOSViewModel ViewModel => App.Locator.SOS;
        Grid BoxGrid;
        public StackLayout stack;
        StackLayout innerStack, mainInnerStack;

        void RegisterEvents()
        {
            ViewModel.PropertyChanged += (sender, e) => 
            {
                if (e.PropertyName == "CurrentViewPage")
                {
                    Device.BeginInvokeOnMainThread(()=> BoxGrid = ProgressBars.GenerateProgressBars(ViewModel.CurrentViewPage));
                }
            };
        }

        public SOSPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundColor = FormsConstants.AppyDarkBlue;
            RegisterEvents();
            ViewModel.CurrentViewPage = 0;
            CreateUI();
        }

        void CreateUI()
        {
            var list = ViewModel.GetSOSListModel;
            foreach (var l in list)
                l.Image = l.Image.CorrectedImageSource();

            var carousel = new CarouselView
            {
                ItemsSource = list,
                HeightRequest = App.ScreenSize.Height * .7,
                IsEnabled = true,
                ItemTemplate = new DataTemplate(typeof(SOSViewCell))
            };

            carousel.PositionSelected += Carousel_PositionSelected;

            BoxGrid = ProgressBars.GenerateProgressBars(ViewModel.CurrentViewPage);

            Content = new StackLayout
            {
                WidthRequest = App.ScreenSize.Width,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Start,
                Children =
                {
                    new TopBar(true, "", this, 1, "back_arrow", "icon_user").CreateTopBar(),
                    new TitleBar("SOS - What to do?"),
                    new StackLayout
                    {
                        HeightRequest = App.ScreenSize.Height * .15,
                        VerticalOptions = LayoutOptions.Center,
                        Children =
                        {
                            new Label {Text = ViewModel.GetSOSMainTitle, TextColor = Color.White, FontSize = 24, FontFamily = Helper.RegFont, HorizontalTextAlignment = TextAlignment.Center}
                        }
                    },
                    new BoxView{WidthRequest = App.ScreenSize.Width, HeightRequest =1, BackgroundColor = Color.White},
                    new StackLayout
                    {
                        Padding = new Thickness(0,24),
                        Children = {carousel, BoxGrid}
                    }
                }
            };
        }

        void Carousel_PositionSelected(object sender, SelectedPositionChangedEventArgs e)
        {
            ViewModel.CurrentViewPage = (int)e.SelectedPosition;
        }
    }
}

