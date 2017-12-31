using System;
using mvvmframework;
using mvvmframework.Languages;
using NewAppyFleet.UIHelpers;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ContentViews.ManageVehicles
{
    public class VehicleSummaryDetails
    {
        public static StackLayout VehicleSummary(ContentView titleBar, PairNewVehicleViewModel ViewModel)
        {
            var lblTitle = GetUIElement.GetFirstElement<Label>(titleBar.Content as StackLayout);
            lblTitle.Text = Langs.Const_Screen_Title_Pairing_Summary;

            var arrowButton = ArrowBtn.ArrowButton(Langs.Const_Button_Manage_Vehicle_Step_4, App.ScreenSize.Width * .9, new Action(()=>ViewModel.MoveToComplete = true));

            return new StackLayout
            {
                WidthRequest = App.ScreenSize.Width,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        Padding = new Thickness(0,12),
                        Children =
                        {
                            new Label
                            {
                                FormattedText = FormattedProgress.GenerateProgress(true,true,true),
                                TextColor = Color.White,
                                FontFamily = Helper.BoldFont
                            }
                        }
                    },
                    new BoxView{WidthRequest = App.ScreenSize.Width, HeightRequest = 1, BackgroundColor = Color.White},
                    new StackLayout
                    {
                        Padding = new Thickness(16,8,0,16),
                        WidthRequest = App.ScreenSize.Width * .8,
                        Children =
                        {
                            new Label
                            {
                                Text = Langs.Const_Label_Review_Info,
                                FontFamily = Helper.BoldFont,
                                TextColor = Color.White
                            }
                        }
                    },
                    new StackLayout
                    {
                        WidthRequest = App.ScreenSize.Width * .8,
                        Padding = new Thickness(16,12),
                        Children =
                        {
                            new Label
                            {
                                Text = $"{ViewModel.Nickname}",
                                TextColor = Color.White,
                                FontFamily = Helper.BoldFont
                            },
                            new Label
                            {
                                Text = ViewModel.Registration,
                                FontFamily = Helper.RegFont,
                                TextColor = Color.White
                            },
                            new Label
                            {
                                Text = $"{ViewModel.Make} {ViewModel.Model}",
                                TextColor = Color.White,
                                FontFamily = Helper.RegFont
                            },
                        }
                    },
                    new StackLayout
                    {
                        Padding = new Thickness(0,16,0,8),
                        WidthRequest = App.ScreenSize.Width * .8,
                        IsVisible = ViewModel.Paired,
                        Children =
                        {
                            new Label
                            {
                                Text = Langs.Const_Label_Bluetooth_Registered,
                                TextColor = Color.White,
                                FontFamily = Helper.BoldFont
                            }
                        }
                    },
                    new BoxView{WidthRequest = App.ScreenSize.Width, HeightRequest = 1, BackgroundColor = Color.White},
                    new StackLayout
                    {
                        Padding = new Thickness(0,12),
                        WidthRequest = App.ScreenSize.Width * .8,
                        Children =
                        {
                            new Label
                            {
                                Text = Langs.Const_Label_Confirm_Pairing,
                                TextColor = Color.White,
                                FontFamily = Helper.RegFont
                            }
                        }
                    },
                    arrowButton
                }
            };
        }
    }
}
