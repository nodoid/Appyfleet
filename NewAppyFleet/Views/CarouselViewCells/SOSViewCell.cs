using System;

using Xamarin.Forms;

namespace NewAppyFleet.Views.CarouselViewCells
{
    public class SOSViewCell : ViewCell
    {
        public SOSViewCell()
        {
            var imgCell = new Image
            {
                WidthRequest = App.ScreenSize.Width * .5,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            imgCell.SetBinding(Image.SourceProperty, new Binding("Image"));

            var lblTitle = new Label
            {
                FontFamily = Helper.BoldFont,
                FontSize = 24,
                TextColor = Color.White,
                WidthRequest = App.ScreenSize.Width * .7,
                HorizontalTextAlignment = TextAlignment.Center
            };
            lblTitle.SetBinding(Label.TextProperty, new Binding("Title"));

            var lblMessage = new Label
            {
                FontFamily = Helper.RegFont,
                FontSize = 24,
                TextColor = Color.White,
                WidthRequest = App.ScreenSize.Width * .7,
                HorizontalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.WordWrap,
            };
            lblMessage.SetBinding(Label.TextProperty, new Binding("Message"));

            View = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = App.ScreenSize.Width,
                Children =
                {
                    new StackLayout
                    {
                        Padding = new Thickness(0,16),
                        Children = {imgCell}
                    },
                    new StackLayout
                    {
                        Children = {lblTitle, lblMessage}
                    }
                }
            };
        }
    }
}

