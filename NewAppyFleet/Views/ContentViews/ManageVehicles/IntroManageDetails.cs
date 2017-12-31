using System;
using mvvmframework;
using mvvmframework.Languages;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ContentViews.ManageVehicles
{
    public class IntroManageDetails
    {
        public static StackLayout IntroManage(ContentView titleBar, PairNewVehicleViewModel ViewModel)
        {
            var lblTitle = GetUIElement.GetFirstElement<Label>(titleBar.Content as StackLayout);
            lblTitle.Text = Langs.Const_Screen_Title_Manage_Vehicle;

            var arrowButton = ArrowBtn.ArrowButton(Langs.Const_Button_Manage_Vehicle_Step_1, App.ScreenSize.Width * .9, 
                                                   new Action(()=>ViewModel.MoveToSearch = true));
            
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
                        WidthRequest = App.ScreenSize.Width * .8,
                        Children =
                        {
                            new Label
                            {
                                Text = Langs.Const_Msg_Registration_Completed_Pair_Vehicle,
                                TextColor = Color.White,
                                FontFamily = Helper.BoldFont
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
                                        Text = Langs.Const_Msg_Pair_Vehicle_Description_1,
                                        TextColor = Color.White,
                                        FontFamily = Helper.RegFont,
                                        HorizontalTextAlignment = TextAlignment.Center
                                    },
                                    new Label
                                    {
                                        Text = Langs.Const_Msg_Pair_Vehicle_Description_2,
                                        TextColor = Color.White,
                                        FontFamily = Helper.BoldFont,
                                        HorizontalTextAlignment = TextAlignment.Center
                                    }
                                }
                            },
                            new BoxView {WidthRequest = App.ScreenSize.Width, HeightRequest = 1, BackgroundColor= Color.White},
                            new StackLayout
                            {
                                WidthRequest= App.ScreenSize.Width * .8,
                                Padding = new Thickness(0,8),
                                HorizontalOptions = LayoutOptions.Center,
                                Children =
                                {
                                    new Label
                                    {
                                        Text = Langs.Const_Label_Manage_Vehicle_Step_1,
                                        TextColor = Color.White,
                                        FontFamily = Helper.RegFont,
                                        HorizontalTextAlignment = TextAlignment.Center
                                    },
                                    new Label
                                    {
                                        Text = Langs.Const_Label_Manage_Vehicle_Step_2,
                                        TextColor = Color.White,
                                        FontFamily = Helper.BoldFont,
                                        HorizontalTextAlignment = TextAlignment.Center
                                    },
                                    new Label
                                    {
                                        Text = Langs.Const_Label_Manage_Vehicle_Step_3,
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
                                Children = {arrowButton }
                            }
                        }
            };
        }
    }
}
