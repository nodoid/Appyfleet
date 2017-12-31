using mvvmframework;
using mvvmframework.Languages;
using mvvmframework.ViewModels.Settings;
using NewAppyFleet.Views.ViewCells;
using System;
using Xamarin.Forms;

namespace NewAppyFleet.Views.Settings
{
    public class ChangeFleetCode : ContentPage
    {
        FleetCodeViewModel ViewModel => App.Locator.FleetCode;
        public StackLayout stack;
        StackLayout innerStack;

        public ChangeFleetCode()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundColor = FormsConstants.AppyDarkShade;
            BindingContext = ViewModel;
            ViewModel.NewFleetCode = ViewModel.FleetNumber;
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
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                BackgroundColor = FormsConstants.AppyLightBlue,
                TextColor = Color.White,
                Text = Langs.Const_Label_Fleet_Code
            };

            var spinner = new ActivityIndicator
            {
                HeightRequest = 40,
                WidthRequest = 40,
                IsRunning = true
            };
            spinner.SetBinding(ActivityIndicator.IsVisibleProperty, new Binding("IsBusy"));

            var enterFleet = UniversalEntry.GeneralEntryCell("", App.ScreenSize.Width * .8, Keyboard.Default, Langs.Const_Label_Enter_Fleet_Code, ReturnKeyTypes.Done);
            enterFleet.SetBinding(Entry.TextProperty, new Binding("NewFleetCode"));

            var arrowButton = ArrowBtn.ArrowButton(Langs.Const_Button_Confirm_3, App.ScreenSize.Width * .8, new Action(() => ViewModel.BtnFleetCode.Execute(null)));
            arrowButton.SetBinding(Button.IsEnabledProperty, new Binding("CanSubmit"));

            var width = App.ScreenSize.Width * .9;
            var grid = new Grid
            {
                WidthRequest = width,
                HeightRequest = 52,
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = width},
                },
                RowDefinitions= new RowDefinitionCollection
                {
                    new RowDefinition {Height = 40}
                }
            };

            grid.Children.Add(new EntryCell(Langs.Const_Label_Fleet_Code, enterFleet, width), 0, 0);

            var dataStack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                WidthRequest = App.ScreenSize.Width,
                HorizontalOptions = LayoutOptions.Center,
                Padding = new Thickness(0, 12),
                Children = { grid, arrowButton, spinner }
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
    }
}