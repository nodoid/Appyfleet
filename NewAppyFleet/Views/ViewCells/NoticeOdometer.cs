using mvvmframework.Languages;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ViewCells
{
    public class NoticeOdometer
    {
        public static StackLayout NoticeOdometerCells(int notifications, string odo, string reg)
        {
            var halfWidth = App.ScreenSize.Width/ 2;

            var grid = new Grid
            {
                WidthRequest = App.ScreenSize.Width,
                RowSpacing = 0,
                ColumnSpacing = 0,
                HeightRequest = 40
            };
            grid.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition {Width = halfWidth -1},
                new ColumnDefinition {Width = 1},
                new ColumnDefinition {Width = halfWidth},
            };
            grid.RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition {Height = 1},
                new RowDefinition {Height = 28},
                /*new RowDefinition {Height = 1},
                new RowDefinition {Height = 56},
                new RowDefinition {Height = 1}*/
            };

            var unreadHeader = new TextArrow(Langs.Const_Label_Notifications, halfWidth - 1, 28, true);
            unreadHeader.BackgroundColor = notifications <= 10 ? FormsConstants.AppyDarkRed : FormsConstants.AppyDarkShade;
            var odoHeader = new TextArrow(Langs.Const_Label_Odometer, halfWidth, 28, true);
            odoHeader.BackgroundColor = FormsConstants.AppyDarkShade;

            var horizLine = new BoxView { HeightRequest = 1, BackgroundColor = Color.White };
            var vertLine = new BoxView { HeightRequest = 28, WidthRequest = 1, BackgroundColor = Color.White };

            var noticationStack = new StackLayout
            {
                Padding = new Thickness(12, 12, 0, 12),
                BackgroundColor = notifications <= 10 ? FormsConstants.AppyRed : FormsConstants.AppyDarkShade,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = halfWidth - 1,
                HeightRequest = 56,
                HorizontalOptions = LayoutOptions.Start,
                Children =
                {
                    new Label
                    {
                        //Text = $"{notifications}",
                        Text = Langs.Const_Label_Notifications,
                        TextColor = Color.White,
                        FontFamily = Helper.RegFont,
                        //FontSize = 28
                    }
                }
            };
            noticationStack.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(() => MessagingCenter.Send<string>("notification", "open"))
            });

            var odoStack = new StackLayout
            {
                Padding = new Thickness(12, 12, 0, 12),
                WidthRequest = halfWidth,
                HeightRequest = 56,
                BackgroundColor = FormsConstants.AppyDarkShade,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Start,
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    new Label
                    {
                        //Text = $"Reg {reg}",
                        Text = Langs.Const_Label_Latest_Odometer,
                        TextColor = Color.White,
                        FontFamily = Helper.RegFont,
                        FontSize = 12
                    }/*,new Label
                    {
                        Text = odo,
                        TextColor = Color.White,
                        FontFamily = Helper.RegFont,
                        FontSize = 18
                    },*/
                }
            };

            /*grid.Children.Add(unreadHeader, 0, 0);
            grid.Children.Add(vertLine, 1, 0);
            grid.Children.Add(odoHeader, 2, 0);
            grid.Children.Add(horizLine, 1, 1);*/
            grid.Children.Add(noticationStack, 0, 1);

            grid.Children.Add(odoStack, 2, 1);
            grid.Children.Add(vertLine, 1, 1);
            grid.Children.Add(horizLine, 0, 0);

            Grid.SetColumnSpan(horizLine, 4);
            Grid.SetRowSpan(vertLine, 2);

            return new StackLayout
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = 40,
                Children = { grid }
            };
        }
    }
}
