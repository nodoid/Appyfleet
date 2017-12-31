using System;
using mvvmframework;
using mvvmframework.Languages;
using NewAppyFleet.Views.ViewCells;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ContentViews.ManageVehicles
{
    public class AddVehicleDetails
    {
        public static StackLayout AddVehicle(ContentView titleBar, PairNewVehicleViewModel ViewModel)
        {
            var lblTitle = GetUIElement.GetFirstElement<Label>(titleBar.Content as StackLayout);
            lblTitle.Text = Langs.Const_Label_Add_Vehicle;

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
                new RowDefinition {Height = 40},
                new RowDefinition {Height = 40}
            };

            var width = App.ScreenSize.Width * .9;
            var entryWidth = width * .8 - 20;

            var vehicleRegEntry = UniversalEntry.GeneralEntryCell(string.Empty, width,Keyboard.Default, Langs.Const_Placeholder_Enter,ReturnKeyTypes.Next, .6,true);
            vehicleRegEntry.SetBinding(Entry.TextProperty, new Binding("Registration"));

            var vehicleMakeEntry = UniversalEntry.GeneralEntryCell(string.Empty, width,Keyboard.Default, Langs.Const_Placeholder_Enter, ReturnKeyTypes.Next, .6,true);
            vehicleMakeEntry.SetBinding(Entry.TextProperty, new Binding("Make"));

            var vehicleModelEntry = UniversalEntry.GeneralEntryCell(string.Empty, width,Keyboard.Default, Langs.Const_Placeholder_Enter, ReturnKeyTypes.Next, .6,true);
            vehicleModelEntry.SetBinding(Entry.TextProperty, new Binding("Model"));

            var vehicleNicknameEntry = UniversalEntry.GeneralEntryCell(string.Empty, width,Keyboard.Default, Langs.Const_Placeholder_Enter, ReturnKeyTypes.Next, .6,true);
            vehicleNicknameEntry.SetBinding(Entry.TextProperty, new Binding("Nickname"));

            var vehicleOdoEntry = UniversalEntry.GeneralEntryCell(string.Empty, width, Keyboard.Numeric, "XXXXX", ReturnKeyTypes.Done, .6, true);
            vehicleOdoEntry.SetBinding(Entry.TextProperty, new Binding("Odometer"));

            var arrowButton = ArrowBtn.ArrowButton(Langs.Const_Button_Create_Vehicle, width, new Action(()=>ViewModel.CanMoveToPairing()));

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
                        var _ = new SpeechBubble(Langs.Const_Msg_Pair_Vehicle_Help_Description_2, width, FormsConstants.AppySilverGray);

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

            masterGrid.Children.Add(new EntryCell(Langs.Const_Label_Registration, vehicleRegEntry, width), 0, 0);
            masterGrid.Children.Add(new EntryCell(Langs.Const_Label_Make, vehicleMakeEntry, width), 0, 1);
            masterGrid.Children.Add(new EntryCell(Langs.Const_Label_Model, vehicleModelEntry, width), 0, 2);
            masterGrid.Children.Add(new EntryCell(Langs.Const_Label_Vehicle_Nickname, vehicleNicknameEntry, width), 0, 3);
            masterGrid.Children.Add(new EntryCell(Langs.Const_Label_Odometer, vehicleOdoEntry, width), 0, 4);
            masterGrid.Children.Add(arrowButton, 0, 5);

            var spinner = new ActivityIndicator
            {
                BackgroundColor = Color.White,
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
                Children = { masterGrid, spinner, helpContainer }
            };
        }
    }
}
