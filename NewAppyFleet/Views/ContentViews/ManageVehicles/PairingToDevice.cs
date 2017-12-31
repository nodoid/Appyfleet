using System;
using mvvmframework;
using mvvmframework.Languages;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ContentViews.ManageVehicles
{
    public class PairingToDevice
    {
        public static StackLayout PairToDevice(ContentView titleBar, PairNewVehicleViewModel ViewModel)
        {
            var lblTitle = GetUIElement.GetFirstElement<Label>(titleBar.Content as StackLayout);
            lblTitle.Text = Langs.Const_Label_Pairing;

            var activitySpinner = new ActivityIndicator
            {
                BackgroundColor = Color.White,
                IsRunning = true
            };
            activitySpinner.SetBinding(ActivityIndicator.IsVisibleProperty, new Binding("IsBusy"));

            var stackSpinning = new StackLayout
            {
                WidthRequest = App.ScreenSize.Width * .6,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    activitySpinner,
                    new Label
                    {
                        Text = Langs.Const_Label_Pairing,
                        HorizontalTextAlignment = TextAlignment.Center,
                        TextColor = Color.White,
                        FontFamily = Helper.BoldFont
                    }
                }
            };


            return new StackLayout
            {
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = App.ScreenSize.Height - 102,
                TranslationX = -6,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children = { stackSpinning }
            };
        }
    }
}
