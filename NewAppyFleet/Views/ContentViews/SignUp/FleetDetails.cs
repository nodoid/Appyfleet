using System;
using mvvmframework;
using mvvmframework.Languages;
using NewAppyFleet.Views.ViewCells;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ContentViews.SignUp
{
    public class FleetDetails
    {
        public static StackLayout GenerateFleetDetails(ContentView titleBar, SignUpViewModel ViewModel)
        {
            var lblTitle = GetUIElement.GetFirstElement<Label>(titleBar.Content as StackLayout);
            lblTitle.Text = Langs.Const_Screen_Title_Registration_4;

            var entryFleetCode = UniversalEntry.GeneralEntryCell(string.Empty, App.ScreenSize.Width * .9, Keyboard.Default, Langs.Const_Placeholder_Fleet_Code, ReturnKeyTypes.Done, .6,true);
            entryFleetCode.SetBinding(Entry.TextProperty, new Binding("FleetCode"));
            entryFleetCode.SetBinding(Entry.IsEnabledProperty, new Binding("HasFleetCode"));

            var swtchHasFleetCode = new Switch();
            swtchHasFleetCode.SetBinding(Switch.IsToggledProperty, new Binding("HasFleetCode"));

            var width = App.ScreenSize.Width * .9;

            var arrowButton = ArrowBtn.ArrowButton(Langs.Const_Button_Registration_4_Next, width, new Action(()=>ViewModel.CmdRegisterUser.Execute(null)));

            var topStack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                WidthRequest = App.ScreenSize.Width,
                HorizontalOptions = LayoutOptions.Center,
                Padding = new Thickness(0, 12),
                Children =
                {
                    new StackLayout
                    {
                        WidthRequest = App.ScreenSize.Width * .8,
                        Padding = new Thickness(8,0),
                        Children =
                        {
                            new Label
                            {
                                Text = Langs.Const_Label_Fleet_Code_Question,
                                TextColor = Color.White,
                                FontFamily = Helper.RegFont,
                                HorizontalTextAlignment = TextAlignment.Center
                            }
                        }
                    },
                    new BoxView{HeightRequest = 1, WidthRequest = App.ScreenSize.Width, BackgroundColor = Color.White},
                    new StackLayout
                    {
                        WidthRequest = App.ScreenSize.Width * .8,
                        Children =
                        {
                            new Label
                            {
                                Text = Langs.Const_Label_Skip_Fleet_Code,
                                TextColor = Color.White,
                                FontFamily = Helper.RegFont,
                                HorizontalTextAlignment = TextAlignment.Center
                            },
                            entryFleetCode
                        }
                    },
                    new BoxView{HeightRequest = 1, WidthRequest = App.ScreenSize.Width, BackgroundColor = Color.White},
                    new StackLayout
                    {
                        WidthRequest =App.ScreenSize.Width * .8,
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.Center,
                        Children =
                        {
                            swtchHasFleetCode,
                            new Label
                            {
                                Text = Langs.Const_Label_Disable_Fleet_Code,
                                TextColor = Color.White,
                                FontFamily = Helper.RegFont
                            }
                        }
                    },
                    new BoxView{HeightRequest = 1, WidthRequest = App.ScreenSize.Width, BackgroundColor = Color.White},
                    arrowButton
                }
            };


            var inStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                HeightRequest = 100,
                Children = { new Label { Text = " " } }
            };

            var imgHelp = new Image
            {
                Source = "help".CorrectedImageSource(),
                HeightRequest = 32
            };
            var imgHelpGesture = new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(() =>
                {
                    var src = imgHelp.Source as FileImageSource;

                    if (src.File == "help".CorrectedImageSource())
                    {
                        var _ = new SpeechBubble(Langs.Const_Msg_Registration_Step_4_Help_Description, width, FormsConstants.AppySilverGray);

                        if (inStack.Children.Count > 0)
                            inStack.Children.RemoveAt(0);
                        inStack.Children.Add(_);
                        imgHelp.Source = "help_close".CorrectedImageSource();
                    }
                    else
                    {
                        imgHelp.Source = "help".CorrectedImageSource();
                        inStack.Children.RemoveAt(0);
                    }
                })
            };
            imgHelp.GestureRecognizers.Add(imgHelpGesture);

            var helpContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = 100,
                HorizontalOptions = LayoutOptions.Start,
                Children =
                {
                    inStack,
                    new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.End,
                        HeightRequest = 64,
                        VerticalOptions = LayoutOptions.Start,
                        Children = {imgHelp}
                    }
                }
            };

            var spinner = new ActivityIndicator
            {
                BackgroundColor = Color.Transparent,
                HeightRequest = 40,
                IsRunning = true
            };
            spinner.SetBinding(ActivityIndicator.IsVisibleProperty, new Binding("IsBusy"));

            return new StackLayout
            {
                TranslationX = -6,
                Padding = new Thickness(0, 12, 0, 0),
                Orientation = StackOrientation.Vertical,
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = App.ScreenSize.Height - 100,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                Children = { topStack, spinner, helpContainer }
            };
        }
    }
}
