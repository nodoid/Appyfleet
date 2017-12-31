using System;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ListViewCells
{
    public class BluetoothViewCell : ViewCell
    {
        public BluetoothViewCell()
        {
            var lblBluetooth = new Label
            {
                TextColor = Color.White,
                FontFamily = Helper.RegFont
            };
            lblBluetooth.SetBinding(Label.TextProperty, new Binding("Name"));
            var img = new Image
            {
                Source = "close_tranparent".CorrectedImageSource(),
                HorizontalOptions = LayoutOptions.End,
                HeightRequest = 32
            };
            var dataStack = new StackLayout
            {
                WidthRequest = App.ScreenSize.Width * .9,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                //BackgroundColor = FormsConstants.AppyDarkShade,
                HeightRequest = 40,
                Children =
                {
                    new StackLayout
                    {
                        WidthRequest = (App.ScreenSize.Width *.9)*.75,
                        Children = {lblBluetooth}
                    },
                    new StackLayout
                    {
                        WidthRequest = (App.ScreenSize.Width *.9)*.25,
                        Children = {img}
                    },
                }
            };
            dataStack.SetBinding(StackLayout.ClassIdProperty, new Binding("Id"));

            View = new StackLayout
            {
                WidthRequest = App.ScreenSize.Width,
                HorizontalOptions = LayoutOptions.Center,
                TranslationX = 6,
                Children = { dataStack }
            };
        }
    }
}
