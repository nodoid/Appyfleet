using System;

using Xamarin.Forms;

namespace NewAppyFleet
{
    public class TextArrow : ContentView
    {
        public TextArrow(string text, double width, double height = 40, bool useChevron = false)
        {
            var grid = new Grid
            {
                WidthRequest = width,
                MinimumWidthRequest = width,
                HeightRequest = height,
            };
            grid.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition {Width = width * .8},
                new ColumnDefinition {Width = GridLength.Auto}
            };
            grid.RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition {Height = GridLength.Star}
            };

            var txtStack = new StackLayout
            {
                Padding = new Thickness(12, 0),
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label
                    {
                        WidthRequest = MinimumWidthRequest = width,
                        VerticalTextAlignment= TextAlignment.Center,
                        Text = text,
                        FontFamily = Helper.RegFont,
                        TextColor = Color.White
                    }
                }
            };
            var imgStack = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End,
                Children =
                {
                    new Image
                    {
                        Source = !useChevron ? "forward_arrow".CorrectedImageSource() : "ic_keyboard_arrow_right_white_24dp".CorrectedImageSource(),
                        HeightRequest = 32,
                        WidthRequest =32
                    }
                }
            };

            grid.Children.Add(txtStack, 0, 0);
            grid.Children.Add(imgStack, 1, 0);

            var blankSize = width / 4;

            var masterStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                WidthRequest = width,
                MinimumWidthRequest = App.ScreenSize.Width,
                HorizontalOptions = LayoutOptions.Start,
                Children = {
                    grid,
                }
            };

            Content = masterStack;
        }
    }
}

