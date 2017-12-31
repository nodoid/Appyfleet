using mvvmframework;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;
using NewAppyFleet.Views;

namespace NewAppyFleet
{
    public class TopBar
    {
        ObservableCollection<View> views = new ObservableCollection<View>();
        string Title, LeftImage, RightImage;
        readonly Page currentPage;
        static StackLayout panel;
        public StackLayout Panel
        {
            get => panel;
            set { if (panel != value) { panel = value; } } 
        }

        bool GoBack;
        Image rightCell;
        Grid grid;
        MenuView menu;
        double opacity;
        bool FromMain;
        bool LoggedIn;

        public TopBar(bool loggedIn, string text = "", Page current = null, double op = 1, string leftImage = "",
                      string rightImage = "", StackLayout stk = null,  bool goBack = false, bool fromMain = true)
        {
            Title = text;
            LeftImage = leftImage;
            RightImage = rightImage;
            currentPage = current;
            LoggedIn = loggedIn;
            if (menu == null)
            {
                menu = new MenuView();
            }
            Panel = stk;

            /*Panel.ChildAdded += Panel_ChildAdded;
            Panel.ChildRemoved += Panel_ChildRemoved;
            Panel.ChildrenReordered += Panel_ChildrenReordered;*/

            GoBack = goBack;
            FromMain = fromMain;
            opacity = op;
        }

        void Panel_ChildrenReordered(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        void Panel_ChildRemoved(object sender, ElementEventArgs e)
        {
            Debug.WriteLine("Element removed");
        }

        void Panel_ChildAdded(object sender, ElementEventArgs e)
        {
            var _ = Panel;
            Panel = null;
            views.Add(e.Element as View);
            Panel = _;
        }

        public Grid CreateTopBar(StackLayout stk = null)
        {
            grid = new Grid
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = FormsConstants.AppyLightBlue,
                Opacity = opacity,
                HeightRequest = 48,
                MinimumHeightRequest = 48,
                WidthRequest = App.ScreenSize.Width,
                MinimumWidthRequest = App.ScreenSize.Width,
                ColumnDefinitions =
                {
                    new ColumnDefinition{Width = 64},
                    new ColumnDefinition{Width = App.ScreenSize.Width - 144},
                    new ColumnDefinition{Width = 64}
                },
                RowDefinitions =
                {
                    new RowDefinition{Height = GridLength.Star}
                }
            };

            if (menu != null)
            {
                menu.HeightRequest = (App.ScreenSize.Height - grid.HeightRequest) + 16;
                menu.MinimumHeightRequest = menu.HeightRequest;
                menu.TranslationY = -8;
            }

            var padView = new ContentView
            {
                WidthRequest = 8
            };

            var leftView = new ContentView
            {
                HeightRequest = 16,
                WidthRequest = 16
            };
            var rightView = new ContentView
            {
                HeightRequest = 16,
                WidthRequest = 32
            };

            var leftCell = new Image();
            if (!string.IsNullOrEmpty(LeftImage))
            {
                leftCell.Source = LeftImage.CorrectedImageSource();
                leftCell.HeightRequest = 16;
                leftCell.WidthRequest = 32;
                leftCell.VerticalOptions = LayoutOptions.Center;
                leftCell.HorizontalOptions = LayoutOptions.Start;
                if (GoBack)
                {
                    var gestBack = new TapGestureRecognizer
                    {
                        NumberOfTapsRequired = 1,
                        Command = new Command(async () => await currentPage.Navigation.PopAsync())
                    };
                    leftCell.GestureRecognizers.Add(gestBack);
                }
                else
                {
                    if (Panel.Children.Count != views.Count)
                    {
                        Panel.Children.Clear();
                        foreach (var v in views)
                            Panel.Children.Add(v);
                        leftCell.GestureRecognizers.Add(MenuGesture(stk));
                    }
                    else
                        leftCell.GestureRecognizers.Add(MenuGesture(stk));
                }
                leftView.Content = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children = { padView, leftCell }
                };
            }

            var title = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    //new Label {Text = Title, TextColor = Color.White, FontSize = 32, FontAttributes = FontAttributes.Bold | FontAttributes.Italic}
                    new Image {Source = "smile_icon".CorrectedImageSource(), HeightRequest= 36}
                }
            };
            title.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(async () => { if (LoggedIn) await currentPage.Navigation.PushAsync(new DashboardPage(true));
                })
            });

            rightCell = new Image();

            if (!string.IsNullOrEmpty(RightImage))
            {
                var imgProfile = new Image
                {
                    Source = "icon_user".CorrectedImageSource(),
                    HeightRequest = 16,
                    WidthRequest = 32,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.EndAndExpand
                };
                imgProfile.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    NumberOfTapsRequired = 1,
                    Command = new Command(async () => await currentPage.Navigation.PushAsync(new ProfilePage()))
                });

                rightCell.Source = RightImage.CorrectedImageSource();
                rightCell.HeightRequest = rightCell.WidthRequest = 32;
                rightCell.VerticalOptions = LayoutOptions.Center;
                rightCell.HorizontalOptions = LayoutOptions.End;
                rightCell.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    NumberOfTapsRequired = 1,
                    Command = new Command(() =>
                    {
                        if (RightImage.Contains("refresh"))
                                MessagingCenter.Send("refresh", "doit");
                    })
                });

                rightView.Content = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Padding = new Thickness(0,0,0,4),
                    Children = { rightCell, imgProfile }
                };
            }
            grid.Children.Add(leftView, 0, 0);
            grid.Children.Add(new StackLayout{HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Children ={title}}, 1, 0);
            grid.Children.Add(rightView, 2, 0);
            return grid;
        }

        TapGestureRecognizer MenuGesture(StackLayout stk)
        {
            var origBounds = new Rectangle();
            var bounds = new Rectangle();
            return new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command((p) =>
                {
                    if (!App.Self.PanelShowing)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            Panel.WidthRequest = Panel.Width + menu.Content.WidthRequest;

                            if (Panel.Children.Count == 1)
                            {
                                /*Panel.Children.Add(new StackLayout
                                {
                                    Children = { menu }
                                });*/
                                Panel.Children.Insert(0, menu);
                                await Panel.Children[0].LayoutTo(new Rectangle(-App.ScreenSize.Width, 0, App.ScreenSize.Width, App.ScreenSize.Height));
                            }

                            bounds = Panel.Children[1].Bounds;
                            bounds.X = 0;
                            bounds.Width = App.ScreenSize.Width;

                            origBounds = Panel.Children[0].Bounds;
                            Panel.Children[1].Opacity = 0;

                            await Panel.Children[0].LayoutTo(bounds, (uint)Constants.MenuSlideInTime, Easing.CubicIn);
                            App.Self.PanelShowing = true;
                        });
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            if (Panel.Children.Count > 1)
                            {
                                await Panel.Children[0].LayoutTo(origBounds, (uint)Constants.MenuSlideOutTime, Easing.CubicOut).ContinueWith((t) =>
                                {
                                    if (t.IsCompleted)
                                    {
                                        Device.BeginInvokeOnMainThread(async () =>
                                        {
                                            Panel.Children[1].Opacity = 1;
                                            await Panel.Children[1].LayoutTo(bounds, (uint)Constants.MenuSlideOutTime, Easing.CubicOut).ContinueWith((_) =>
                                            {
                                                if (_.IsCompleted)
                                                {
                                                    App.Self.PanelShowing = false;
                                                }
                                            });
                                        });
                                    }
                                });
                            }
                        });
                    }
                })
            };
        }
    }
}
