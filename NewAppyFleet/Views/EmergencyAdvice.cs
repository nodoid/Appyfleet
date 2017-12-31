using System;
using System.Diagnostics;
using mvvmframework.Languages;
using mvvmframework.ViewModels;
using NewAppyFleet.CustomViews;
using Xamarin.Forms;

namespace NewAppyFleet.Views
{
    public class EmergencyAdvice : ContentPage
    {
        EmergencyAdviceViewModel ViewModel => App.Locator.Emergency;

        public StackLayout stack;
        StackLayout innerStack;
        ContentView view1, view2, view3, view4, view5, view6, view7;
        SliderView slider;

        public EmergencyAdvice()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundColor = FormsConstants.AppyDarkBlue;
            CreateUI();
        }

        void CreateUI()
        {
            stack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = App.ScreenSize.Height - 48,
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            innerStack = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Orientation = StackOrientation.Horizontal,
                MinimumWidthRequest = App.ScreenSize.Width,
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = App.ScreenSize.Height - 48,
            };

            var topbar = new TopBar(true, "", this, 1, "back_arrow", "", innerStack, true).CreateTopBar();
            stack.HeightRequest = App.ScreenSize.Height - topbar.HeightRequest;

            var titleBar = new Label
            {
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = 48,
                MinimumHeightRequest = 48,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                BackgroundColor = FormsConstants.AppyBlue,
                TextColor = Color.White,
                Text = Langs.Const_Menu_EmergencyAdvice
            };

            view1 = CreateView("icon_0", Langs.Const_Msg_SOS_Title_1, Langs.Const_Msg_SOS_Description_1, 1, .55); 
            view2 = CreateView("icon_1", Langs.Const_Msg_SOS_Title_2, Langs.Const_Msg_SOS_Description_2, 1, .55);
            view3 = CreateView("icon_2", Langs.Const_Msg_SOS_Title_3, Langs.Const_Msg_SOS_Description_3, 1, .55);
            view4 = CreateView("icon_3", Langs.Const_Msg_SOS_Title_4, Langs.Const_Msg_SOS_Description_4, 1, .55);
            view5 = CreateView("icon_4", Langs.Const_Msg_SOS_Title_5, Langs.Const_Msg_SOS_Description_5, 1, .55);
            view6 = CreateView("icon_5", Langs.Const_Msg_SOS_Title_6, Langs.Const_Msg_SOS_Description_6, 1, .55);
            view7 = CreateView("icon_6", Langs.Const_Msg_SOS_Title_7, Langs.Const_Msg_SOS_Description_7, 1, .4);

            slider = new SliderView(view1, App.ScreenSize.Height * 0.62, App.ScreenSize.Width)
            {
                TransitionLength = 200,
                MinimumSwipeDistance = 50
            };

            slider.Children.Add(view2);
            slider.Children.Add(view3);
            slider.Children.Add(view4);
            slider.Children.Add(view5);
            slider.Children.Add(view6);
            slider.Children.Add(view7);

            var btnSendSOS = ArrowBtn.ArrowButton(Langs.Const_Button_Send_Incident, App.ScreenSize.Width * .7,
                                                  new Action(()=>ViewModel.SendSMS()));

            var dataStack = new StackLayout
            {
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = App.ScreenSize.Height - 120,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label {Text = Langs.Const_Label_Guide_Steps, HorizontalTextAlignment = TextAlignment.Center, FontFamily = Helper.RegFont,
                        TextColor = Color.White,FontSize = 14},
                    new BoxView {WidthRequest = App.ScreenSize.Width, HeightRequest = 1, BackgroundColor = Color.White},
                    slider,
                    new BoxView {WidthRequest = App.ScreenSize.Width, HeightRequest = 1, BackgroundColor = Color.White},
                    btnSendSOS
                }
            };

            stack.Children.Add(dataStack);

            innerStack.Children.Add(stack);

            var masterStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                Children =
                {
                    new StackLayout
                    {
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalOptions = LayoutOptions.Start,
                        WidthRequest = App.ScreenSize.Width,
                        Children = { topbar }
                    },
                    new StackLayout
                    {
                        TranslationY = -6,
                        Children = {titleBar}
                    },
                    innerStack
                }
            };

            Content = masterStack;
        }

        ContentView CreateView(string imageName, string mainHeading, string text, double width = 1, double multiplier = .8, double height = .4)
        {
            var img = new Image();
            try
            {
                img.Source = imageName.CorrectedImageSource();
                img.HeightRequest = App.ScreenSize.Height * height;
                img.WidthRequest = App.ScreenSize.Width;
                img.HorizontalOptions = LayoutOptions.CenterAndExpand;
                img.Aspect = width == 1 ? Aspect.AspectFit : Aspect.Fill;
            }
            catch (OutOfMemoryException ex)
            {
#if DEBUG
                Debug.WriteLine("Out of memory - {0}::{1}", ex.Message, ex.InnerException);
#endif
            }

            var newstack = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(0),
                StyleId = mainHeading,
                Children =
                {
                                new StackLayout
                                {
                                    Children =
                                    {
                                        new Label
                                        {
                                            Text = mainHeading,
                                TextColor = Color.White,
                                FontFamily = Helper.BoldFont,
                                            LineBreakMode = LineBreakMode.WordWrap,
                                            WidthRequest = App.ScreenSize.Width * (width / 1.5),
                                            HorizontalTextAlignment = TextAlignment.Center,
                                            HorizontalOptions = LayoutOptions.Center
                                        },
                                        new Label
                                        {
                                            Text = text,
                                            TextColor = Color.White,
                                FontFamily = Helper.RegFont,
                                            FontSize = 14,
                                            HorizontalTextAlignment = TextAlignment.Center,
                                            WidthRequest = App.ScreenSize.Width * .9,
                                            LineBreakMode = LineBreakMode.WordWrap,
                                            HorizontalOptions = LayoutOptions.Center
                                        }
                        }
                    }
                }
            };


            return new ContentView
            {
                Content = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    VerticalOptions = LayoutOptions.Start,
                    Children =
                    {
                        new StackLayout
                        {
                            Padding = new Thickness(0),
                            Children =
                            {
                                img
                            }
                        },
                        newstack
                    }
                }
            };
        }
    }
}

