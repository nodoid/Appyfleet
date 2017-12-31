using System;

using Xamarin.Forms;

namespace NewAppyFleet
{
    public class ArrowBtn
    {
        public static StackLayout ArrowButton(string text, double width, Action click = null,
                                              double height = 40, bool useChevron = false)
        {
            var grid = new Grid
            {
                WidthRequest = width,
                MinimumWidthRequest = width,
                HeightRequest = height,
                BackgroundColor = FormsConstants.AppyBrightBlue,
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
                        WidthRequest = width,
                        MinimumWidthRequest = width,
                        VerticalTextAlignment= TextAlignment.Center,
                        Text = text,
                        FontFamily = Helper.RegFont,
                        TextColor = Color.White
                    }
                }
            };

            var imgStack = new StackLayout
            {
                Padding = new Thickness(12, 0),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End,
                Children =
                {
                    new Image
                    {
                        Source = !useChevron ? "forward_arrow".CorrectedImageSource() : "ic_keyboard_arrow_right_white_24dp".CorrectedImageSource(),
                        HeightRequest = 32,
                        WidthRequest = 32
                    }
                }
            };

            grid.Children.Add(txtStack, 0, 0);
            grid.Children.Add(imgStack, 1, 0);

            if (click != null)
            {
                grid.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    NumberOfTapsRequired = 1,
                    Command = new Command(() => click.Invoke())
                });
            }

            var blankSize = (App.ScreenSize.Width - width) / 2;

            return new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                WidthRequest = App.ScreenSize.Width,
                MinimumWidthRequest = App.ScreenSize.Width,
                HorizontalOptions = LayoutOptions.Center,
                Children = {
                    new BoxView
                    {
                        WidthRequest = blankSize,
                        BackgroundColor = Color.Transparent
                    },
                    grid,
                    new BoxView
                    {
                        WidthRequest = blankSize,
                        BackgroundColor = Color.Transparent
                    }
                }
            };
        }
    }
}

