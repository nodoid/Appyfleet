using Xamarin.Forms;
using mvvmframework.Languages;
using mvvmframework;
using NewAppyFleet.Views.ViewCells;
using System.ComponentModel;
using mvvmframework.ViewModels;

namespace NewAppyFleet
{
    public class ForgottenPassword : ContentPage
    {
        ForgottenPasswordViewModel ViewModel => App.Locator.Forgotten;

        void RegisterEvents()
        {
            ViewModel.PropertyChanged += async (object sender, PropertyChangedEventArgs e) =>
            {
                if (e.PropertyName == "Done")
                {
                    await Navigation.PopAsync();
                }
            };
        }


        public ForgottenPassword()
        {
            RegisterEvents();
            CreateUI();
            BackgroundColor = FormsConstants.AppyLightBlue;
            BindingContext = ViewModel;
            NavigationPage.SetHasNavigationBar(this, false);
        }

        void CreateUI()
        {
            var masterGrid = new Grid
            {
                WidthRequest = App.ScreenSize.Width * .8,
                VerticalOptions = LayoutOptions.Center,
            };
            masterGrid.RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition {Height = App.ScreenSize.Height * .3},
                new RowDefinition {Height = App.ScreenSize.Height * .5},
                new RowDefinition {Height = App.ScreenSize.Height * .2},
            };

            masterGrid.Children.Add(new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new Image
                    {
                        Source = "app_icon1".CorrectedImageSource(),
                        HeightRequest = 72,
                        VerticalOptions = LayoutOptions.Center
                    }
                }
            }, 0, 0);

            var usernameEntry = UniversalEntry.GeneralEntryCell("", App.ScreenSize.Width * .8, Keyboard.Email, Langs.Const_Label_Email_2, ReturnKeyTypes.Next,.9);
            usernameEntry.SetBinding(Entry.TextProperty, new Binding("Username"));

            var passwordEntry = UniversalEntry.GeneralEntryCell("", App.ScreenSize.Width * .8, Keyboard.Numeric, Langs.Const_Placeholder_DOB,ReturnKeyTypes.Done,.9);
            passwordEntry.SetBinding(Entry.TextProperty, new Binding("YOB"));


            var usernameGrid = new ImageEntryCell("icon_user", usernameEntry, App.ScreenSize.Width * .9);
            var passwordGrid = new ImageEntryCell("icon_password", passwordEntry, App.ScreenSize.Width * .9);

            var btnSubmit = new Button
            {
                BorderRadius = 6,
                BackgroundColor = FormsConstants.AppyBlue,
                TextColor = Color.White,
                WidthRequest = App.ScreenSize.Width * .9,
                Text = Langs.Const_Label_Forgot_Password
            };
            btnSubmit.SetBinding(Button.IsEnabledProperty, new Binding("CanSubmit"));
            btnSubmit.Clicked += delegate 
            {
                ViewModel.ResetPasswordCommand.Execute(null);
            };

            var detailsStack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(0, 12),
                WidthRequest = App.ScreenSize.Width * .9,
                TranslationX = -6,
                Children =
                {
                    new StackLayout
                    {
                        Padding = new Thickness(0,12),
                        WidthRequest = App.ScreenSize.Width *.8,
                        HorizontalOptions = LayoutOptions.Center,
                        Children = 
                        {
                            new Label
                            {
                                Text = Langs.Const_Msg_Forgot_Password_Desc,
                                TextColor = Color.White,
                                FontFamily = Helper.RegFont,
                                FontSize = 14,
                                HorizontalTextAlignment = TextAlignment.Center
                            }
                        }
                    },
                    usernameGrid,
                    passwordGrid,
                    new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        Orientation = StackOrientation.Horizontal,
                        TranslationX = 0,
                        Children =
                        {
                            btnSubmit
                        }
                    }
                }
            };

            masterGrid.Children.Add(detailsStack, 0, 1);

            Content = new StackLayout
            {
                WidthRequest = App.ScreenSize.Width,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                Children = { masterGrid }
            };
        }
    }
}

