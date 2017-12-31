using System;
using mvvmframework;
using mvvmframework.Languages;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ContentViews.SignUp
{
    public class SignupCompleted
    {
        public static StackLayout SignupDetailsCompleted(ContentView titleBar, SignUpViewModel ViewModel)
        {
            var lblTitle = GetUIElement.GetFirstElement<Label>(titleBar.Content as StackLayout);
            lblTitle.Text = Langs.Const_Screen_Title_Registration_Completed;

            var arrowButton = ArrowBtn.ArrowButton(Langs.Const_Button_Pair_Vehicle, App.ScreenSize.Width * .9, new Action(()=>ViewModel.MoveToPairing = true));

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
                        WidthRequest = App.ScreenSize.Width * .9,
                        Children =
                        {
                            new StackLayout
                            {
                                HeightRequest = App.ScreenSize.Height * .3,
                                HorizontalOptions = LayoutOptions.Center,
                                VerticalOptions = LayoutOptions.Center,
                                Children =
                                {
                                    new Image
                                    {
                                        Source = "accept_icon".CorrectedImageSource(),
                                        HeightRequest = 64
                                    },
                                }
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
                                        Text = Langs.Const_Msg_Registration_Completed_Description,
                                        TextColor = Color.White,
                                        FontFamily = Helper.RegFont,
                                        HorizontalTextAlignment = TextAlignment.Center
                                    },
                                    new Label
                                    {
                                        Text = Langs.Const_Msg_Registration_Completed_Pair_Vehicle,
                                        TextColor = Color.White,
                                        FontFamily = Helper.BoldFont,
                                        HorizontalTextAlignment = TextAlignment.Center
                                    }
                                }
                            }
                        }
                    },
                    new StackLayout
                    {
                        Padding  = new Thickness(0,12),
                        TranslationX = -6,
                        Children = {arrowButton }
                    }
                }
            };
        }
    }
}
