using System;
using mvvmframework;
using mvvmframework.Languages;
using NewAppyFleet.UIHelpers;
using NewAppyFleet.Views.ListViewCells;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ContentViews.ManageVehicles
{
    public class SearchSelectAddDetails
    {
        static PairNewVehicleViewModel Vm { get; set; }
        static ListView listView;

        static void RegisterEvents()
        {
            Vm.PropertyChanged += (sender, e) => 
            {
                if (e.PropertyName == "RegisteredVehicles")
                {
                    if (listView != null)
                    {
                        Device.BeginInvokeOnMainThread(()=>
                        {
                            listView.ItemsSource = null;
                            listView.ItemsSource = Vm.RegisteredVehicles?.Vehicles;
                        });
                    }
                }
            };
        }

        public static StackLayout SearchSelectAddVehicle(ContentView titleBar, PairNewVehicleViewModel ViewModel)
        {
            Vm = ViewModel;

            RegisterEvents();

            if (ViewModel.VehicleModels == null)
                ViewModel.MoveToAdd = true;
            else
            if (ViewModel.VehicleModels?.Count == 0)
            {
                ViewModel.GetVehiclesFromDb();
                if (ViewModel.VehicleModels.Count == 0)
                    ViewModel.MoveToAdd = true;
            }

            var lblTitle = GetUIElement.GetFirstElement<Label>(titleBar.Content as StackLayout);
            lblTitle.Text = Langs.Const_Screen_Title_Select_Vehicle;

            var srchVehicle = new SearchBar
            {
                WidthRequest = App.ScreenSize.Width * .9,
            };
            srchVehicle.SetBinding(SearchBar.TextProperty, new Binding("VehicleSearch"));

            var lblNoneFound = new Label
            {
                Text = Langs.Const_Label_No_Search_Result,
                TextColor = Color.White,
                FontFamily = Helper.BoldFont,
                IsVisible = false
            };

            srchVehicle.SearchButtonPressed += (sender, e) => 
            {
                var _ = ViewModel.SearchBasedOnVehicle();
                if (!_)
                    lblNoneFound.IsVisible = true;
            };

            var grid = new Grid
            {
                WidthRequest = App.ScreenSize.Width
            };
            grid.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition {Width = App.ScreenSize.Width}
            };
            grid.RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition {Height = GridLength.Auto},
                new RowDefinition {Height = App.ScreenSize.Height * .5},
                new RowDefinition {Height = GridLength.Auto}
            };

            var activitySpinner = new ActivityIndicator
            {
                BackgroundColor = Color.Transparent,
                WidthRequest = 40,
                IsRunning = true
            };
            activitySpinner.SetBinding(ActivityIndicator.IsVisibleProperty, new Binding("IsBusy"));

            listView = new ListView(ListViewCachingStrategy.RecycleElement)
            {
                IsPullToRefreshEnabled = true,
                ItemsSource = ViewModel.VehicleModels,
                ItemTemplate = new DataTemplate(typeof(RegisteredVehicleViewCell)),
                SeparatorVisibility = SeparatorVisibility.None,
                HasUnevenRows = true
                                                         
            };
            listView.ItemSelected += (sender, e) => 
            {
                var obj = e.SelectedItem as VehicleModel;
                ViewModel.SelectedVehicle = obj;
            };
            listView.Refreshing += (sender, e) => 
            {
                listView.IsRefreshing = true;
                ViewModel.CmdGetRegisteredVehicles.Execute(null);
                listView.IsRefreshing = false;
            };

            var dummyCell = new StackLayout
            {
                Padding = new Thickness(12),
                BackgroundColor = FormsConstants.AppyDarkShade,
                VerticalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                Children =
                        {
                            new Image
                            {
                                Source = "add_new".CorrectedImageSource(),
                                HeightRequest = 48
                            },
                            new Label
                            {
                                Text = Langs.Const_Label_Add_Vehicle,
                                FontFamily = Helper.RegFont,
                                TextColor = Color.White,
                                HeightRequest = 48,
                                VerticalTextAlignment = TextAlignment.Center
                            }
                        }
            };
var dummyListCellRecogniser = new TapGestureRecognizer
{
    NumberOfTapsRequired = 1,
                Command = new Command(() => ViewModel.MoveToAdd = !ViewModel.MoveToAdd)
};
            dummyCell.GestureRecognizers.Add(dummyListCellRecogniser);
            var dummyListCell = new StackLayout
            {
                HeightRequest = 60,
                InputTransparent = true,
                Padding = new Thickness(8),
                Children =
                {
                    dummyCell
                }
            };


            var listStack = new StackLayout
            {
                WidthRequest = App.ScreenSize.Width * .9,
                Padding = new Thickness(4),
                Children = {lblNoneFound, activitySpinner, listView, dummyListCell}
            };

            grid.Children.Add(new StackLayout
            {
                WidthRequest = App.ScreenSize.Width,
                HorizontalOptions= LayoutOptions.Center,
                Children =
                {
                    new StackLayout
                    {
                        WidthRequest = App.ScreenSize.Width * .9,
                        HorizontalOptions = LayoutOptions.Center,
                        Children = 
                        {
                            new Label {FormattedText = FormattedProgress.GenerateProgress(true, false, false), HorizontalTextAlignment = TextAlignment.Center},
                    srchVehicle
                        }
                    },
                    new BoxView{WidthRequest = App.ScreenSize.Width, HeightRequest = 1, BackgroundColor = Color.White},
                }
            }, 0,0);

            var arrowButton = ArrowBtn.ArrowButton(Langs.Const_Button_Manage_Vehicle_Step_1, App.ScreenSize.Width * .9,
                                                   new Action(() => { if (ViewModel.RegisteredVehicles?.Vehicles.Count != 0) ViewModel.MoveToPair = true; }));

            grid.Children.Add(new StackLayout
            {
                WidthRequest = App.ScreenSize.Width,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new StackLayout
                    {
                        WidthRequest = App.ScreenSize.Width * .9,
                        HorizontalOptions = LayoutOptions.Center,
                        Children = {listStack}
                    },
                    new BoxView{WidthRequest = App.ScreenSize.Width, HeightRequest = 1, BackgroundColor = Color.White},
                    arrowButton
                }
            }, 0, 1);

           

            var inStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                HeightRequest = 100,
                Children = { new Label { Text = " " } }
            };

            var imgHelp = new Image
            {
                Source = "help".CorrectedImageSource(),
                HeightRequest = 32
            };
            var imgHelpGesture = new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(() =>
                {
                    var src = imgHelp.Source as FileImageSource;

                    if (src.File == "help".CorrectedImageSource())
                    {
                        var _ = new SpeechBubble(Langs.Const_Msg_Pair_Vehicle_Help_Description_1, App.ScreenSize.Width * .8, FormsConstants.AppySilverGray);

                        if (inStack.Children.Count > 0)
                            inStack.Children.RemoveAt(0);
                        inStack.Children.Add(_);
                        imgHelp.Source = "help_close".CorrectedImageSource();
                    }
                    else
                    {
                        imgHelp.Source = "help".CorrectedImageSource();
                        inStack.Children.RemoveAt(0);
                    }
                })
            };
            imgHelp.GestureRecognizers.Add(imgHelpGesture);

            var helpContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                WidthRequest = App.ScreenSize.Width,

                HorizontalOptions = LayoutOptions.Start,
                Children =
                {
                    inStack,
                    new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.End,
                        HeightRequest = 64,
                        VerticalOptions = LayoutOptions.Start,
                        Children = {imgHelp}
                    }
                }
            };

            grid.Children.Add(helpContainer, 0, 2);

            return new StackLayout
            {
                WidthRequest = App.ScreenSize.Width,
                TranslationX = -6,
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = App.ScreenSize.Height - 100,
                Children = {grid}
            };
        }

    }
}
