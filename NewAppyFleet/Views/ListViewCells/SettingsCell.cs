using NewAppyFleet.Converters;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ListViewCells
{
    public class SettingsListCell : ViewCell
    {
        public SettingsListCell()
        {
            var img = new Image
            {
                HeightRequest = 36,
            };
            img.SetBinding(Image.SourceProperty, new Binding("image", converter: new ImageFilenameConverter()));

            var lbl = new Label
            {
                TextColor = Color.White,
                FontSize = 14,
                FontFamily = Helper.RegFont,
                VerticalTextAlignment= TextAlignment.Center
            };
            lbl.SetBinding(Label.TextProperty, new Binding("text"));

            var listGrid = new Grid
            {
                WidthRequest = App.ScreenSize.Width * .8,
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = 48},
                    new ColumnDefinition {Width = GridLength.Star},
                    new ColumnDefinition {Width = 48}
                }
            };
            listGrid.Children.Add(img,0,0);
            listGrid.Children.Add(lbl, 1, 0);
            listGrid.Children.Add(new Image { Source = "link_arrow".CorrectedImageSource(), HeightRequest = 36},2,0);

            View = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(0,8),
                Children = { listGrid }
            };
        }
    }
}