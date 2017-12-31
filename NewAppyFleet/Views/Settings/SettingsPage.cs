using mvvmframework.Languages;
using NewAppyFleet.Views;
using NewAppyFleet.Views.ListViewCells;
using NewAppyFleet.Views.Settings;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace NewAppyFleet
{
    public class SettingsPage : ContentPage
    {
        public StackLayout stack;
        StackLayout innerStack;
        List<MenuListClass> menuList;

        public SettingsPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundColor = FormsConstants.AppyDarkShade;
            CreateUI();
        }

        void CreateUI()
        {
            menuList = new List<MenuListClass>
            {
                new MenuListClass {text = Langs.Const_Label_Change_Password, image = "Settings_Password"},
                new MenuListClass {text = Langs.Const_Label_Phone_Number, image = "Settings_PhoneNumber"},
                new MenuListClass {text = Langs.Const_Label_Fleet_Code, image = "Settings_FleetCode"},
                new MenuListClass {text = Langs.Const_Label_Odometer, image = "Settings_Odometer"},
                new MenuListClass {text = Langs.Const_Label_Marketing_Preferences, image = "Settings_Marketing"},
                new MenuListClass {text = Langs.Const_Label_Language_Preferences, image = "Settings_Languages"},
                new MenuListClass {text = Langs.Const_Label_Log_Files, image = "Settings_LogFiles"},
                new MenuListClass {text = Langs.Const_Label_About, image = "Settings_About"},
            };

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
                Text = Langs.Const_Menu_Settings
            };

            var lstMenu = new ListView
            {
                BackgroundColor = FormsConstants.AppyDarkBlue,
                MinimumWidthRequest = App.ScreenSize.Width,
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = App.ScreenSize.Height,
                ClassId = "menu",
                ItemsSource = menuList,
                ItemTemplate = new DataTemplate(typeof(SettingsListCell))
            };

            lstMenu.ItemSelected += async (s, e) =>
            {
                var item = e.SelectedItem as MenuListClass;
                if(e.SelectedItem == null) return;
                var final = item.image.Split('_').Last();
                switch (final)
                {
                    case "Password":
                        await Navigation.PushAsync(new ChangePassword(), true);
                        break;
                    case "PhoneNumber":
                        await Navigation.PushAsync(new ChangePhoneNumber(), true);
                        break;
                    case "FleetCode":
                        await Navigation.PushAsync(new ChangeFleetCode(), true);
                        break;
                    case "Odometer":
                        await Navigation.PushAsync(new ChangeOdometer(), true);
                        break;
                    case "Marketing":
                        await Navigation.PushAsync(new Marketting(), true);
                        break;
                    case "LogFiles":
                        await Navigation.PushAsync(new LogFiles(), true);
                        break;
                    case "About":
                        await Navigation.PushAsync(new About(), true);
                        break;
                    case "Language":
                        await Navigation.PushAsync(new ChangeLanguage(), true);
                        break;
                }
                ((ListView)s).SelectedItem = null;
            };

            stack.Children.Add(lstMenu);
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

