using mvvmframework;
using NewAppyFleet.CustomViews;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ViewCells
{
    public class UniversalEntry
    {
        public static BorderlessEntry GeneralEntryCell(string text, double width, Keyboard keyboard, string placeholder = "", ReturnKeyTypes returnKey = ReturnKeyTypes.Done, double factor = .6, bool useBold = false, bool isPassword = false)
        {
            return new BorderlessEntry
            {
                WidthRequest = width * factor,
                BackgroundColor = Color.Transparent,
                Text = text,
                Keyboard = keyboard,
                TextColor = Color.White,
                Placeholder = placeholder,
                PlaceholderColor = Color.White,
                FontFamily = useBold ? Helper.BoldFont : Helper.RegFont,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Center,
                ReturnType = returnKey,
                IsPassword = isPassword
            };
        }
    }
}
