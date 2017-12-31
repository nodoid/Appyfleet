using mvvmframework.Languages;
using mvvmframework.Models;
using NewAppyFleet.Converters;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ListViewCells
{
    public class JourneyListViewCell
    {
        public static StackLayout JourneyListCell(JourneyDetails jd)
        {
            var outerGrid = new Grid();
            outerGrid.Children.Add(
                new Label
                {
                    FontFamily = Helper.BoldFont,
                    FontSize = 16,
                    Text = jd.JourneyDateTime,
                    TextColor = Color.White
                });

            var mainStack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Start,
                WidthRequest = App.ScreenSize.Width * .95,
                Children = {outerGrid}
            };
            foreach (var j in jd.Journey)
            {
                var grid = new Grid
                {
                    WidthRequest = App.ScreenSize.Width,
                    BackgroundColor = FormsConstants.AppyDarkShade,
                    HorizontalOptions = LayoutOptions.Start,
                    RowSpacing = 0
            };
                grid.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition {Width = App.ScreenSize.Width * .2},
                new ColumnDefinition {Width = App.ScreenSize.Width * .55},
                new ColumnDefinition {Width = App.ScreenSize.Width * .15},
                new ColumnDefinition {Width = App.ScreenSize.Width * .1}
            };
                grid.RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition {Height = 24},
                new RowDefinition {Height = GridLength.Auto},
                new RowDefinition {Height = 24},
            };
                var lblJourneyType = new Label
                {
                    TextColor = Color.White,
                    FontFamily = Helper.BoldFont,
                    FontSize = 12,
                    Text = j.Private ? Langs.Const_Label_Private : Langs.Const_Label_Business
                };

                var lblRegistration = new Label
                {
                    Text = j.Nickname,
                    FontFamily = Helper.BoldFont,
                    FontSize = 12,
                    TextColor = Color.White
                };

                var lblScore = new Label
                {
                    TextColor = Color.White,
                    FontSize = 24,
                    HeightRequest = 60,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontFamily = Helper.RegFont,
                    Text = j.OverallScore.ToString(),
                    BackgroundColor = j.OverallScore < 0  ? FormsConstants.AppyRed : FormsConstants.AppyGreen
                };

                var lblStartTime = new Label
                {
                    TextColor = Color.White,
                    FontFamily = Helper.BoldFont,
                    FontSize = 10,
                    Text = j.StartTime
                };

                var lblArriveTime = new Label
                {
                    TextColor = Color.White,
                    FontFamily = Helper.BoldFont,
                    FontSize = 10,
                    Text = j.EndTime
                };

                var lblDepartPlace = new Label
                {
                    TextColor = Color.White,
                    FontFamily = Helper.RegFont,
                    FontSize = 10,
                    Text = j.StartLocation
                };

                var lblArrivePlace = new Label
                {
                    TextColor = Color.White,
                    FontFamily = Helper.RegFont,
                    FontSize = 10,
                    Text = j.EndLocation
                };

                var lblDate = new Label
                {
                    TextColor = Color.White,
                    FontFamily = Helper.RegFont,
                    FontSize = 10,
                    Text = j.StartDateString
                };

                var lblYear = new Label
                {
                    TextColor = Color.White,
                    FontFamily = Helper.BoldFont,
                    FontSize = 10,
                    Text = j.StartDateYearString
                };

                var lblDistance = new Label
                {
                    TextColor = Color.White,
                    FontFamily = Helper.RegFont,
                    FontSize = 10,
                   Text = j.Miles.ToString()
                };

                var stackOutTime = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Padding= new Thickness(8,0,0,0),
                    Children =
                {
                    new Image {Source = "radio_off_light".CorrectedImageSource(), HeightRequest = 20},
                    lblStartTime
                }
                };
                var stackInTime = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Padding= new Thickness(8,0,0,0),
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Children =
                {
                    new Image {Source = "radio_on_light".CorrectedImageSource(), HeightRequest = 20},
                    lblArriveTime
                }
                };

                var imgNext = new Image
                {
                    Source = "ic_keyboard_arrow_right_white_24dp".CorrectedImageSource(),
                    HeightRequest = 32,
                    ClassId = j.Id.ToString()
                };
                imgNext.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    NumberOfTapsRequired = 1,
                    Command = new Command(() => MessagingCenter.Send("JourneyListCell", "JourneyId", imgNext.ClassId))
                });

                var stackNextArrow = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Padding = new Thickness(0,0,0,8),
                    Children = { imgNext }
                };

                var stackScore = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Children = { lblScore }
                };
                stackScore.SetBinding(StackLayout.BackgroundColorProperty, new Binding("OverallScore", converter: new ScoreToColor()));

                var imgViewAll = new Image
                {
                    Source = "ic_keyboard_arrow_right_white_24dp".CorrectedImageSource(),
                    HeightRequest = 20
                };
                var lblViewAll = new Label
                {
                    TextColor = Color.White,
                    FontFamily = Helper.RegFont,
                    FontSize = 10,
                    Text = Langs.Const_Button_View_All_Journeys
                };

                grid.Children.Add(lblJourneyType,0,0);
                grid.Children.Add(lblRegistration, 1, 0);
                grid.Children.Add(stackScore, 2, 1);
                grid.Children.Add(stackNextArrow, 3, 1);

                grid.Children.Add(stackOutTime, 0, 1);
                grid.Children.Add(lblDepartPlace, 1, 1);

                grid.Children.Add(stackInTime, 0, 2);
                grid.Children.Add(lblArrivePlace, 1, 2);

                Grid.SetRowSpan(stackScore, 4);
                Grid.SetRowSpan(stackNextArrow, 3);
                mainStack.Children.Add(grid);
            }

            return mainStack;
        }
    }
}
