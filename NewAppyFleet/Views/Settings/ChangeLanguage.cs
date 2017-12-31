using System;
using System.Globalization;
using mvvmframework;
using mvvmframework.Languages;
using mvvmframework.ViewModels.Settings;
using NewAppyFleet.Views.ListViewCells;
using Xamarin.Forms;

namespace NewAppyFleet.Views.Settings
{
    public class ChangeLanguage : ContentPage
    {
        public StackLayout stack;
        StackLayout innerStack;
        ListView langListView;

        ChangeLanguageViewModel ViewModel => App.Locator.Language;

        void RegisterEvents()
        {
            ViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "Languages")
                {
                    if (langListView != null)
                    {
                        Device.BeginInvokeOnMainThread(() => { langListView.ItemsSource = null; langListView.ItemsSource = ViewModel.Languages; });
                    }
                }
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RegisterEvents();
        }

        public ChangeLanguage()
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
                Text = Langs.Const_Screen_Title_Language
            };

            langListView = new ListView
            {
                HasUnevenRows = true,
                ItemsSource = ViewModel.Languages,
                ItemTemplate = new DataTemplate(typeof(LanguageViewCell))
            };

            langListView.ItemSelected += (s, e) =>
             {
                 var item = e.SelectedItem as LanguageModel;
                 if (e.SelectedItem == null) return;
                 ViewModel.NewLanguage = ViewModel.Languages[(int)e.SelectedItem].Name;
                Culture.currentCulture = new CultureInfo(ViewModel.Languages[(int)e.SelectedItem].Code);
                 Langs.Culture = new CultureInfo(ViewModel.Languages[(int)e.SelectedItem].Code);
                 DependencyService.Get<ILocalize>().SetLocale(Langs.Culture);
             };

            var spinner = new ActivityIndicator
            {
                HeightRequest = 40,
                WidthRequest = 40,
                IsRunning = true
            };
            spinner.SetBinding(ActivityIndicator.IsVisibleProperty, new Binding("IsBusy"));

            var stk = new StackLayout
            {
                WidthRequest = App.ScreenSize.Width,
                Children = { langListView, spinner }
            };

            stack.Children.Add(stk);
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
