using mvvmframework.Languages;
using mvvmframework.Models;
using NewAppyFleet.Converters;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ListViewCells
{
    public class RankingViewCell : ViewCell
    {
        public RankingViewCell()
        {
            var lblTeamRank = new Label
            {
                Text = Langs.Const_Label_Group_Rank,
                FontFamily = Helper.RegFont
            };
            var lblFleetRank = new Label
            {
                Text = Langs.Const_Label_Fleet_Rank,
                FontFamily = Helper.RegFont
            };
            var lblCurrentRank = new Label
            {
                FontSize = 28,
                FontFamily = Helper.RegFont
            };
            lblCurrentRank.SetBinding(Label.TextProperty,new Binding("Rank"));

            var lblOutOfRank = new Label
            {
                FontFamily = Helper.RegFont
            };
            lblOutOfRank.SetBinding(Label.TextProperty, new Binding("Total"));

            var lblAveScore = new Label
            {
                FontFamily = Helper.RegFont,
                FontSize = 28
            };

            var lblYourDiff = new Label
            {
                FontFamily = Helper.RegFont,
                FontSize = 28
            };
            lblYourDiff.SetBinding(Label.TextProperty, new Binding("Difference"));
            lblYourDiff.SetBinding(Label.BackgroundColorProperty, new Binding("Difference", converter: new DiffScoreToColor()));

            var lblSlash = new Label
            {
                Text = "/",
                FontSize = 18,
                FontFamily = Helper.RegFont
            };

            var aveTitle = string.Empty;
            var ysColor = FormsConstants.AppyYellow;

            lblCurrentRank.BindingContextChanged += (sender, e) => 
            {
                var bc = BindingContext as Rankings;
                if (bc != null)
                {
                    if (bc.IsCompany)
                    {
                        lblTeamRank.TextColor = FormsConstants.AppyLightGray;
                        lblFleetRank.TextColor = Color.White;
                        lblAveScore.Text = $"{bc.FleetScore}";
                        aveTitle = Langs.Const_Label_Average_Fleet_Score;
                    }
                    else
                    {
                        lblFleetRank.TextColor = FormsConstants.AppyLightGray;
                        lblTeamRank.TextColor = Color.White;
                        lblAveScore.Text = $"{bc.Score}";
                        aveTitle = Langs.Const_Label_Team_Score;
                    }
                    if (bc.Difference != 0)
                        lblYourDiff.Text = bc.Difference > 0 ? $"+{bc.Difference}" : $"-{bc.Difference}";

                    if (bc.Difference == 0)
                        ysColor = FormsConstants.AppyYellow;
                    else
                    {
                        if (bc.Difference < 0)
                            ysColor = FormsConstants.AppyRed;
                        else
                            ysColor = FormsConstants.AppyGreen;
                    }
                        
                }
            };

            var grid = new Grid
            {
                WidthRequest = App.ScreenSize.Width * .95,
            };
            grid.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition {Width = (App.ScreenSize.Width * .95)/2},
                new ColumnDefinition {Width = (App.ScreenSize.Width * .95)/2},
            };

            var stackAve = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                WidthRequest = (App.ScreenSize.Width * .95) / 2,
                Children =
                {
                    new StackLayout
                    {
                        BackgroundColor = FormsConstants.AppyDarkShade,
                        Children = {new Label {Text = aveTitle, TextColor = Color.White, FontFamily = Helper.RegFont}}
                    },
                    new StackLayout
                    {
                        BackgroundColor = FormsConstants.AppyDarkBlue,
                        VerticalOptions = LayoutOptions.Start,
                        Children = {lblAveScore}
                    }
                }
            };

            var stackDiff = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                WidthRequest = (App.ScreenSize.Width * .95) / 2,
                Children =
                {
                    new StackLayout
                    {
                        BackgroundColor = ysColor,
                        Children = {new Label {Text = Langs.Const_Label_Your_Difference, TextColor = Color.White, FontFamily = Helper.RegFont}}
                    },
                    new StackLayout
                    {
                        BackgroundColor = FormsConstants.AppyDarkBlue,
                        VerticalOptions = LayoutOptions.Start,
                        Children = {lblAveScore}
                    }
                }
            };

            var stackRank = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = FormsConstants.AppyDarkBlue,
                WidthRequest = App.ScreenSize.Width * .95,
                Children =
                {
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children = {lblTeamRank, lblFleetRank}
                    },
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        VerticalOptions = LayoutOptions.Start,
                        Children = {lblCurrentRank, lblSlash, lblOutOfRank}
                    }
                }
            };

            View = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                WidthRequest = App.ScreenSize.Width,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    stackRank,
                    new BoxView {WidthRequest = App.ScreenSize.Width, HeightRequest = 1, BackgroundColor = Color.White},
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children = {stackAve, stackDiff}
                    }
                }
            };
        }
    }
}
