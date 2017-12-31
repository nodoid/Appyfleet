using Xamarin.Forms;
using mvvmframework.Languages;
using mvvmframework;
using NewAppyFleet.Views.ViewCells;
using System.ComponentModel;
using NewAppyFleet.Views;
using NewAppyFleet.Converters;

namespace NewAppyFleet
{
    public class LoginPage : ContentPage
    {
        LoginViewModel ViewModel => App.Locator.Login;

        protected override bool OnBackButtonPressed()
        {
            return false;
        }

        void RegisterEvents()
        {
            ViewModel.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
                if (e.PropertyName == "LoggedIn")
                {
                    if (ViewModel.LoggedIn)
                    { 
                        Device.BeginInvokeOnMainThread(() => Application.Current.MainPage = new NavigationPage(new DashboardPage(false)));
                    }
                }
            };
        }

        public LoginPage()
        {
            RegisterEvents();
            BackgroundColor = FormsConstants.AppyLightBlue;
            BindingContext = ViewModel;
            CreateUI(); 
            ViewModel.CheckForDetails();
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

            var imgRegister = new Image
            {
                Source = "register".CorrectedImageSource(),
                HeightRequest = 52
            };
            imgRegister.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(async()=>await Navigation.PushAsync(new InitialApprovalPage()))
            });

            masterGrid.Children.Add(new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = App.ScreenSize.Height * .2,
                Padding = new Thickness(0,4),
                Children =
                {
                    imgRegister,
                    new Label
                    {
                        Text = Langs.Const_Label_Register,
                        TextColor = Color.White,
                        FontFamily= Helper.RegFont,
                        HorizontalTextAlignment = TextAlignment.Center
                    }
                }
            }, 0, 2);

            var usernameEntry = UniversalEntry.GeneralEntryCell("", App.ScreenSize.Width * .8, Keyboard.Email, Langs.Const_Placeholder_UserName, ReturnKeyTypes.Next,.9);
            usernameEntry.SetBinding(Entry.TextProperty, new Binding("Username"));

            var passwordEntry = UniversalEntry.GeneralEntryCell("", App.ScreenSize.Width * .8, Keyboard.Default, Langs.Const_Placeholder_Password, ReturnKeyTypes.Done, .9);
            passwordEntry.SetBinding(Entry.TextProperty, new Binding("Password"));
            passwordEntry.SetBinding(Entry.IsPasswordProperty, new Binding("ShowPassword", converter: new ReverseBoolConverter()));

            var usernameGrid = new ImageEntryCell("icon_user", usernameEntry, App.ScreenSize.Width * .9);
            var passwordGrid = new ImageEntryCell("icon_password", passwordEntry, App.ScreenSize.Width * .9);

            var btnSignIn = new Button
            {
                BorderRadius = 8,
                BackgroundColor = FormsConstants.AppyDarkShade,
                TextColor = Color.White,
                Text = Langs.Const_Button_SignIn,
                FontFamily = Helper.RegFont,
                WidthRequest = App.ScreenSize.Width * .8,
                HeightRequest = 40,
            };
            btnSignIn.Clicked += delegate { ViewModel.CmdLoginUser.Execute(null); };
            btnSignIn.SetBinding(Button.IsEnabledProperty, new Binding("CanLogin"));    

            var swchShowPassword = new Switch();
            swchShowPassword.Toggled += (sender, e) =>
            {
                //passwordEntry.IsPassword = !e.Value;
                ViewModel.ShowPassword = e.Value;
            };
            swchShowPassword.SetBinding(Switch.IsToggledProperty, new Binding("ShowPassword"));

            var lblForgottenPassword = new Label
            {
                Text = Langs.Const_Label_Forgot_Password,
                TextColor = Color.Wheat,
                FontFamily = Helper.RegFont,
                HorizontalTextAlignment = TextAlignment.End
            };
            var lblForgottenPasswordTap = new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(async () => await Navigation.PushAsync(new ForgottenPassword()))
            };
            lblForgottenPassword.GestureRecognizers.Add(lblForgottenPasswordTap);

            var spinner = new ActivityIndicator
            {
                HeightRequest = 40,
                WidthRequest = 40,
                IsRunning = true
            };
            spinner.SetBinding(ActivityIndicator.IsVisibleProperty, new Binding("IsBusy"));

            var detailsStack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(0, 12),
                WidthRequest = App.ScreenSize.Width * .9,
                TranslationX = -6,
                Children =
                {
                    usernameGrid,
                    passwordGrid,
                    new StackLayout
                    {
                        WidthRequest = App.ScreenSize.Width * .9,
                        HorizontalOptions = LayoutOptions.Center,
                        TranslationX = 6,
                        Children = {btnSignIn}
                    },
                    spinner,
                    new StackLayout
                    {
                        WidthRequest = App.ScreenSize.Width * .9,
                        HorizontalOptions = LayoutOptions.Center,
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                WidthRequest = App.ScreenSize.Width * .45,
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.Center,
                                Children =
                                {
                                    swchShowPassword,
                                    new Label
                                    {
                                        Text = Langs.Const_Label_Show_Password,
                                        FontFamily = Helper.BoldFont,
                                        TextColor = Color.White
                                    }
                                }
                            },
                            new StackLayout
                            {
                                WidthRequest = App.ScreenSize.Width * .45,
                                HorizontalOptions = LayoutOptions.EndAndExpand,
                                Children = {lblForgottenPassword}
                            }
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

