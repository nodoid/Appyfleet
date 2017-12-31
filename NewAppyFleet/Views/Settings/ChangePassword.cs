using mvvmframework;
using mvvmframework.Languages;
using mvvmframework.ViewModels.Settings;
using NewAppyFleet.Converters;
using NewAppyFleet.Views.ViewCells;
using System;
using Xamarin.Forms;

namespace NewAppyFleet.Views.Settings
{
    public class ChangePassword : BasePage
    {
        ChangePasswordViewModel ViewModel => App.Locator.Password;
        public StackLayout stack;
        StackLayout innerStack;

        public ChangePassword()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundColor = FormsConstants.AppyDarkShade;
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
                Text = Langs.Const_Label_Change_Password
            };

            var height = App.ScreenSize.Height - (48 * 2);
            var grid = new Grid
            {
                WidthRequest = App.ScreenSize.Width * .9,
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = height * .25},
                    new RowDefinition {Height = 1},
                    new RowDefinition {Height = GridLength.Star},
                    new RowDefinition {Height = 1},
                    new RowDefinition {Height = height * .25}
                }
            };

            var spinner = new ActivityIndicator
            {
                HeightRequest = 40,
                WidthRequest = 40,
                IsRunning = true
            };
            spinner.SetBinding(ActivityIndicator.IsVisibleProperty, new Binding("IsBusy"));

            var enterCurrentPassword = UniversalEntry.GeneralEntryCell("", App.ScreenSize.Width * .8, Keyboard.Default, Langs.Const_Placeholder_Company_Name, ReturnKeyTypes.Done);
            enterCurrentPassword.SetBinding(Entry.TextProperty, new Binding("CurrentPassword"));
            enterCurrentPassword.SetBinding(Entry.IsPasswordProperty, new Binding("HidePassword", converter: new ReverseBoolConverter()));

            var enterNewPassword = UniversalEntry.GeneralEntryCell("", App.ScreenSize.Width * .8, Keyboard.Default, Langs.Const_Placeholder_New_Password, ReturnKeyTypes.Done);
            enterNewPassword.SetBinding(Entry.TextProperty, new Binding("NewPasswordOne"));
            enterNewPassword.SetBinding(Entry.IsEnabledProperty, new Binding("PasswordEnabled"));
            enterNewPassword.SetBinding(Entry.IsPasswordProperty, new Binding("HidePassword", converter: new ReverseBoolConverter()));

            var enterNewPassword2 = UniversalEntry.GeneralEntryCell("", App.ScreenSize.Width * .8, Keyboard.Default, Langs.Const_Placeholder_ReType_Password, ReturnKeyTypes.Done);
            enterNewPassword2.SetBinding(Entry.TextProperty, new Binding("NewPasswordTwo"));
            enterNewPassword2.SetBinding(Entry.IsEnabledProperty, new Binding("PasswordEnabled"));
            enterNewPassword2.SetBinding(Entry.IsPasswordProperty, new Binding("HidePassword", converter: new ReverseBoolConverter()));

            var arrowButton = ArrowBtn.ArrowButton(Langs.Const_Button_Confirm_1, App.ScreenSize.Width * .8, new Action(() => ViewModel.BtnChangePassword.Execute(null)));
            arrowButton.SetBinding(Button.IsEnabledProperty, new Binding("EnableButton"));

            var swtchPassword = new Switch();
            swtchPassword.SetBinding(Switch.IsToggledProperty, new Binding("HidePassword"));
            swtchPassword.Toggled += (s, e) => ViewModel.HidePassword = e.Value;

            var imgRegister = new Image
            {
                Source = "cancel".CorrectedImageSource(),
                HeightRequest = 52
            };
            imgRegister.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(async () => await Navigation.PopAsync())
            });

            var width = App.ScreenSize.Width * .9;
            var oldPWGrid = new Grid
            {
                InputTransparent = true,
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = App.ScreenSize.Width * .9}
                },
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = 40},
                }
            };

            var newPWGrid = new Grid
            {
                RowSpacing = 12,
                InputTransparent = true,
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = App.ScreenSize.Width * .9}
                },
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = 40},
                    new RowDefinition {Height = 40}
                }
            };

            oldPWGrid.Children.Add(new EntryCell(Langs.Const_Label_Current_Password, enterCurrentPassword, width), 0, 0);
            newPWGrid.Children.Add(new EntryCell(Langs.Const_Label_New_Password, enterNewPassword, App.ScreenSize.Width * .9), 0, 0);
            newPWGrid.Children.Add(new EntryCell(Langs.Const_Label_ReEnter, enterNewPassword2, App.ScreenSize.Width * .9), 0, 1);

            var midStack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(0,8),
                Children =
                {
                    newPWGrid,
                    arrowButton,
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.Center,
                        Children =
                        {
                            swtchPassword,
                            new Label {Text = Langs.Const_Label_Hide_Password, FontFamily = Helper.RegFont, FontSize = 14, TextColor = Color.White}
                        }
                    }
                }
            };

            grid.Children.Add(oldPWGrid, 0, 0);
            grid.Children.Add(new BoxView { HeightRequest = 1, WidthRequest = App.ScreenSize.Width, BackgroundColor = Color.White }, 0, 1);
            grid.Children.Add(midStack, 0, 2);
            grid.Children.Add(new BoxView { HeightRequest = 1, WidthRequest = App.ScreenSize.Width, BackgroundColor = Color.White }, 0, 3);
            grid.Children.Add(new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = App.ScreenSize.Height * .2,
                Padding = new Thickness(0, 4),
                Children =
                {
                    imgRegister,
                    new Label
                    {
                        Text = Langs.Const_Label_Cancel,
                        TextColor = Color.White,
                        FontFamily= Helper.RegFont,
                        HorizontalTextAlignment = TextAlignment.Center
                    }
                }
            }, 0, 4);

            stack.Children.Add(grid);
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

