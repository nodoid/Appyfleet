using System;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ListViewCells
{
    public class RegisteredVehicleViewCell : ViewCell
    {
        public RegisteredVehicleViewCell()
        {
            var lblNickname = new Label
            {
                FontFamily = Helper.BoldFont,
                TextColor = Color.White
            };
            lblNickname.SetBinding(Label.TextProperty, new Binding("Nickname"));

            var lblRegistration = new Label
            {
                FontFamily = Helper.RegFont,
                TextColor = Color.White
            };
            lblRegistration.SetBinding(Label.TextProperty, new Binding("Registration"));

            var lblMake = new Label
            { 
                FontFamily = Helper.RegFont,
                TextColor = Color.White
            };
            lblMake.SetBinding(Label.TextProperty, new Binding("Make"));

            var lblModel = new Label
            {
                FontFamily = Helper.RegFont,
                TextColor = Color.White
            };
            lblMake.SetBinding(Label.TextProperty, new Binding("Model"));

            View = new StackLayout
            {
                Padding = new Thickness(8),
                Children =
                {
                    new StackLayout
                    {
                        Padding= new Thickness(12),
                        BackgroundColor = FormsConstants.AppyDarkShade,
                        Children =
                        {
                            lblNickname,
                            lblRegistration,
                            new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                Children = {lblMake, lblModel}
                            }
                        }
                    }
                }
            };
        }
    }
}
