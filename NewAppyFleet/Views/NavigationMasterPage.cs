using mvvmframework.Languages;
using NewAppyFleet.Views.ListViewCells;
using System.Collections.Generic;

using Xamarin.Forms;

namespace NewAppyFleet.Views
{
    public class MasterPageList
    {
        public string Image { get; set; }

        public string Text { get; set; }
       
        public override string ToString()
        {
            return Text;
        }
    }

    public class NavigationMasterPage : MasterDetailPage
    {
        public NavigationMasterPage()
        {
            var header = new Label
            {
                Text = "MasterPageView",
                FontSize = 28,
                FontAttributes = FontAttributes.Bold | FontAttributes.Italic,
                HorizontalOptions = LayoutOptions.Center
            };

            var listOptions = new List<MasterPageList>
            {
                new MasterPageList { Text = Langs.Const_Menu_Dashboard, Image = "main_menu_dashboard" },
                new MasterPageList { Text = Langs.Const_Menu_ScoreHistory, Image = "main_menu_score" },
                new MasterPageList { Text = Langs.Const_Menu_YourJourneys, Image = "main_menu_journey" },
                new MasterPageList { Text = Langs.Const_Menu_Expenses, Image = "main_menu_expenses" },
                new MasterPageList { Text = Langs.Const_Menu_UserGuide, Image = "main_menu_user" },
                new MasterPageList { Text = Langs.Const_Menu_Terms, Image="main_menu_terms" },
                new MasterPageList { Text = Langs.Const_Menu_DrivingTips, Image="main_menu_tips" },
                new MasterPageList { Text = Langs.Const_Menu_Settings, Image="main_menu_settings"},
                new MasterPageList { Text = Langs.Const_Menu_EmergencyAdvice, Image="main_menu_emergency" },
            };

            var listView = new ListView
            {
                ItemTemplate = new DataTemplate(typeof(MDPListCell)),
                ItemsSource = listOptions
            };

            // create the master page
            Master = new ContentPage
            {
                Title = "",
                Content = new StackLayout
                {
                    Children =
                    {
                        header, listView
                    }
                }
            };

            Detail = new NavigationPage(new DashboardPage());

            listView.ItemSelected += LaunchPage;

            // the final step is to enable some method to get out of the detail page. iOS handles this without an issue
            // but we need something in for Android and Windows Mobile

            if (Device.RuntimePlatform != Device.iOS)
            {
                var tap = new TapGestureRecognizer();
                tap.Tapped += (sender, args) =>
                {
                    IsPresented = true;
                };
            }
        }

        void LaunchPage(object s, SelectedItemChangedEventArgs e)
        {
            var binding = e.SelectedItem as MasterPageList;
            Page pg = null;

            switch(binding.Text)
            {

            }

            Detail = string.IsNullOrEmpty(binding.Url) ? new NavigationPage(new WebviewGenerated()) :
                new NavigationPage(new Webview(binding.Url));

            // show the distribution page

            IsPresented = false;
        }
    }
}