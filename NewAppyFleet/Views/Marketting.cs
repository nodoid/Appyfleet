using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mvvmframework.Languages;
using mvvmframework.ViewModels.Settings;
using Xamarin.Forms;

namespace NewAppyFleet.Views
{
    public class Marketting : ContentPage
    {
        MarkettingPrefsViewModel ViewModel => App.Locator.Marketting;
        public StackLayout stack;
        StackLayout innerStack;

        public Marketting()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundColor = FormsConstants.AppyDarkShade;
            BindingContext = ViewModel;
            ViewModel.ChangeMarketting = ViewModel.ShowCurrentMarketting;
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
                Text = Langs.Const_Screen_Title_Marketing_Preferences
            };

            var spinner = new ActivityIndicator
            {
                HeightRequest = 40,
                WidthRequest = 40,
                IsRunning = true
            };
            spinner.SetBinding(ActivityIndicator.IsVisibleProperty, new Binding("IsBusy"));

            var swchAgree = new Switch();
            swchAgree.SetBinding(Switch.IsToggledProperty, new Binding("ChangeMarketting"));
            swchAgree.Toggled += (sender, e) => ViewModel.ChangeMarketting = e.Value;

            var dataStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new BoxView {HeightRequest = 52, BackgroundColor = Color.Transparent},
                    new StackLayout
                    {
                        Padding = new Thickness(16,0),
                        Children =
                        {
                            new Label
                            {
                                FontFamily = Helper.RegFont,
                                TextColor = Color.White,
                                Text = ViewModel.ShowBoilerPlate
                            }
                        }
                    },
                    new BoxView {HeightRequest = 16, BackgroundColor = Color.Transparent},
                    new BoxView {HeightRequest = 1, WidthRequest = App.ScreenSize.Width, BackgroundColor = Color.White},
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.Center,
                        Children =
                        {
                            swchAgree,
                            new Label
                            {
                                FontFamily = Helper.RegFont,
                                TextColor = Color.White,
                                Text = Langs.Const_Label_Agree_1
                            }
                        }
                    },
                    spinner
                }
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