using mvvmframework.Languages;
using NewAppyFleet.Converters;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ListViewCells
{
    public class NotificationViewCell : ViewCell
    {
        public NotificationViewCell()
        {
            var lblDate = new Label
            {
                TextColor = Color.White,
                FontSize = 16,
                FontFamily = Helper.RegFont
            };
            lblDate.SetBinding(Label.TextProperty, new Binding("DateString"));

            var lblNumber = new Label
            {
                TextColor = Color.White,
                FontSize = 16,
                FontFamily = Helper.RegFont,
                VerticalTextAlignment= TextAlignment.Center
            };
            lblNumber.SetBinding(Label.TextProperty, new Binding("EventCount"));

            var width = App.ScreenSize.Width * .9;
            var dataGrid = new Grid
            {
                HeightRequest = 48,
                WidthRequest = width,
                HorizontalOptions = LayoutOptions.Center,
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = width * .5},
                    new ColumnDefinition {Width = 1,},
                    new ColumnDefinition {Width = GridLength.Star}
                }
            };

            var imgAlert = new Image
            {
                WidthRequest = 32,
                HeightRequest = 32
            };
            imgAlert.SetBinding(Image.SourceProperty, new Binding("Read", converter: new BoolToAlertImage()));

            dataGrid.Children.Add(new StackLayout { VerticalOptions = LayoutOptions.Center, Children = { lblDate } }, 0, 0);
            dataGrid.Children.Add(new BoxView { HeightRequest = 48, WidthRequest = 1, BackgroundColor = Color.White }, 1, 0);

            var w = (App.ScreenSize.Width * .5) - 1;
            var smallInnerGrid = new Grid
            {
                WidthRequest = w,
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = w * .7},
                    new ColumnDefinition {Width = GridLength.Star}
                }
            };

            smallInnerGrid.Children.Add(new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    lblNumber,
                    new Label {VerticalTextAlignment=TextAlignment.Center, Text = Langs.Const_Label_Warnings, TextColor = Color.White, FontFamily = Helper.RegFont},
                }
            }, 0, 0);
            smallInnerGrid.Children.Add(imgAlert, 1, 0);

            dataGrid.Children.Add(smallInnerGrid,2,0);
            dataGrid.SetBinding(Grid.BackgroundColorProperty, new Binding("Read", converter: new BoolToAlertColor()));  

            View = new StackLayout
            {
                HeightRequest = 52,
                WidthRequest = App.ScreenSize.Width,
                Padding = new Thickness(0, 8),
                HorizontalOptions = LayoutOptions.Center,
                Children = { dataGrid }
            };
        }
    }
}

