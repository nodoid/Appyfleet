using mvvmframework.Languages;
using Xamarin.Forms;

namespace NewAppyFleet.Views
{
    public class ErrorPage : ContentPage
    {
        string ErrorMessage { get; set; }

        public ErrorPage(string message)
        {
            ErrorMessage = !string.IsNullOrEmpty(message) ? message : string.Empty;
            NavigationPage.SetHasNavigationBar(this, false);
            CreateUI();
            BackgroundColor = FormsConstants.AppyLightBlue;
        }

        void CreateUI()
        {
            var masterGrid = new Grid
            {
                WidthRequest = App.ScreenSize.Width * .8,
                VerticalOptions = LayoutOptions.Center,
            };
            masterGrid.RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition {Height = App.ScreenSize.Height * .3},
                new RowDefinition {Height = App.ScreenSize.Height * .5},
                new RowDefinition {Height = App.ScreenSize.Height * .2},
            };

            masterGrid.Children.Add(new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new Image
                    {
                        Source = "app_icon1".CorrectedImageSource(),
                        HeightRequest = 72,
                        VerticalOptions = LayoutOptions.Center
                    }
                }
            }, 0, 0);

            var dataFrame = new Frame
            {
                WidthRequest = App.ScreenSize.Width * .9,
                BackgroundColor = FormsConstants.AppyLightBlue,
                Padding = new Thickness(0),
                HasShadow = false,
                Content = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    Padding = new Thickness(0, 8),
                    HorizontalOptions = LayoutOptions.Center,
                    Children =
                    {
                        new Label
                        {
                            Text = Langs.Const_Title_Error_1,
                            FontSize = 18,
                            TextColor = Color.White,
                            FontFamily = Helper.BoldFont,
                            HorizontalTextAlignment = TextAlignment.Center
                        },
                        new BoxView {HeightRequest = 1, BackgroundColor = Color.White, WidthRequest = App.ScreenSize.Width * .9},
                        new Label
                        {
                            Text = ErrorMessage,
                            FontSize = 14,
                            FontFamily = Helper.RegFont,
                            TextColor = Color.White,
                            HorizontalTextAlignment = TextAlignment.Center
                        }
                    }
                }
            };
            masterGrid.Children.Add(new StackLayout
            {
                WidthRequest = App.ScreenSize.Width,
                HorizontalOptions = LayoutOptions.Center,
                Children = { dataFrame }
            }, 0, 1);

            var imgRegister = new Image
            {
                Source = "close_button".CorrectedImageSource(),
                HeightRequest = 52
            };
            imgRegister.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(async () => { await Navigation.PopAsync(); Navigation.RemovePage(this); })
            });

            masterGrid.Children.Add(new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = App.ScreenSize.Height * .2,
                Padding = new Thickness(0, 4),
                Children =
                {
                    imgRegister,
                    new Label
                    {
                        Text = Langs.Const_Button_Try_Again,
                        TextColor = Color.White,
                        FontFamily= Helper.RegFont,
                        HorizontalTextAlignment = TextAlignment.Center
                    }
                }
            }, 0, 2);

            Content = new StackLayout
            {
                WidthRequest = App.ScreenSize.Width,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                Children = { masterGrid }
            };
        }
    }
}