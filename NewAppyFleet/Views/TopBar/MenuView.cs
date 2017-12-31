using mvvmframework.Languages;
using NewAppyFleet.Views;
using NewAppyFleet.Views.ListViewCells;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using mvvmframework;
using GalaSoft.MvvmLight.Ioc;
using mvvmframework.Interfaces;

namespace NewAppyFleet
{
    public class MenuListClass
    {
        public string image { get; set; }

        public string text { get; set; }

        public bool enabled { get; set; } = true;
    }

    public class MenuView : ContentView
    {
        List<MenuListClass> menuList;

        public MenuView()
        {
            menuList = new List<MenuListClass>
            {
                new MenuListClass { text = Langs.Const_Menu_Dashboard, image = "main_menu_dashboard" },
                new MenuListClass { text = Langs.Const_Menu_ScoreHistory, image = "main_menu_score" },
                new MenuListClass { text = Langs.Const_Menu_YourJourneys, image = "main_menu_journey" },
                new MenuListClass { text = Langs.Const_Menu_Expenses, image = "main_menu_expenses" },
                new MenuListClass { text = Langs.Const_Menu_UserGuide, image = "main_menu_user" },
                new MenuListClass { text = Langs.Const_Menu_Terms, image="main_menu_terms" },
                new MenuListClass { text = Langs.Const_Menu_DrivingTips, image="main_menu_tips" },
                new MenuListClass { text = Langs.Const_Menu_Settings, image="main_menu_settings"},
                new MenuListClass { text = Langs.Const_Menu_EmergencyAdvice, image="main_menu_emergency" },
            };

            var lstMenu = new ListView
            {
                BackgroundColor = FormsConstants.AppyDarkBlue,
                MinimumWidthRequest = App.ScreenSize.Width,
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = App.ScreenSize.Height,
                ClassId = "menu",
                ItemsSource = menuList,
                ItemTemplate = new DataTemplate(typeof(MDPListCell))
            };

            lstMenu.ItemSelected += async (s, e) =>
            {
                var item = e.SelectedItem as MenuListClass;

                if (e.SelectedItem == null) return;

                var final = item.image.Split('_').Last();
                App.Self.PanelShowing = false;
                switch (final)
                {
                    case "dashboard":
                        await Navigation.PushAsync(new DashboardPage(), true);
                        break;
                    case "score":
                        await Navigation.PushAsync(new ScoreHistory(), true);
                        break;
                    case "journey":
                        await Navigation.PushAsync(new JourneyPage(), true);
                        break;
                    case "expenses":
                        await Navigation.PushAsync(new ExpensesPage(), true);
                        break;
                    case "user":
                        //await Navigation.PushAsync(new UserGuidePage(), true);
                        MessagingCenter.Send("url", "load", "userguide");
                        break;
                    case "terms":
                        MessagingCenter.Send("url", "load", "terms");
                        //await Navigation.PushAsync(new TandCPage(), true);
                        break;
                    case "tips":
                        MessagingCenter.Send("url", "load", "tips");
                        //await Navigation.PushAsync(new DrivingTipsPage(), true);
                        break;
                    case "settings":
                        await Navigation.PushAsync(new SettingsPage(), true);
                        break;
                    case "emergency":
                        await Navigation.PushAsync(new EmergencyAdvice(), true);
                        break;
                }

                ((ListView)s).SelectedItem = null;
            };

            Content = new StackLayout
            {
                Children = {lstMenu}
            };
        }
    }
}