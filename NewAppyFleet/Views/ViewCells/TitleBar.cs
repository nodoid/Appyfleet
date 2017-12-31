using Xamarin.Forms;

namespace NewAppyFleet
{
    public class TitleBar : ContentView
    {
        public TitleBar(string text)
        {
            Content = new StackLayout
            {
                WidthRequest = MinimumWidthRequest = App.ScreenSize.Width,
                HeightRequest = 40,
                BackgroundColor = FormsConstants.AppyLightBlue,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions= LayoutOptions.Center,
                Children =
                {
                    new Label
                    {
                        Text = text,
                        TextColor = Color.White,
                        FontFamily = Helper.RegFont,
                        HeightRequest = 40,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalTextAlignment = TextAlignment.Center
                    }
                }
            };
        }
    }
}

