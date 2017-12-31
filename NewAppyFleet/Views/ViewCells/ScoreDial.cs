using NewAppyFleet.CustomViews;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ViewCells
{
    public class ScoreDialView
    {
        public static StackLayout ScoreDial(double score, double angle)
        {
            var sz = App.ScreenSize.Width * .95;

            var outerGrid = new Grid
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                ColumnSpacing = 0,
                RowSpacing = 0,
            };

            var innerGrid = new Grid
            {
                HeightRequest = sz,
                WidthRequest = sz,
                ColumnSpacing = 0,
                RowSpacing = 0,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = sz}
                },
            };

            var imgInner = new CustomImage 
            { 
                ImageSource= "dial_negative_background".CorrectedImageSource(),
                Source = "dial_negative_background".CorrectedImageSource(), 
                Aspect = Aspect.Fill,
                WidthRequest = sz,
                HeightRequest = sz
            };
            innerGrid.Children.Add(imgInner);

            var needle = new CustomImage
            {
                ImageSource = score > 0 ? "positive_needle".CorrectedImageSource() : "negative_needle".CorrectedImageSource(),
                Source = score > 0 ? "positive_needle".CorrectedImageSource() : "negative_needle".CorrectedImageSource(),
                WidthRequest = sz,
                HeightRequest = sz,
                Aspect = Aspect.Fill
            };

            var stackInner = new StackLayout
            {
                //WidthRequest = 200,
                //HeightRequest = 200,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    new Label {Text = $"{score}", TextColor = Color.White, FontSize = 28, FontFamily = Helper.RegFont, HeightRequest = sz, WidthRequest = sz,
                        VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center}
                }
            };

            innerGrid.Children.Add(needle);
            needle.RotateTo(angle);
            innerGrid.Children.Add(stackInner);
            outerGrid.Children.Add(innerGrid);

            return new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = sz,
                WidthRequest = App.ScreenSize.Width,
                Children = { outerGrid }
            };
        }
    }
}


