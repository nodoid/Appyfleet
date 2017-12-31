using System;
using mvvmframework;
using mvvmframework.Languages;
using Xamarin.Forms;
using NewAppyFleet.Views.ViewCells;

namespace NewAppyFleet.Views.ContentViews.SignUp
{
    public class AccountDetails
    {
        public static StackLayout GenerateAccountDetails(ContentView titleBar, SignUpViewModel ViewModel)
        {
            var lblTitle = GetUIElement.GetFirstElement<Label>(titleBar.Content as StackLayout);
            lblTitle.Text = Langs.Const_Screen_Title_Registration_2;
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
                new RowDefinition {Height = 40},
                new RowDefinition {Height = 40},
            };

            var width = App.ScreenSize.Width * .9;
            var entryWidth = width * .8 - 20;

            var companyNameEntry = UniversalEntry.GeneralEntryCell(string.Empty, width, Keyboard.Default, Langs.Const_Placeholder_Enter, ReturnKeyTypes.Next, .6, true);
            companyNameEntry.SetBinding(Entry.TextProperty, new Binding("CompanyName"));

            var positionEntry = UniversalEntry.GeneralEntryCell(string.Empty, width, Keyboard.Default, Langs.Const_Placeholder_Enter, ReturnKeyTypes.Next, .6, true);
            positionEntry.SetBinding(Entry.TextProperty, new Binding("Position"));

            ViewModel.EmailAddress = string.Empty;

            var emailEntry = UniversalEntry.GeneralEntryCell(string.Empty, width, Keyboard.Email, Langs.Const_Placeholder_Enter, ReturnKeyTypes.Done, .6, true);
            emailEntry.SetBinding(Entry.TextProperty, new Binding("EmailAddress"));

            var arrowButton = ArrowBtn.ArrowButton(Langs.Const_Button_Registration_2_Next, width, new Action(() => { ViewModel.CmdCheckDriver.Execute(null); }));

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
                        var _ = new SpeechBubble(Langs.Const_Msg_Registration_Step_2_Help_Description, width, FormsConstants.AppySilverGray);

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

            masterGrid.SizeChanged += (sender, e) =>
            {
                helpContainer.HeightRequest = App.ScreenSize.Height - 100 - masterGrid.Height;
                inStack.HeightRequest = (App.ScreenSize.Height - 100 - masterGrid.Height) * .7;
            };

            masterGrid.Children.Add(new EntryCell(Langs.Const_Label_Company_Name, companyNameEntry, width), 0, 0);
            masterGrid.Children.Add(new EntryCell(Langs.Const_Placeholder_Position, positionEntry, width), 0, 1);
            masterGrid.Children.Add(new EntryCell(Langs.Const_Label_Email_1, emailEntry, width), 0, 2);
            masterGrid.Children.Add(arrowButton, 0, 3);

            var spinner = new ActivityIndicator
            {
                HeightRequest = 40,
                WidthRequest = 40,
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
                Children = { masterGrid, spinner, helpContainer }
            };
        }
    }
}
