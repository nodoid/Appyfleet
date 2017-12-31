using System;
using mvvmframework.Languages;
using mvvmframework.Models;
using NewAppyFleet.Converters;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ListViewCells
{
    public class ExpensesViewCell : ViewCell
    {
        public ExpensesViewCell()
        {
            var topGrid = new Grid
            {
                WidthRequest = App.ScreenSize.Width,
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = 36
            };
            topGrid.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition {Width = App.ScreenSize.Width * .75},
                new ColumnDefinition {Width = GridLength.Auto}
            };

            var lblDistance = new Label
            {
                FontSize = 28,
                FontFamily = Helper.RegFont,
                TextColor = Color.White
            };
            lblDistance.SetBinding(Label.TextProperty, new Binding("Miles", converter: new DoubleToOneDpString()));

            var imgSelected = new Image
            {
                WidthRequest = 40
            };
            imgSelected.SetBinding(Image.SourceProperty, new Binding("ImageName"));
            imgSelected.SetBinding(Image.ClassIdProperty, new Binding("JourneyId"));
            imgSelected.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(() => MessagingCenter.Send(this, "Selected", imgSelected.ClassId))
            });

            var imgNotifications = new Image
            {
                Source = "main_menu_notifications".CorrectedImageSource(),
                HeightRequest = 32
            };
            imgSelected.SetBinding(Image.ClassIdProperty, new Binding("JourneyId"));
            imgNotifications.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(() => MessagingCenter.Send(this, "Notifications", imgNotifications.ClassId))
            });
            imgNotifications.SetBinding(Image.IsVisibleProperty, new Binding("HasNotifications"));

            imgSelected.BindingContextChanged += (sender, e) =>
            {
                var bc = BindingContext as ExpenseModel;
                if (bc != null)
                {
                    imgSelected.Source = bc.ImageName.CorrectedImageSource();
                }
            };

            var lblSelect = new Label
            {
                FontFamily = Helper.RegFont,
                FontSize = 16
            };
            lblSelect.SetBinding(Label.TextProperty, new Binding("SelectedText"));
            lblSelect.SetBinding(Label.ClassIdProperty, new Binding("JourneyId"));

            var distStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(8, 0, 0, 0),
                Children =
                {
                    lblDistance,
                    new Label {Text = Langs.Const_Label_Miles, FontSize = 28, FontFamily = Helper.RegFont},
                    new StackLayout
                    {
                        Padding = new Thickness(12,0),
                        Children = {imgNotifications}
                    }
                }
            };

            var selectStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(0, 0, 0, 8),
                Children = { lblSelect, imgSelected }
            };
            topGrid.Children.Add(distStack, 0, 0);
            topGrid.Children.Add(selectStack, 1, 0);

            var grid = new Grid
            {
                WidthRequest = App.ScreenSize.Width,
                HorizontalOptions = LayoutOptions.Start
            };
            grid.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition {Width = App.ScreenSize.Width * .2},
                new ColumnDefinition {Width = App.ScreenSize.Width * .5},
                new ColumnDefinition {Width = App.ScreenSize.Width * .2},
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
            imgNext.SetBinding(Image.ClassIdProperty, new Binding("MapviewId"));
            imgNext.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(() => MessagingCenter.Send(this, "JourneyId", imgNext.ClassId))
            });

            var stackNextArrow = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                Children = { imgNext }
            };

            var stackScore = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children = { lblScore }
            };
            stackScore.SetBinding(Label.BackgroundColorProperty, new Binding("OverallScore", converter: new ScoreToColor()));

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

            View = new StackLayout
            {
                WidthRequest = App.ScreenSize.Width,
                BackgroundColor = FormsConstants.AppySeaBlue,
                Children =
                {
                    topGrid,
                    new BoxView {HeightRequest = 1, WidthRequest = App.ScreenSize.Width, BackgroundColor = Color.Transparent},
                    grid
                }
            };
        }
    }
}