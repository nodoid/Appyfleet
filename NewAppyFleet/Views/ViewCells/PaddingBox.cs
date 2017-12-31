using Xamarin.Forms;

namespace NewAppyFleet
{
    public class PaddingBox
    {
        public static BoxView CreatePaddingBox(double width)
        {
            return new BoxView { WidthRequest = width, MinimumWidthRequest = width, BackgroundColor = Color.Transparent };
        }
    }
}
