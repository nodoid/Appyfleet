using Xamarin.Forms;

namespace NewAppyFleet.Views.ViewCells
{
    public class ImageEntryCell : ContentView
    {
        public ImageEntryCell(string imgSource, Entry entry, double width, double height = 40)
        {
            var padbox = PaddingBox.CreatePaddingBox((App.ScreenSize.Width - width) / 2);
            var whiteLine = new BoxView { BackgroundColor = Color.White, WidthRequest = 1, HeightRequest = 32, Margin = new Thickness(0, 4) };

            var stackLine = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new BoxView {HeightRequest = (height - 32) /4 , WidthRequest = 1, BackgroundColor = Color.Transparent},
                    whiteLine,
                    new BoxView {HeightRequest = (height - 32) /4 , WidthRequest = 1, BackgroundColor = Color.Transparent},
                }
            };

            var grid = new Grid
            {
                WidthRequest = width,
                HeightRequest = height,
                BackgroundColor = FormsConstants.AppyBrightBlue
            };

            grid.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition {Width = width / 8},
                new ColumnDefinition {Width = 3},
                new ColumnDefinition {Width = GridLength.Auto}
            };
            grid.RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition {Height = height}
            };

            grid.Children.Add(new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Padding = new Thickness(4),
                Children = 
                {
                    new Image
                    {
                        Source = imgSource.CorrectedImageSource(),
                        HeightRequest = height - 8,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                    }
                }
            }, 0, 0);
            grid.Children.Add(whiteLine, 1, 0);
            grid.Children.Add(entry, 2, 0);

            Content = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                WidthRequest = App.ScreenSize.Width,
                Children = { padbox, grid, padbox }
            };
        }
    }
}

