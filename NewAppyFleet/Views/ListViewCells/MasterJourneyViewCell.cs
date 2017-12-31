using mvvmframework.Models;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ListViewCells
{
    public class MasterJourneyViewCell : ViewCell
    {
        public MasterJourneyViewCell()
        {
            var lblDate = new Label
            {
                FontFamily = Helper.BoldFont,
                FontSize = 18,
                TextColor = Color.White
            };
            lblDate.SetBinding(Label.TextProperty, new Binding("JourneyDateTime"));
        
            View = new StackLayout
            {
                WidthRequest = App.ScreenSize.Width,
                HorizontalOptions = LayoutOptions.Center,
                Children = { lblDate }
            };
        }
    }
}
