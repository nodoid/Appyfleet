using mvvmframework.Languages;
using mvvmframework.Models;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ViewCells
{
    public class MileageCell : ViewCell
    {
        public static StackLayout MileageView(Mileage mileage)
        {
            var width = App.ScreenSize.Width * .95;
            var grid = new Grid
            {
                ColumnSpacing = 0,
                RowSpacing = 0,
                WidthRequest = width
            };

            var fs = App.ScreenSize.Width;

            grid.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition{ Width = fs  / 2},
                new ColumnDefinition{Width = 1},
                new ColumnDefinition{Width = (fs / 2)-1}
            };

            grid.RowDefinitions = new RowDefinitionCollection
            {
                //new RowDefinition {Height = 28},
                //new RowDefinition {Height = 36},
                new RowDefinition {Height = 28},
                new RowDefinition {Height = 36}
            };

            /*var lblMiles = new Label
            {
                FontSize = 28,
                FontFamily = Helper.RegFont,
                TextColor = Color.White,
                Text = $"{mileage.TotalMiles}"
            };
            var lblMilesUnit = new Label
            {
                FontFamily = Helper.BoldFont,
                TextColor = Color.White,
                Text = Langs.Const_Label_Miles
            };*/
            var lblMilesUnit1 = new Label
            {
                FontFamily = Helper.BoldFont,
                TextColor = Color.White,
                Text = Langs.Const_Label_Miles
            };
            var lblMilesUnit2 = new Label
            {
                FontFamily = Helper.BoldFont,
                TextColor = Color.White,
                Text = Langs.Const_Label_Miles
            };
            /*var stackMiles = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Start,
                Children = { lblMiles, lblMilesUnit }
            };*/

            var lblBusinessMiles = new Label
            {
                FontSize = 28,
                FontFamily = Helper.RegFont,
                TextColor = Color.White,
                Text = $"{mileage.TotalMiles - mileage.PersonalMiles}"
            };

            var lblPersonalMiles = new Label
            {
                FontSize = 28,
                FontFamily = Helper.RegFont,
                TextColor = Color.White,
                Text = $"{mileage.PersonalMiles}"
            };

            var stackBusiness = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Start,
                WidthRequest = width /2,
                Children = { lblBusinessMiles, lblMilesUnit1 }
            };

            var stackPersonal = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Start,
                WidthRequest = width /2,
                Children = { lblPersonalMiles, lblMilesUnit2 }
            };

            var titleStack = new StackLayout
            {
                WidthRequest = width,
                BackgroundColor = FormsConstants.AppyDarkShade,
                Padding = new Thickness(8, 0),
                Children = { new Label { Text = Langs.Const_Label_Total_Mileage, TextColor = Color.White, FontFamily = Helper.RegFont } }
            };

            //grid.Children.Add(titleStack, 0, 0);
            //grid.Children.Add(stackMiles, 0, 1);
            grid.Children.Add(
                new Label
                {
                    Text = Langs.Const_Label_Business_Mileage,
                    FontFamily = Helper.RegFont,
                    TextColor = Color.White,
                    BackgroundColor = FormsConstants.AppyBlue,
                WidthRequest = width / 2
                }, 0, 0);
            var box = new BoxView { WidthRequest = 1, BackgroundColor = Color.White };
            grid.Children.Add(box, 1, 0);
            grid.Children.Add(
                new Label
                {
                    Text = Langs.Const_Label_Personal_Mileage,
                    FontFamily = Helper.RegFont,
                    TextColor = Color.White,
                    BackgroundColor = FormsConstants.AppyBlue,
                WidthRequest = width / 2
                }, 2,0);

            grid.Children.Add(stackBusiness, 0, 1);
            grid.Children.Add(stackPersonal, 2, 1);

            Grid.SetRowSpan(titleStack, 2);
            //Grid.SetRowSpan(stackMiles, 2);
            Grid.SetRowSpan(box, 2);

            return new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = App.ScreenSize.Width,
                Children = { grid }
            };
        }
    }
}
