using mvvmframework.Languages;
using NewAppyFleet.Converters;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ListViewCells
{
    public class JourneyViewCell : ViewCell
    {
        public JourneyViewCell()
        {
            var grid = new Grid
            {
                WidthRequest = App.ScreenSize.Width * .95,
                HorizontalOptions = LayoutOptions.Start
            };
            grid.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition {Width = App.ScreenSize.Width * .2},
                new ColumnDefinition {Width = App.ScreenSize.Width * .5},
                new ColumnDefinition {Width = App.ScreenSize.Width * .15},
                new ColumnDefinition {Width = App.ScreenSize.Width * .1}
            };
            grid.RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition {Height = 24},
                new RowDefinition {Height = 48},
                new RowDefinition {Height = 24},
            };

            var lblJourneyType = new Label
            {
                TextColor = Color.White,
                FontFamily = Helper.RegFont,
                FontSize = 10
            };
            lblJourneyType.SetBinding(Label.TextProperty, new Binding("JourneyType"));

            var lblRegistration = new Label
            {
                TextColor = Color.White,
                FontFamily = Helper.RegFont,
                FontSize = 10
            };
            lblRegistration.SetBinding(Label.TextProperty, new Binding("Registration"));

            var lblScore = new Label
            {
                TextColor = Color.White,
                FontSize = 24,
                HeightRequest = 60,
                VerticalTextAlignment = TextAlignment.Center,
                FontFamily = Helper.RegFont
            };
            lblScore.SetBinding(Label.BackgroundColorProperty, new Binding("OverallScore", converter: new ScoreToColor()));
            lblScore.SetBinding(Label.TextProperty, new Binding("OverallScore"));

            var lblStartTime = new Label
            {
                TextColor = Color.White,
                FontFamily = Helper.BoldFont,
                FontSize = 10
            };
            lblStartTime.SetBinding(Label.TextProperty, new Binding("StartTime"));

            var lblArriveTime = new Label
            {
                TextColor = Color.White,
                FontFamily = Helper.BoldFont,
                FontSize = 10
            };
            lblArriveTime.SetBinding(Label.TextProperty, new Binding("EndTime"));

            var lblDepartPlace = new Label
            {
                TextColor = Color.White,
                FontFamily = Helper.RegFont,
                FontSize = 10
            };
            lblDepartPlace.SetBinding(Label.TextProperty, new Binding("StartLocation"));

            var lblArrivePlace = new Label
            {
                TextColor = Color.White,
                FontFamily = Helper.RegFont,
                FontSize = 10
            };
            lblArrivePlace.SetBinding(Label.TextProperty, new Binding("EndLocation"));

            var lblDate = new Label
            {
                TextColor = Color.White,
                FontFamily = Helper.RegFont,
                FontSize = 10
            };
            lblDate.SetBinding(Label.TextProperty, new Binding("StartDateString"));

            var lblYear = new Label
            {
                TextColor = Color.White,
                FontFamily = Helper.BoldFont,
                FontSize = 10
            };
            lblYear.SetBinding(Label.TextProperty, new Binding("StartDateYearString"));

            var lblDistance = new Label
            {
                TextColor = Color.White,
                FontFamily = Helper.RegFont,
                FontSize = 10
            };
            lblDistance.SetBinding(Label.TextProperty, new Binding("Miles"));

            var stackOutTime = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    new Image {Source = "radio_off_light".CorrectedImageSource(), HeightRequest = 20},
                    lblStartTime
                }
            };
            var stackInTime = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
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
                HeightRequest = 32
            };
            imgNext.SetBinding(Image.ClassIdProperty, new Binding("JourneyId"));
            imgNext.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(() => MessagingCenter.Send(this, "JourneyId", imgNext.ClassId))
            });

            var stackNextArrow = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                Children ={imgNext}
            };

            var stackScore = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children = {lblScore}
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
                FontSize = 12,
                Text = Langs.Const_Button_View_All_Journeys
            };

            var stackViewAll = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Children = { lblViewAll, imgViewAll }
            };
            stackViewAll.GestureRecognizers.Add(new TapGestureRecognizer 
            { 
                NumberOfTapsRequired = 1, Command = new Command(() => MessagingCenter.Send(this, "ShowAllJourneys")) 
            });

            grid.Children.Add(lblJourneyType, 0, 0);
            grid.Children.Add(lblRegistration, 1, 0);
            grid.Children.Add(stackScore, 2, 1);
            grid.Children.Add(stackNextArrow, 3, 1);

            grid.Children.Add(stackOutTime, 0, 1);
            grid.Children.Add(lblDepartPlace, 1, 1);

            grid.Children.Add(stackInTime, 0, 2);
            grid.Children.Add(lblArrivePlace, 1, 2);

            Grid.SetRowSpan(stackScore, 3);
            Grid.SetRowSpan(stackNextArrow, 3);

            var topGrid = new Grid
            {
                WidthRequest = App.ScreenSize.Width * .95,
                HorizontalOptions = LayoutOptions.Start
            };
            topGrid.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition {Width = App.ScreenSize.Width * .3},
                new ColumnDefinition {Width = App.ScreenSize.Width * .4},
                new ColumnDefinition {Width = GridLength.Auto}
            };

            var stackDate = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { lblDate, lblYear }
            };

            var stackDistance = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = {
                    lblDistance,
                    new Label
                    {
                        TextColor = Color.White,
                        FontFamily = Helper.RegFont,
                        FontSize = 10,
                        Text = Langs.Const_Button_Map_Tab3
                    }}
            };
            topGrid.Children.Add(stackDate, 0, 0);
            topGrid.Children.Add(stackDistance, 1, 0);
            topGrid.Children.Add(new Label { Text = Langs.Const_Label_Journey_Score, TextColor = Color.White, FontSize = 10 }, 2, 0);

            View = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                WidthRequest = App.ScreenSize.Width,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    topGrid,
                    grid,
                    stackViewAll
                }
            };
        }
    }
}
