using Xamarin.Forms;
using System.Collections.Generic;
using mvvmframework.Languages;

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

            var masterStack = new StackLayout
            {
                BackgroundColor = FormsConstants.AppyDarkBlue,
                Orientation = StackOrientation.Vertical,
                MinimumWidthRequest = App.ScreenSize.Width,
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = App.ScreenSize.Height /*- 52 - 36*/,
                //TranslationY = 8,
                StyleId = "menu"
            };
            for (var i = 0; i < menuList.Count; ++i)
                masterStack.Children.Add(MenuListView(i));

            var num = 0;


            if (num == 0)
                menuList[1].enabled = false;

            Content = masterStack;
        }

        StackLayout MenuListView(int i)
        {
            var imgIcon = new Image
            {
                WidthRequest = 36,
                HeightRequest = 36,
                Source = menuList[i].image.CorrectedImageSource()
            };

            var lblText = new Label
            {
                FontSize = 18,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White,
                Text = menuList[i].text,
                FontFamily = Helper.RegFont
            };

            var tap = new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(async (t) =>
                    {
                        App.Self.PanelShowing = false;
                        switch (i)
                        {
                            case 0:
                                await Navigation.PushAsync(new DashboardSettingsPage(), true);
                                break;
                            case 1:
                                await Navigation.PushAsync(new ScoreHistoryPage(), true);
                                break;
                            case 2:
                                await Navigation.PushAsync(new YourJourneyPage(), true);
                                break;
                            case 3:
                                await Navigation.PushAsync(new ExpensesPage(), true);
                                break;
                            case 4:
                                await Navigation.PushAsync(new UserGuidePage(), true);
                                break;
                            case 5:
                                await Navigation.PushAsync(new TandCPage(), true);
                                break;
                            case 6:
                                await Navigation.PushAsync(new DrivingTipsPage(), true);
                                break;
                            case 7:
                                await Navigation.PushAsync(new SettingsPage(), true);
                                break;
                            case 8:
                                await Navigation.PushAsync(new EmergencyAdvicePage(), true);
                                break;

                        }
                    }
                )
            };

            var stack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(0, 4, 0, 0),
                HeightRequest = 40,
                WidthRequest = App.ScreenSize.Width,
                Children =
                {
                    new StackLayout
                    {
                        Padding = new Thickness(8),
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        { imgIcon,
                            new StackLayout
                            {
                                Padding = new Thickness(8, 0, 0, 0),
                                VerticalOptions = LayoutOptions.Center,
                                Children = { lblText }
                            }
                        }
                    }
                },
            };
            stack.GestureRecognizers.Add(tap);

            return stack;
        }
    }
}


