using NewAppyFleet.CustomViews;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AppyFleet.UWP.UserControls
{
    public sealed partial class XamarinMapOverlay : UserControl
    {
        CustomPin customPin;

        public XamarinMapOverlay(CustomPin pin)
        {
            this.InitializeComponent();
            customPin = pin;
            SetupData();
        }

        void SetupData()
        {
            Label.Text = customPin.Pin.Label;
            Address.Text = customPin.Pin.Address;
        }

        private async void OnInfoButtonTapped(object sender, TappedRoutedEventArgs e)
        {
            
        }
    }
}
