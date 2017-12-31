using mvvmframework.Languages;

using Xamarin.Forms;

namespace NewAppyFleet.Views.ListViewCells
{
    public class OdoViewCell : ViewCell
    {
        public OdoViewCell()
        {
            var grid = new Grid
            {
                WidthRequest = App.ScreenSize.Width * .75,
                HeightRequest = 48,
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = 100},
                    new ColumnDefinition {Width = GridLength.Star},
                    new ColumnDefinition {Width = 80}
                }
            };

            var lblDate = new Label
            {
                TextColor = Color.White,
                FontSize = 14,
                FontFamily = Helper.RegFont
            };
            lblDate.SetBinding(Label.TextProperty, new Binding("DateString"));

            var lblFile = new Label
            {
                TextColor = Color.White,
                FontSize = 14,
                FontFamily = Helper.RegFont
            };
            lblFile.SetBinding(Label.TextProperty, new Binding("OdoReading"));

            var btnSend = new Button
            {
                Text = Langs.Const_Button_Remove,
                TextColor = Color.White,
                FontFamily = Helper.RegFont,
                BackgroundColor = FormsConstants.AppyRed,
                BorderRadius = 8,
                WidthRequest = 86
            };
            btnSend.SetBinding(Button.ClassIdProperty, new Binding("ID"));
            btnSend.Command = new Command(() => MessagingCenter.Send<OdoViewCell, string>(this, "odo", btnSend.ClassId));

            grid.Children.Add(
                new StackLayout { Orientation = StackOrientation.Horizontal, Children = { lblDate, new BoxView { HeightRequest = 48, WidthRequest = 1, BackgroundColor = Color.White } } }, 0, 0);
            grid.Children.Add(lblFile, 1, 0);
            grid.Children.Add(btnSend, 2, 0);

            View = new Frame
            {
                BackgroundColor = FormsConstants.AppyDarkBlue,
                Content = grid
            };
        }
    }
}