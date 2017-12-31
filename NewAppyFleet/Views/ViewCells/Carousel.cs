using mvvmframework.Languages;
using mvvmframework.ViewModels;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ViewCells
{
    public class Carousel
    {
        public static StackLayout GroupScores(DashboardViewModel Vm)
        {
            if (Vm.GroupScores.Count == 0)
                return new StackLayout{HeightRequest = 120, WidthRequest = App.ScreenSize.Width};

            var grid = new Grid
            {
                RowSpacing = 0,
                ColumnSpacing = 0,
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = 24},
                    new ColumnDefinition { Width = App.ScreenSize.Width - 48},
                    new ColumnDefinition {Width = 24},
                },
                RowDefinitions= new RowDefinitionCollection
                {
                    new RowDefinition {Height = 120}
                }
            };

            var leftArrow = new Label { BackgroundColor = Color.LightGray, TextColor = Color.Black, Text = "<", 
                HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };
            leftArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(() => MessagingCenter.Send("Carousel", "Movement", "Back"))
            });
            leftArrow.SetBinding(Label.IsEnabledProperty, new Binding("GoBackEnabled"));
            var rightArrow = new Label { BackgroundColor = Color.LightGray, TextColor = Color.Black, Text = ">",
                HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };
            rightArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(() => MessagingCenter.Send("Carousel", "Movement", "Forward"))
            });
            rightArrow.SetBinding(Label.IsEnabledProperty, new Binding("GoForwardEnabled"));

            grid.Children.Add(leftArrow, 0, 0);
            grid.Children.Add(rightArrow, 2, 0);

            var midWidth = App.ScreenSize.Width - 48;

            var mainGrid = new Grid
            {
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = 120,
                ColumnSpacing = 0,
                RowSpacing = 0,
                HorizontalOptions = LayoutOptions.Center,
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = 60},
                    new RowDefinition {Height = 60}
                }
            };

            var topGrid = new Grid
            {
                HeightRequest = 60,
                WidthRequest = App.ScreenSize.Width,
                ColumnSpacing = 0,
                RowSpacing = 0,
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = 60}
                },
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = midWidth * .9},
                    new ColumnDefinition {Width = midWidth * .1}
                }
            };

            var btnGrid = new Grid
            {
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = 60,
                ColumnSpacing = 0,
                RowSpacing = 0,
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = 1},
                    new RowDefinition {Height = 20},
                    new RowDefinition {Height = 39}
                },
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = midWidth * .5},
                    new ColumnDefinition {Width = midWidth * .5}
                }
            };

            var boxView = new BoxView { HeightRequest = 1, WidthRequest = App.ScreenSize.Width, BackgroundColor = Color.White };

            btnGrid.Children.Add(boxView, 0, 0);

            Grid.SetColumnSpan(boxView, 2);

            var lblGroupName = new Label
            {
                FontSize = 12,
                FontFamily = Helper.RegFont,
                TextColor = Color.White
            };
            lblGroupName.SetBinding(Label.TextProperty, new Binding("CurrentName"));
            var lblRank = new Label
            {
                FontSize = 32,
                FontFamily = Helper.BoldFont,
                TextColor = Color.White,
            };
            lblRank.SetBinding(Label.TextProperty, new Binding("CurrentRank"));
            var lblRankOutOf = new Label
            {
                FontSize = 16,
                FontFamily = Helper.BoldFont,
                TextColor = Color.White,
            };
            lblRankOutOf.SetBinding(Label.TextProperty, new Binding("CurrentRankOutOf"));

            var scoreStack = new StackLayout
            {
                WidthRequest = App.ScreenSize.Width * .9,
                HeightRequest = 49,
                Padding = new Thickness(12, 0),
                Children =
                {
                    lblGroupName,
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            lblRank,
                            lblRankOutOf
                        }
                    }
                }
            };

            var lblPosn = new Label {TextColor = Color.White, FontSize = 12, FontFamily = Helper.RegFont };
            lblPosn.SetBinding(Label.TextProperty, new Binding("CurrentRank"));

            topGrid.Children.Add(scoreStack, 0, 0);
            topGrid.Children.Add(lblPosn, 1, 0);

            btnGrid.Children.Add(new StackLayout
            {
                Padding = new Thickness(12, 0),
                BackgroundColor = FormsConstants.AppyDarkBlue,
                Children =
                {
                    new Label
                    {
                        HeightRequest = 20,
                        VerticalTextAlignment = TextAlignment.Center,
                        Text = Langs.Const_Label_Team_Score,
                        TextColor = Color.White,
                        FontFamily = Helper.RegFont
                    }
                }
            }, 0, 1);
            var lblScore = new Label
            {
                HeightRequest = 29,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White,
                FontFamily = Helper.RegFont,
                FontSize = 20
            };
            lblScore.SetBinding(Label.TextProperty, new Binding("CurrentScore"));
            btnGrid.Children.Add(new StackLayout
            {
                Padding = new Thickness(12, 0),
                BackgroundColor = FormsConstants.AppyDarkBlue,
                Children =
                {
                    lblScore
                }
            }, 0, 2);

            //var scr = score.Score - dcs;

            btnGrid.Children.Add(new StackLayout
            {
                Padding = new Thickness(12, 0),
                BackgroundColor = Vm.CurrentScoreDifference < 0 ? FormsConstants.AppyLightRed : (Vm.CurrentScoreDifference == 0 ? FormsConstants.AppyYellow : FormsConstants.AppyGreen),
                Children =
                {
                    new Label
                    {
                        HeightRequest = 20,
                        VerticalTextAlignment = TextAlignment.Center,
                        Text = Langs.Const_Label_Your_Difference,
                        TextColor = Color.White,
                        FontFamily = Helper.RegFont
                    }
                }
            }, 1, 1);

            var lblDiffScore = new Label
            {
                HeightRequest = 29,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White,
                FontFamily = Helper.RegFont,
                FontSize = 20
            };
            lblDiffScore.SetBinding(Label.TextProperty, new Binding("CurrentScoreDifference"));

            btnGrid.Children.Add(new StackLayout
            {
                Padding = new Thickness(12, 0),
                BackgroundColor = Vm.CurrentScoreDifference < 0 ? FormsConstants.AppyLightRed : (Vm.CurrentScoreDifference == 0 ? FormsConstants.AppyYellow : FormsConstants.AppyGreen),
                Children =
                {
                    lblDiffScore
                }
            }, 1, 2);

            mainGrid.Children.Add(topGrid, 0, 0);
            mainGrid.Children.Add(btnGrid, 0, 1);

            grid.Children.Add(mainGrid, 1, 0);

            var carouselLayout = new StackLayout
            {
                HeightRequest = 120,
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Start,
                Children =
                    {
                        grid
                    }
            };

            return carouselLayout;
        }
    }
}
