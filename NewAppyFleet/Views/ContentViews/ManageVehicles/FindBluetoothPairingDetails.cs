using System;
using mvvmframework;
using mvvmframework.Languages;
using NewAppyFleet.UIHelpers;
using NewAppyFleet.Views.ListViewCells;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ContentViews.ManageVehicles
{
    public class FindBluetoothPairingDetails
    {
        static PairNewVehicleViewModel Vm { get; set; }
        static ListView lstDevices;

        static void RegisterEvents()
        {
            Vm.PropertyChanged += (sender, e) => 
            {
                if (e.PropertyName == "BluetoothDeviceList")
                {
                    if (lstDevices != null)
                    {
                        Device.BeginInvokeOnMainThread(()=>
                        {
                            lstDevices.ItemsSource = null;
                            lstDevices.ItemsSource = Vm.BluetoothDeviceList;
                        });
                    }
                }
            };
        }

        public static StackLayout FindBluetooth(ContentView titleBar, PairNewVehicleViewModel ViewModel)
        {
            Vm = ViewModel;
            RegisterEvents();

            ViewModel.GetBluetoothDeviceList();

            var lblTitle = GetUIElement.GetFirstElement<Label>(titleBar.Content as StackLayout);
            lblTitle.Text = Langs.Const_Screen_Title_Select_Bluetooth;

            lstDevices = new ListView
            {
                ItemsSource = ViewModel.BluetoothDeviceList,
                ItemTemplate = new DataTemplate(typeof(BluetoothViewCell)),
                IsPullToRefreshEnabled = true,
                SeparatorVisibility = SeparatorVisibility.None
            };
            lstDevices.ItemSelected += (sender, e) =>
            {
                var obj = e.SelectedItem as BluetoothDevice;
                ViewModel.SelectedBluetoothDevice = obj.Id;
                ViewModel.PopulateBasedOnId();
            };
            lstDevices.Refreshing += (sender, e) =>
            {
                ViewModel.GetBluetoothDeviceList();
            };

            var grid = new Grid
            {
                WidthRequest = App.ScreenSize.Width * .9
            };
            grid.RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition {Height = 80},
                new RowDefinition {Height = App.ScreenSize.Height * .6},
                new RowDefinition {Height = GridLength.Auto},
                new RowDefinition {Height = 200}
            };

            var arrowButton = ArrowBtn.ArrowButton(Langs.Const_Button_Manage_Vehicle_Step_3, App.ScreenSize.Width * .8, 
                                                   new Action(()=>{ if (ViewModel.SelectedVehicle.BluetoothId == ViewModel.SelectedBluetoothDevice.ToString()) ViewModel.MoveToSummary = true; }));

            arrowButton.SetBinding(Button.IsEnabledProperty, new Binding("Paired"));

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
                        var _ = new SpeechBubble(Langs.Const_Msg_Pair_Vehicle_Help_Description_3, App.ScreenSize.Width * .8,
                                                 FormsConstants.AppySilverGray);

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
                HeightRequest = 100,
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

            grid.Children.Add(new StackLayout
            {
                WidthRequest = App.ScreenSize.Width,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new StackLayout
                    {
                        Padding = new Thickness(0,12),
                        HorizontalOptions = LayoutOptions.Center,
                        Children =
                        {
                            new StackLayout
                            {
                                WidthRequest = App.ScreenSize.Width * .9,
                                HorizontalOptions = LayoutOptions.Center,
                                Children =
                                {
                                    new Label
                                    {
                                        FormattedText =FormattedProgress.GenerateProgress(true,true,false),
                                        FontFamily = Helper.BoldFont,
                                        TextColor = Color.White,
                                        HorizontalTextAlignment = TextAlignment.Center
                                    },
                                }
                            },
                            new BoxView {WidthRequest = App.ScreenSize.Width, HeightRequest = 1, BackgroundColor =Color.White},
                            new StackLayout
                            {
                                WidthRequest = App.ScreenSize.Width * .9,
                                HorizontalOptions = LayoutOptions.Center,
                                Children =
                                {
                                    new Label
                                    {
                                        Text = Langs.Const_Button_Pair_Vehicle,
                                        FontFamily = Helper.RegFont,
                                        TextColor = Color.White,
                                        HorizontalOptions = LayoutOptions.Center
                                    },
                                }
                            },
                        }

                    },
                }
            }, 0, 0);

            grid.Children.Add(lstDevices, 0, 1);
            grid.Children.Add(arrowButton, 0, 2);
            grid.Children.Add(helpContainer, 0, 3);

            return new StackLayout
            {
                WidthRequest = App.ScreenSize.Width,
                HorizontalOptions = LayoutOptions.Center,
                TranslationX = -6,
                Padding = new Thickness(4),
                Children = { grid }
            };
        }
    }
}
