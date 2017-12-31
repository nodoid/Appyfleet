using System;
using System.Collections.Generic;
using System.Linq;
using mvvmframework.Models;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ViewCells
{
    public class GeneralTopBar : ContentView
    {
        public GeneralTopBar(List<UIAttributes> uiElements)
        {
            var leftViews = new List<View>();
            var rightViews = new List<View>();
            foreach (var ui in uiElements)
            {
                dynamic view= null;

                if (ui.IsImageSource)
                {
                    var imgView = new Image { Source = ui.Text, HeightRequest = 32, StyleId = $"{ui.Position}" };
                    if (ui.ClickEvent != null)
                        imgView.GestureRecognizers.Add(new TapGestureRecognizer { NumberOfTapsRequired = 1, Command = new Command(() => ui.ClickEvent(this, null)) });
                    view = imgView;
                }
                else
                {
                    var lblView = new Label { Text = ui.Text, TextColor = Color.White, VerticalTextAlignment = TextAlignment.Center, FontFamily = ui.TextBold ? Helper.BoldFont : Helper.RegFont, StyleId = $"{ui.Position}" };
                    if (ui.ClickEvent != null)
                            lblView.GestureRecognizers.Add(new TapGestureRecognizer { NumberOfTapsRequired = 1, Command = new Command(() => ui.ClickEvent(this,null)) });
                    view = lblView;
                }

                // split position
                if (ui.ScreenLeft)
                    leftViews.Add(view as View);
                else
                    rightViews.Add(view as View);
            }

            // order them 
            leftViews = leftViews.OrderBy(t => Convert.ToInt32(t.StyleId)).ToList();
            rightViews = rightViews.OrderByDescending(t => Convert.ToInt32(t.StyleId)).ToList();

            var grid = new Grid
            {
                WidthRequest = App.ScreenSize.Width * .96,
                HeightRequest = 40,
                MinimumHeightRequest = 40
            };
            grid.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition {Width = App.ScreenSize.Width * .48},
                new ColumnDefinition {Width = App.ScreenSize.Width * .48}
            };

            var leftStack = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Start
            };
            var rightStack = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.End
            };

            if (leftViews.Count != 0)
            {
                foreach (var v in leftViews)
                    leftStack.Children.Add(v);
            };
            if (rightViews.Count != 0)
            {
                foreach (var v in rightViews)
                    rightStack.Children.Add(v);
            }
            grid.Children.Add(leftStack, 0, 0);
            grid.Children.Add(rightStack, 1, 0);

            var stk = new StackLayout
            {
                WidthRequest = MinimumWidthRequest = App.ScreenSize.Width,
                HeightRequest = 40,
                Padding = new Thickness(4,0,0,0),
                BackgroundColor = FormsConstants.AppyLightBlue,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    grid
                }
            };

            Content = stk;
        }
    }
}
