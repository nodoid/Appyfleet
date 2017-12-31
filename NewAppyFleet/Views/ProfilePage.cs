using System;
using mvvmframework.Languages;
using mvvmframework.ViewModels;
using Xamarin.Forms;

namespace NewAppyFleet.Views
{
    public class ProfilePage : ContentPage
    {
        MyProfileViewModel ViewModel => App.Locator.MyProfile;
        public StackLayout stack;
        StackLayout innerStack;

        void RegisterEvents()
        {
            ViewModel.PropertyChanged += async (sender, e) => 
            {
                if (e.PropertyName == "HasLoggedOut")
                {
                    if (ViewModel.HasLoggedOut)
                    {
                        await Navigation.PushAsync(new LoginPage());
                        Navigation.RemovePage(this);
                    }
                }
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RegisterEvents();
        }

        public ProfilePage()
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
                BackgroundColor = FormsConstants.AppyBlue,
                TextColor = Color.White,
                Text = Langs.Const_Screen_Title_My_Profile
            };

            var grid = new Grid
            {
                WidthRequest = App.ScreenSize.Width,
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = App.ScreenSize.Height * .1},
                    new RowDefinition {Height = 1},
                    new RowDefinition {Height = GridLength.Star}
                }
            };

            var width = App.ScreenSize.Width * .9;
            var innerGrid = new Grid
            {
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = width,
                VerticalOptions = LayoutOptions.Center,
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = width * .6},
                    new ColumnDefinition {Width = GridLength.Star}
                }
            };

            var lblName = new Label
            {
                FontFamily = Helper.RegFont,
                FontSize = 24,
                TextColor = Color.White,
                Text = ViewModel.FullName
            };
            var btnLogout = new Button
            {
                BorderRadius = 6,
                BackgroundColor = Color.White,
                TextColor = Color.Black,
                Text = Langs.Const_Button_SignOut
            };
            btnLogout.Clicked += delegate {ViewModel.SignOut();};
            innerGrid.Children.Add(lblName, 0, 0);
            innerGrid.Children.Add(btnLogout, 1, 0);
            grid.Children.Add(innerGrid, 0, 0);
            grid.Children.Add(new BoxView { WidthRequest = App.ScreenSize.Width, HeightRequest = 1, BackgroundColor = Color.White },0,1);

            var imgPrivate = new Image
            {
                Margin = new Thickness(12,0,0,0),
                HeightRequest = 20,
                WidthRequest = 20,
                Source = ViewModel.PrivateMod ? "radio_on_light".CorrectedImageSource() : "radio_off_light".CorrectedImageSource()
            };
            var lblFleetName = new Label
            {
                FontSize = 18,
                FontFamily = Helper.RegFont,
                TextColor = Color.White,
                Text = ViewModel.FleetName
            };

            var privateStack = new StackLayout
            {
                BackgroundColor = FormsConstants.AppyDarkBlue,
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = 36,
                Padding = new Thickness(0,8,0,0),
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                Children =
                {
                    imgPrivate,
                    new Label {Text = Langs.Const_Label_Private_Mode,VerticalTextAlignment=TextAlignment.Center, FontSize = 18,FontFamily = Helper.RegFont, TextColor = Color.White}
                }
            };

            width = App.ScreenSize.Width * .9;
            var btnGrid = new Grid
            {
                WidthRequest = width,
                ColumnSpacing = 8,
                HeightRequest = 42,
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = width * .5},
                    new ColumnDefinition {Width = width * .5}
                }
            };
            var btnNextJourney = new Button
            {
                WidthRequest = width * .5,
                BackgroundColor = FormsConstants.AppyBlue,
                TextColor = Color.White,
                BorderRadius = 6,
                Text = Langs.Const_Button_Next_Journey_Only
            };
            var btnSetTime = new Button
            {
                WidthRequest = width * .5,
                BackgroundColor = FormsConstants.AppyBlue,
                TextColor = Color.White,
                BorderRadius = 6,
                Text = Langs.Const_Button_Set_End_Time
            };
            btnGrid.Children.Add(btnNextJourney, 0, 0);
            btnGrid.Children.Add(btnSetTime, 1, 0);

            width = App.ScreenSize.Width * .9;
            var notificationGrid = new Grid
            {
                WidthRequest = width,
                HeightRequest =36,
                VerticalOptions = LayoutOptions.Center,
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = width * .9},
                    new ColumnDefinition {Width = GridLength.Star}
                }
            };
            notificationGrid.Children.Add(new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                Children =
                {
                    new Image {HeightRequest = 36, WidthRequest = 36, Source = "exclamation_mark".CorrectedImageSource()},
                    new Label {Text = $"{ViewModel.NotificationCount} {Langs.Const_Label_Notifications}", FontSize = 18,FontFamily = Helper.RegFont, TextColor = Color.White}
                }
            },0,0);
            notificationGrid.Children.Add(new Image {Source="link_arrow".CorrectedImageSource(), WidthRequest = 24,HeightRequest=24},1,0);
                            
            var notificationStack = new StackLayout
            {
                BackgroundColor = FormsConstants.AppyDarkBlue,
                HeightRequest = 36,
                Padding = new Thickness(12),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children = {notificationGrid}
            };
            notificationStack.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(async () => await Navigation.PushAsync(new NotificationsPage()))
            });

            var manageGrid = new Grid
            {
                WidthRequest = width,
                HeightRequest = 36,
                VerticalOptions = LayoutOptions.Center,
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = width * .9},
                    new ColumnDefinition {Width = GridLength.Star}
                }
            };
            manageGrid.Children.Add(new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                Children =
                {
                    new Image {HeightRequest = 36, WidthRequest = 36, Source = "main_menu_tips".CorrectedImageSource()},
                    new Label {Text = Langs.Const_Label_Manage_Vehicles, FontSize = 18,FontFamily = Helper.RegFont, TextColor = Color.White}
                }
            }, 0, 0);
            manageGrid.Children.Add(new Image { Source = "link_arrow".CorrectedImageSource(), WidthRequest = 24, HeightRequest = 24 }, 1, 0);

            var manageStack = new StackLayout
            {
                BackgroundColor = FormsConstants.AppyDarkBlue,
                HeightRequest = 36,
                Padding = new Thickness(12),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children = { manageGrid }
            };
            manageStack.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(async () => await Navigation.PushAsync(new PairNewVehiclePage(false)))
            });

            var dataStack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new StackLayout
                    {
                        Padding = new Thickness(8,8,0,0),
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            new Label {Text = Langs.Const_Screen_Title_Fleet_Code,TextColor = Color.White, FontSize = 18, FontFamily = Helper.RegFont},
                            lblFleetName
                        }
                    },
                    new StackLayout
                    {
                        Padding = new Thickness(0,8,0,0),
                        Children = {privateStack}
                    },
                    new StackLayout
                    {
                        Padding = new Thickness(0,12,0,0),
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        Children =
                        {
                            new StackLayout
                            {
                                WidthRequest = App.ScreenSize.Width * .9,
                                Children =
                                {
                                    new Label
                                    {
                                        TextColor = Color.White,
                                        FontSize = 18,
                                        FontFamily = Helper.RegFont,
                                        HorizontalTextAlignment = TextAlignment.Center,
                                        LineBreakMode=  LineBreakMode.WordWrap,
                                        Text = Langs.Const_Msg_Private_Mode_Desc
                                    },
                                    btnGrid
                                }
                            }
                        }
                    },
                    new StackLayout
                    {
                        Padding = new Thickness(0,16,0,0),
                        VerticalOptions = LayoutOptions.Start,
                        Children =
                        {
                            notificationStack,
                            manageStack
                        }
                    }
                }
            };
            grid.Children.Add(dataStack, 0, 2);
            stack.Children.Add(grid);
            innerStack.Children.Add(grid);

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

