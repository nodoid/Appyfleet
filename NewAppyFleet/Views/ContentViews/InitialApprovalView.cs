using System;
using mvvmframework.ViewModels;
using mvvmframework.Languages;
using Xamarin.Forms;
using NewAppyFleet.Converters;

namespace NewAppyFleet.Views.ContentViews
{
    public class InitialApprovalView
    {
        public static StackLayout InitialApproval(ContentView titleBar, InitialApprovalViewModel ViewModel)
        {
            ViewModel.OkToGo = false;

            var lblTitle = GetUIElement.GetFirstElement<Label>(titleBar.Content as StackLayout);
            lblTitle.Text = Langs.Const_Screen_Title_Terms;

            var masterGrid = new Grid
            {
                WidthRequest = App.ScreenSize.Width * .9,
                HeightRequest= App.ScreenSize.Height - 100,
                HorizontalOptions = LayoutOptions.Center
            };
            masterGrid.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition {Width = App.ScreenSize.Width * .9}
            };
            masterGrid.RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition {Height = App.ScreenSize.Height * .2},
                new RowDefinition {Height = GridLength.Auto},
                new RowDefinition {Height = GridLength.Auto},
            };

            var innerGrid = new Grid
            {
                RowSpacing = 12,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
            };
            innerGrid.RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition{Height = 48},
                new RowDefinition{Height = 48},
                new RowDefinition{Height = 48}
            };

            var width = App.ScreenSize.Width * .9;

            var chkTandC = new Image
            {
                WidthRequest = 32,
                HeightRequest = 32,
            };
            chkTandC.SetBinding(Image.SourceProperty, new Binding("OptOneTicked", converter: new OptToFilename()));
            chkTandC.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(() => ViewModel.OptOneTicked = !ViewModel.OptOneTicked)
            });

            var chkPrivacy = new Image
            {
                WidthRequest = 32,
                HeightRequest = 32,
            };
            chkPrivacy.SetBinding(Image.SourceProperty, new Binding("OptTwoTicked", converter: new OptToFilename()));
            chkPrivacy.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(() => ViewModel.OptTwoTicked = !ViewModel.OptTwoTicked)
            });

            var chkSpeed = new Image
            {
                WidthRequest = 32,
                HeightRequest = 32,
            };
            chkSpeed.SetBinding(Image.SourceProperty, new Binding("OptThreeTicked", converter: new OptToFilename()));
            chkSpeed.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(() => ViewModel.OptThreeTicked = !ViewModel.OptThreeTicked)
            });

            var lblTandC = new Label
            {
                Text = Langs.Const_Label_Terms_1,
                TextColor = Color.Blue,
                FontFamily = Helper.RegFont,
                VerticalTextAlignment = TextAlignment.Center
            };
            lblTandC.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(() => Device.OpenUri(new Uri(@"https://www.appyfleet.co.uk/wp-content/uploads/2017/05/Terms-and-Conditions-for-Supply-of-Appy-Fleet_Driver-Terms.pdf")))
            });

            var lblPrivacy = new Label
            {
                Text = Langs.Const_Label_Terms_2,
                TextColor = Color.Blue,
                FontFamily = Helper.RegFont,
                VerticalTextAlignment = TextAlignment.Center,
            };
            lblPrivacy.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(() => Device.OpenUri(new Uri(@"https://www.appyfleet.co.uk/wp-content/uploads/2017/05/Appy-Fleet-Cookie-Privacy-Policies.pdf")))
            });

            var lblSpeed = new Label
            {
                TextColor = Color.Blue,
                Text = Langs.Const_Label_Terms_3,
                FontFamily = Helper.RegFont,
                VerticalTextAlignment = TextAlignment.Center
            };
            lblSpeed.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(() => Device.OpenUri(new Uri(@"https://legal.here.com/terms/general-content-supplier/terms-and-notices/")))
            });

            var frameDriverTC = new Frame
            {
                BackgroundColor = FormsConstants.AppySilverGray,
                WidthRequest = App.ScreenSize.Width * .9,
                HeightRequest = 48,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(4),
                Content = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    VerticalOptions = LayoutOptions.Center,
                    Padding = new Thickness(4, 0),
                    Children =
                    {
                        new StackLayout
                        {
                            WidthRequest = 60,
                            HorizontalOptions = LayoutOptions.Start,
                            Children = {chkTandC}
                        }, lblTandC
                    }
                }
            };

            var framePrivacy = new Frame
            {
                BackgroundColor = FormsConstants.AppySilverGray,
                WidthRequest = App.ScreenSize.Width * .9,
                HeightRequest = 48,
                Padding = new Thickness(4),
                Content = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    VerticalOptions = LayoutOptions.Center,
                    Padding = new Thickness(4, 0),
                    Children =
                    {new StackLayout
                        {
                            WidthRequest = 60,
                            HorizontalOptions = LayoutOptions.Start,
                            Children = {chkPrivacy}
                        }, lblPrivacy
                    }
                }
            };

            var frameSpeed = new Frame
            {
                BackgroundColor = FormsConstants.AppySilverGray,
                WidthRequest = App.ScreenSize.Width * .9,
                HeightRequest = 48,
                Padding = new Thickness(4),
                Content = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    VerticalOptions = LayoutOptions.Center,
                    Padding = new Thickness(4, 0),
                    Children =
                    {new StackLayout
                        {
                            WidthRequest = 60,
                            HorizontalOptions = LayoutOptions.Start,
                            Children = {chkSpeed}
                        }, lblSpeed
                    }
                }
            };

            innerGrid.Children.Add(frameDriverTC, 0, 0);
            innerGrid.Children.Add(framePrivacy, 0, 1);
            innerGrid.Children.Add(frameSpeed, 0, 2);
            masterGrid.Children.Add(innerGrid, 0, 1);

            var btnAccept = new Button
            {
                WidthRequest = App.ScreenSize.Width * .9,
                Text = Langs.Const_Button_Accept,
                BorderRadius = 6,
                BackgroundColor = FormsConstants.AppyDarkBlue,
                TextColor = Color.White,
                FontFamily = Helper.BoldFont,
            };
            btnAccept.SetBinding(Button.IsEnabledProperty, new Binding("OkToGo"));
            btnAccept.Clicked += delegate { ViewModel.OkGo = true; };
            masterGrid.Children.Add(btnAccept, 0, 2);

            masterGrid.Children.Add(new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Padding = new Thickness(0, 12),
                Children =
                {
                    /*new Label
                    {
                        Text = Langs.Const_Screen_Title_Terms,
                        FontFamily = Helper.BoldFont,
                        TextColor = Color.White,
                        HorizontalTextAlignment = TextAlignment.Center
                    },*/
                    new Label
                    {
                        Text = Langs.Const_Msg_Terms_Description,
                        FontFamily = Helper.RegFont,
                        TextColor = Color.White,
                        HorizontalTextAlignment = TextAlignment.Center
                    }
                }
            }, 0, 0);

            return new StackLayout
            {
                TranslationX = -6,
                WidthRequest = App.ScreenSize.Width,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                Children = { masterGrid }
            };
        }
    }
}
