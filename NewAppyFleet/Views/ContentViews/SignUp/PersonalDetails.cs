using System;
using mvvmframework;
using mvvmframework.Languages;
using NewAppyFleet.Views.ViewCells;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ContentViews.SignUp
{
    public class PersonalDetails
    {
        public static StackLayout GeneratePersonalDetails(ContentView titleBar, SignUpViewModel ViewModel)
        {
            var lblTitle = GetUIElement.GetFirstElement<Label>(titleBar.Content as StackLayout);
            lblTitle.Text = Langs.Const_Screen_Title_Registration_1;
            var masterGrid = new Grid
            {
                RowSpacing = 12,
                InputTransparent = true
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
                new RowDefinition {Height = 40},
            };

            var width = App.ScreenSize.Width * .9;
            var entryWidth = width * .8 - 20;
            var firstNameEntry = UniversalEntry.GeneralEntryCell(string.Empty, width, Keyboard.Default, Langs.Const_Placeholder_Enter, ReturnKeyTypes.Next, .6,true);
            firstNameEntry.SetBinding(Entry.TextProperty, new Binding("FirstName"));

            var lastNameEntry = UniversalEntry.GeneralEntryCell(string.Empty, width,Keyboard.Default, Langs.Const_Placeholder_Enter, ReturnKeyTypes.Next, .6,true);
            lastNameEntry.SetBinding(Entry.TextProperty, new Binding("LastName"));

            var yearOfBirthEntry = UniversalEntry.GeneralEntryCell(string.Empty, width, Keyboard.Numeric, Langs.Const_Placeholder_Enter, ReturnKeyTypes.Next, .6,true);
            yearOfBirthEntry.SetBinding(Entry.TextProperty, new Binding("YOB"));

            var mobileEntry = UniversalEntry.GeneralEntryCell(string.Empty, width, Keyboard.Telephone, Langs.Const_Placeholder_Enter, ReturnKeyTypes.Done, .6, true);
            mobileEntry.SetBinding(Entry.TextProperty, new Binding("MobNumber"));

            var arrowButton = ArrowBtn.ArrowButton(Langs.Const_Button_Registration_1_Next, width, new Action(()=>ViewModel.CanMoveToTwo()));

            /*var arrowGestureRecogniser = new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(() => ViewModel.CanMoveToTwo())
            };
            arrowButton.GestureRecognizers.Add(arrowGestureRecogniser);*/

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
                        var _ = new SpeechBubble(Langs.Const_Msg_Registration_Step_1_Help_Description, width, FormsConstants.AppySilverGray);

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

            masterGrid.Children.Add(new EntryCell(Langs.Const_Label_First_Name, firstNameEntry, width), 0, 0);
            masterGrid.Children.Add(new EntryCell(Langs.Const_Label_Last_Name, lastNameEntry, width), 0, 1);
            masterGrid.Children.Add(new EntryCell(Langs.Const_Placeholder_DOB, yearOfBirthEntry, width), 0, 2);
            masterGrid.Children.Add(new EntryCell(Langs.Const_Label_Mobile_Number, mobileEntry, width), 0, 3);
            masterGrid.Children.Add(arrowButton, 0, 4);

            return new StackLayout
            {
                TranslationX = -6,
                Padding = new Thickness(0, 12, 0, 0),
                Orientation = StackOrientation.Vertical,
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = App.ScreenSize.Height - 100,
                HorizontalOptions = LayoutOptions.Center,
                InputTransparent = true,
                VerticalOptions = LayoutOptions.Start,
                Children = { masterGrid, helpContainer }
            };
        }
    }
}