using System;
using mvvmframework;
using mvvmframework.Languages;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ContentViews.ManageVehicles
{
    public class PairingCompleted
    {
        public static StackLayout PairingComplete(ContentView titleBar, PairNewVehicleViewModel ViewModel)
        {
            var lblTitle = GetUIElement.GetFirstElement<Label>(titleBar.Content as StackLayout);
            lblTitle.Text = Langs.Const_Screen_Title_Pairing_Completed;

            var arrowButton = ArrowBtn.ArrowButton(Langs.Const_Button_Manage_Vehicle_Step_5, App.ScreenSize.Width * .9, 
                                                   new Action(()=>ViewModel.MoveToLogin = true));

            return new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                WidthRequest = App.ScreenSize.Width,
                Children =
                {
                    new StackLayout
                    {
                        Padding = new Thickness(0,12),
                        HorizontalOptions = LayoutOptions.Center,
                        Children =
                        {
                            new Image
                            {
                                Source = "accept_icon".CorrectedImageSource(),
                                HeightRequest = 64
                            },
                            new BoxView {WidthRequest = App.ScreenSize.Width, HeightRequest = 1, BackgroundColor= Color.White},
                            new StackLayout
                            {
                                WidthRequest= App.ScreenSize.Width * .8,
                                Padding = new Thickness(0,8),
                                Children =
                                {
                                    new Label
                                    {
                                        Text = Langs.Const_Label_Pairing_Completed,
                                        TextColor = Color.White,
                                        FontFamily = Helper.BoldFont,
                                        HorizontalTextAlignment = TextAlignment.Center
                                    },
                                }
                            },
                            new StackLayout
                            {
                                Padding  = new Thickness(0,12),
                                Children = {arrowButton }
                            }
                        }
                    }
                }
            };
        }
    }
}
