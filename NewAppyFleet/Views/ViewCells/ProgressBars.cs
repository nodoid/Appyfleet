using Xamarin.Forms;

namespace NewAppyFleet.Views.ViewCells
{
    public class ProgressBars
    {
        public static Grid GenerateProgressBars(int currentBar)
        {
            var smallBar = new BoxView { WidthRequest = 50, HeightRequest = 2, BackgroundColor = Color.White };
            var largeBar = new BoxView { WidthRequest = 50, HeightRequest = 30, BackgroundColor = Color.White };

            var grid = new Grid
            {
                ColumnSpacing = 5
            };
            grid.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition {Width = GridLength.Auto},
                new ColumnDefinition {Width = GridLength.Auto},
                new ColumnDefinition {Width = GridLength.Auto},
                new ColumnDefinition {Width = GridLength.Auto},
                new ColumnDefinition {Width = GridLength.Auto},
                new ColumnDefinition {Width = GridLength.Auto},
                new ColumnDefinition {Width = GridLength.Auto},
            };

            for (var _ = 0; _ < 7; ++_)
            {
                var box = _ == currentBar ? largeBar : smallBar;
                grid.Children.Add(box, _, 0);
            }

            return grid;
        }
    }
}

