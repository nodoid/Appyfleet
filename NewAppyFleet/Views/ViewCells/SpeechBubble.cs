using Xamarin.Forms;

namespace NewAppyFleet
{
    public class SpeechBubble : ContentView
    {
        public SpeechBubble(string text, double width, Color bgColor)
        {
            var lblBubble = new RoundedLabel
            {
                Text = text,
                FontFamily = Helper.BoldFont,
                TextColor = Color.Black,
                InsidePadding = new Thickness(16),
                RoundedCornerRadius = 6,
                RoundedBackgroundColor = bgColor,
                WidthRequest = width,
                MinimumWidthRequest = width,
                HorizontalTextAlignment = TextAlignment.Start
            };
            var imgArrow = new Image
            {
                Source = "speech_bubble_corner".CorrectedImageSource(),
                WidthRequest = 32,
                HeightRequest = 32
            };
            var holderView = new StackLayout
            {
                WidthRequest = App.ScreenSize.Width,
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Vertical,
                Children =
                {new StackLayout
                {
                    Children =
                {
                    new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        Children = {lblBubble}
                    },
                    new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.End,
                                TranslationX = -64,
                                TranslationY = -8,
                        Children = {imgArrow }
                    }
                }
                }
                }
            };

            Content = holderView;
        }
    }
}

