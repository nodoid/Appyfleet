using mvvmframework.Languages;
using mvvmframework.ViewModels.Settings;
using NewAppyFleet.Views.ListViewCells;
using System;
using System.Linq;
using Xamarin.Forms;

namespace NewAppyFleet.Views
{
    public class LogFiles : ContentPage
    {
        public StackLayout stack;
        StackLayout innerStack;
        LogFilesViewModel ViewModel => App.Locator.LogFiles;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<LogFileViewCell, string>(this, "logfile", (s, e) =>
            {
                ViewModel.SendLog = ViewModel.GetLogs.FirstOrDefault(t => t.DateIndex == Convert.ToDateTime(e)).DateIndex;
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<LogFileViewCell, string>(this, "logfile");
        }

        public LogFiles()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = ViewModel;
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
                Text = Langs.Const_Label_Log_Files
            };

            var lstLogs = new ListView
            {
                WidthRequest = App.ScreenSize.Width * .9,
                HorizontalOptions = LayoutOptions.Center,
                ItemsSource = ViewModel.GetLogs,
                SeparatorVisibility = SeparatorVisibility.None,
                ItemTemplate = new DataTemplate(typeof(LogFileViewCell)),
                HasUnevenRows = true
            };

            var lstStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Padding = new Thickness(0, 12),
                Children = { lstLogs }
            };

            stack.Children.Add(lstStack);
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