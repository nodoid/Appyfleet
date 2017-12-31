using System;
using mvvmframework;
using mvvmframework.Languages;
using Xamarin.Forms;
using NewAppyFleet.Views.ViewCells;
using NewAppyFleet.Converters;

namespace NewAppyFleet.Views.ContentViews.SignUp
{
    public class SetPasswordDetails
    {
        public static StackLayout GeneratePasswordDetails(ContentView titleBar, SignUpViewModel ViewModel)
        {
            var lblTitle = GetUIElement.GetFirstElement<Label>(titleBar.Content as StackLayout);
            lblTitle.Text = Langs.Const_Screen_Title_Registration_3;
            var masterGrid = new Grid
            {
                RowSpacing = 12,
            };
            masterGrid.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition {Width = App.ScreenSize.Width * .9}
            };
            masterGrid.RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition {Height = 40},
                new RowDefinition {Height = 40},
            };

            var width = App.ScreenSize.Width * .9;
            var entryWidth = width * .8 - 20;

            var passwordOneEntry = UniversalEntry.GeneralEntryCell(string.Empty, width, Keyboard.Default, Langs.Const_Placeholder_Enter, ReturnKeyTypes.Next, .6,true);
            passwordOneEntry.SetBinding(Entry.TextProperty, new Binding("PasswordOne"));
            passwordOneEntry.SetBinding(Entry.IsPasswordProperty, new Binding("ShowPasswords", converter: new ReverseBoolConverter()));

            var passwordTwoEntry = UniversalEntry.GeneralEntryCell(string.Empty, width, Keyboard.Default, Langs.Const_Placeholder_Enter, ReturnKeyTypes.Done, .6,true);
            passwordTwoEntry.SetBinding(Entry.TextProperty, new Binding("PasswordTwo"));
            passwordTwoEntry.SetBinding(Entry.IsPasswordProperty, new Binding("ShowPasswords", converter: new ReverseBoolConverter()));

            var swchShowPassword = new Switch();
            swchShowPassword.SetBinding(Switch.IsToggledProperty, new Binding("ShowPasswords"));

            var lblShowPassword = new Label
            {
                Text = Langs.Const_Label_Show_Password,
                TextColor = Color.White,
                FontFamily = Helper.RegFont
            };

            var swtchStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                WidthRequest = App.ScreenSize.Width * .6,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(0,12),
                Children = { swchShowPassword, lblShowPassword }
            };

            var lblEmail = new Label
            {
                Text = Langs.Const_Msg_Marketing,
                TextColor = Color.White,
                FontFamily = Helper.RegFont,
            };
            var swchMarketting = new Switch();
            swchMarketting.SetBinding(Switch.IsToggledProperty, new Binding("EmailPrefs"));

            var mrktStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                WidthRequest = App.ScreenSize.Width * .85,
                MinimumHeightRequest = 100,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(0, 8),
                Children =
                {
                    lblEmail, swchMarketting
                }
            };

            var arrowButton = ArrowBtn.ArrowButton(Langs.Const_Screen_Title_Registration_4, width, new Action(()=>ViewModel.CanMoveToFour()));

            var midStack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    swtchStack,
                    new BoxView {HeightRequest = 1, BackgroundColor = Color.White},
                    mrktStack,
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
                        var _ = new SpeechBubble(Langs.Const_Msg_Registration_Step_3_Help_Description, width, FormsConstants.AppySilverGray);

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
                    midStack,
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

            masterGrid.SizeChanged += (sender, e) =>
            {
                helpContainer.HeightRequest = App.ScreenSize.Height - 100 - masterGrid.Height;
                inStack.HeightRequest = (App.ScreenSize.Height - 200 - masterGrid.Height) * .4;
            };

            masterGrid.Children.Add(new EntryCell(Langs.Const_Placeholder_Password, passwordOneEntry, width), 0, 0);
            masterGrid.Children.Add(new EntryCell(Langs.Const_Label_ReEnter, passwordTwoEntry, width), 0, 1);

            return new StackLayout
            {
                TranslationX = -6,
                Padding = new Thickness(0, 12, 0, 0),
                Orientation = StackOrientation.Vertical,
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = App.ScreenSize.Height - 100,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                Children = { masterGrid, helpContainer }
            };
        }
    }
}
