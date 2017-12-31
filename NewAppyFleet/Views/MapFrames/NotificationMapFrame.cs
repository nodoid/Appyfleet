using System;
using mvvmframework.Languages;
using Xamarin.Forms;

namespace NewAppyFleet.Views.MapFrames
{
    public class NotificationMapFrame
    {
        public Frame GenerateMapAlertFrame(int speed, int roadSpeed, string address, string time, string date, string type)
        {
            var height = App.ScreenSize.Height * .35;
            var width = App.ScreenSize.Width * .85;
            var grid = new Grid
            {
                WidthRequest = width,
                HeightRequest = height,
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = height * .1},
                    new RowDefinition {Height = height * .35},
                    new RowDefinition {Height = GridLength.Auto}
                }
            };

            var topLineGrid = new Grid
            {
                WidthRequest = width,
                HeightRequest = height * .1,
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition{Width = width * .4},
                    new ColumnDefinition{Width = width * .4},
                    new ColumnDefinition{Width = width * .2}
                }
            };

            topLineGrid.Children.Add(new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Children = { new Label { Text = type, FontFamily = Helper.BoldFont, TextColor = Color.White, FontSize = 16 } }
            }, 0, 0);
            topLineGrid.Children.Add(new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Children = { new Label { Text = date, FontFamily = Helper.RegFont, TextColor = Color.White, FontSize = 16 } }
            },1,0);

            /*var imgClose = new Image
            {
                Source = "Close_Button".CorrectedImageSource(),
                WidthRequest = 48,
                HeightRequest = 48,
            };*/
            var imgClose = new Label
            {
                Text = "X",
                FontSize = 16,
                FontFamily = Helper.BoldFont,
                TextColor = Color.White
            };
            imgClose.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(()=>MessagingCenter.Send("alert", "frame","close"))
            });

            topLineGrid.Children.Add(new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Children = { imgClose }
            },2,0);

            grid.Children.Add(topLineGrid, 0, 0);

            var midGrid = new Grid
            {
                WidthRequest = width,
                HeightRequest = height * .35,
                VerticalOptions = LayoutOptions.Center,
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = width * .5},
                    new ColumnDefinition {Width = width * .5}
                }
            };

            midGrid.Children.Add(new StackLayout
            {
                HorizontalOptions= LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = height * .15,
                Children = {new Label {HorizontalTextAlignment = TextAlignment.Center,Text = $"{speed} {Langs.Const_Label_Speed_Unit}",
                        FontSize = 32, FontFamily = Helper.BoldFont,TextColor = Color.White}}
            },0,0);
            midGrid.Children.Add(new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = height * .15,
                Children = {new Label {HorizontalTextAlignment = TextAlignment.Center,Text = $"{time}",
                        FontSize = 32, FontFamily = Helper.BoldFont,TextColor = Color.White}}
            },1,0);

            grid.Children.Add(midGrid, 0,1);

            var btmGrid = new Grid
            {
                WidthRequest = width,
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = width * .5},
                    new ColumnDefinition {Width = width * .5}
                }
            };

            var speedCircle = GenerateCircle(roadSpeed);
            btmGrid.Children.Add(new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions= LayoutOptions.Center,
                Children = {speedCircle }
            }, 0, 0);
            btmGrid.Children.Add(new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Children = {new Label {Text = address, FontSize = 18, FontFamily = Helper.RegFont, LineBreakMode = LineBreakMode.WordWrap,
                        TextColor = Color.White}}
            },1,0);

            grid.Children.Add(btmGrid, 0, 2);

            return new Frame
            {
                BackgroundColor = FormsConstants.AppyRed,
                WidthRequest = App.ScreenSize.Width * .9,
                HeightRequest = App.ScreenSize.Height * .4,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(0),
                Content = grid,
                HasShadow = false,
                CornerRadius = 6
            };
        }

        Grid GenerateCircle(int roadSpeed)
        {
            var imgGrid = new Grid
            {
                WidthRequest = 128,
                HeightRequest = 128,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start
            };

            var imgCircle = new Image
            {
                Source = "sphere".CorrectedImageSource(),
                WidthRequest = 128,
                HeightRequest = 128
            };

            var lblSpeed = new Label
            {
                TextColor = Color.Black,
                FontFamily = Helper.RegFont,
                FontSize = 42,
                Text = $"{roadSpeed}",
                Margin = new Thickness(41, 28, 0, 0),
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Start
            };

            var lblZone = new Label
            {
                TextColor = Color.Black,
                FontFamily = Helper.RegFont,
                VerticalTextAlignment = TextAlignment.Start,
                Text = Langs.Const_Label_Zone,
                Margin = new Thickness(48,76,0,0)
            };

            imgGrid.Children.Add(imgCircle);
            imgGrid.Children.Add(lblSpeed);
            imgGrid.Children.Add(lblZone);

            return imgGrid;
        }
    }
}