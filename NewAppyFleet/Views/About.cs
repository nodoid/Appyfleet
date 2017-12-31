using mvvmframework.Languages;
using mvvmframework.ViewModels.Settings;

using Xamarin.Forms;

namespace NewAppyFleet.Views
{
    public class About : ContentPage
    {
        public StackLayout stack;
        StackLayout innerStack;

        AboutViewModel ViewModel => App.Locator.About;

        public About()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundColor = FormsConstants.AppyDarkBlue;
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
                Text = Langs.Const_Screen_Title_About
            };

            var centerStack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(0, 8),
                WidthRequest = App.ScreenSize.Width * .8,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label {TextColor = Color.White, FontFamily = Helper.RegFont, FontSize = 18, Text = $"{Langs.Const_Label_Version} {ViewModel.VersionNumber}", HorizontalTextAlignment = TextAlignment.Center},
                    new Label {TextColor = Color.White, FontFamily = Helper.RegFont, FontSize = 18, Text = $"{Langs.Const_Label_Installed} {ViewModel.VersionDate}", HorizontalTextAlignment = TextAlignment.Center},
                    new Label {TextColor = Color.White, FontFamily = Helper.RegFont, FontSize = 18, Text = Langs.Const_Label_About_Description, HorizontalTextAlignment = TextAlignment.Center},
                    new Label {TextColor = Color.White, FontFamily = Helper.RegFont, FontSize = 18, Text = Langs.Const_Label_About_Company_Number, HorizontalTextAlignment = TextAlignment.Center},
                }
            };

            stack.Children.Add(centerStack);
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