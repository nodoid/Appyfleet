using NewAppyFleet.Converters;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ListViewCells
{
    public class MDPListCell : ViewCell
    {
        public MDPListCell()
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

            View = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(4),
                VerticalOptions = LayoutOptions.Center,
                Children = { img, lbl }
            };
        }
    }
}