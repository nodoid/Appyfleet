using mvvmframework.Languages;
using mvvmframework.ViewModels;
using Xamarin.Forms;
using NewAppyFleet.Converters;
using NewAppyFleet.Views.ListViewCells;
using NewAppyFleet.CustomViews;
using mvvmframework.Models;
using System.Linq;

namespace NewAppyFleet.Views
{
    public class NotificationsPage : ContentPage
    {
        NotificationsViewModel ViewModel => App.Locator.Notifications;
        public StackLayout stack;
        StackLayout innerStack;
        ListView listNotes;
        CustomListView notificationView = new CustomListView();

        void RegisterEvents()
        {
            ViewModel.PropertyChanged += (sender, e) => 
            {
                if (e.PropertyName == "Notes")
                {
                    Device.BeginInvokeOnMainThread(()=>
                    {
                        if (listNotes != null)
                        {
                            listNotes.ItemsSource = null;
                            listNotes.ItemsSource = ViewModel.SmallNoteList;
                        }
                    });
                }
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<string,int>("modal", "map", async (a,b)=>
            {
                await Navigation.PushAsync(new NotificationMapPage(ViewModel.SelectedJourneyId, b));
            });
            RegisterEvents();
        }

        public NotificationsPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundColor = FormsConstants.AppyDarkShade;
            CreateUI();
        }

        public NotificationsPage(long journeyId)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundColor = FormsConstants.AppyDarkShade;
            CreateUI();
        }

        void CreateUI()
        {
            ViewModel.SetFilter("All");

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

            var topbar = new TopBar(true, "", this, 1, "back_arrow", "refresh_icon", innerStack, true).CreateTopBar();
            stack.HeightRequest = App.ScreenSize.Height - topbar.HeightRequest;

            var titleBar = new Label
            {
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = 48,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                BackgroundColor = FormsConstants.AppyBlue,
                TextColor = Color.White,
                Text = Langs.Const_Screen_Title_Notifications
            };

            var mainGrid = new Grid
            {
                WidthRequest = App.ScreenSize.Width * .9,
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = App.ScreenSize.Height * .2},
                    new RowDefinition { Height = 1},
                    new RowDefinition {Height = App.ScreenSize.Height * .15},
                    new RowDefinition {Height = 1},
                    new RowDefinition { Height = GridLength.Star}
                }
            };

            mainGrid.Children.Add(new BoxView { HeightRequest = 1, WidthRequest = App.ScreenSize.Width, BackgroundColor = Color.White },0,1);
            mainGrid.Children.Add(new BoxView { HeightRequest = 1, WidthRequest = App.ScreenSize.Width, BackgroundColor = Color.White },0,3);

            var topGrid = new Grid
            {
                WidthRequest = App.ScreenSize.Width * .9,
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = (App.ScreenSize.Width *.9) *.33},
                    new ColumnDefinition {Width = (App.ScreenSize.Width *.9) *.34},
                    new ColumnDefinition {Width = (App.ScreenSize.Width *.9) *.33},
                }
            };

            var btnRead = new Button
            {
                WidthRequest = App.ScreenSize.Width * .3,
                BorderRadius = 6,
                BorderColor = Color.Black,
                Text = Langs.Const_Button_Notifications_Tab3,
                TextColor = Color.Black
            };
            btnRead.SetBinding(Button.BackgroundColorProperty, new Binding("ReadFilter", converter: new NotificationFilterToBGColor()));
            btnRead.Clicked += (sender, e) => {ViewModel.SetFilter("Read");};

            var btnUnread = new Button
            {
                WidthRequest = App.ScreenSize.Width * .3,
                BorderRadius = 6,
                BorderColor = Color.Black,
                Text = Langs.Const_Button_Notifications_Tab2,
                TextColor = Color.Black                   
            };
            btnUnread.SetBinding(Button.BackgroundColorProperty, new Binding("UnreadFilter", converter: new NotificationFilterToBGColor()));
            btnUnread.Clicked += (sender, e) => {ViewModel.SetFilter("Unread");};

            var btnAll = new Button
            {
                WidthRequest = App.ScreenSize.Width * .3,
                BorderRadius = 6,
                BorderColor = Color.Black,
                Text = Langs.Const_Button_Notifications_Tab1,
                TextColor = Color.Black
            };
            btnAll.SetBinding(Button.BackgroundColorProperty, new Binding("AllFilter", converter: new NotificationFilterToBGColor()));
            btnAll.Clicked += (sender, e) => {ViewModel.SetFilter("All");};

            topGrid.Children.Add(
                new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Children = {btnAll}
                }, 0, 0);
            topGrid.Children.Add(
                new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                Children = { btnUnread }
                }, 1, 0);
            topGrid.Children.Add(
                new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                Children = { btnRead }
                }, 2, 0);

            mainGrid.Children.Add(new StackLayout {HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Children = { topGrid } }, 0, 0);

            var imgNotifications = new Image
            {
                Source = "icon_0".CorrectedImageSource(),
                HeightRequest = 36
            };
            imgNotifications.SetBinding(Image.IsVisibleProperty, new Binding("GetHasNotifications"));

            var midGrid = new Grid
            {
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = App.ScreenSize.Height * .15,
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = App.ScreenSize.Width * .5},
                    new ColumnDefinition {Width = 1},
                    new ColumnDefinition {Width = GridLength.Star}
                }
            };

            midGrid.Children.Add
                   (
                       new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children = 
                {
                    imgNotifications,
                    new Label {Text = Langs.Const_Label_Notifications, FontFamily = Helper.RegFont, FontSize = 16, TextColor = Color.White}
                }
            },0,0);
            midGrid.Children.Add(new BoxView { HeightRequest = App.ScreenSize.Height * .15, WidthRequest = 1, BackgroundColor = Color.White },1,0);

            var lblCount = new Label
            {
                Text = $"{ViewModel.GetUnreadNotifications}",
                FontSize = 16,
                TextColor = Color.White,
                FontFamily = Helper.RegFont
            };
            midGrid.Children.Add(new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    lblCount,
                    new Label {Text = Langs.Const_Button_Notifications_Tab2, FontFamily = Helper.RegFont, FontSize = 16, TextColor = Color.White}
                }
            },2,0);

            mainGrid.Children.Add(midGrid, 0, 2);

            listNotes = new ListView(ListViewCachingStrategy.RecycleElement)
            {
                ItemsSource = ViewModel.SmallNoteList,
                ItemTemplate = new DataTemplate(typeof(NotificationViewCell)),
                SeparatorVisibility = SeparatorVisibility.None
            };
            listNotes.ItemSelected += (sender, e) => 
            {
                var note = e.SelectedItem as SmallNotificationModel;
                if (note != null)
                {
                    ViewModel.SelectedJourneyId = note.JourneyId;
                    var notification = ViewModel.Notifications.FirstOrDefault(t => t.JourneyId == note.JourneyId);
                    notificationView.EventDate = notification.DateString;
                    notificationView.EventJourneyId = notification.JourneyId;
                    notificationView.EventNumber = $"{notification.EventCount} {Langs.Const_Label_Warnings}";
                    notificationView.EventsListSource = notification.Events;
                    mainGrid.IsVisible = false;
                    stack.Children.Add(notificationView);
                }
            };

            mainGrid.Children.Add(listNotes, 0, 4);
            stack.Children.Add(mainGrid);
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

