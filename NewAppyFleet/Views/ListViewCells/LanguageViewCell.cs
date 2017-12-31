using System;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ListViewCells
{
    public class LanguageViewCell : ViewCell
    {
        public LanguageViewCell()
        {
            var lblLanguage = new Label
            {
                TextColor = Color.White,
                FontFamily = Helper.RegFont
            };
            lblLanguage.SetBinding(Label.TextProperty, new Binding("Name"));

            View = new StackLayout
            {
                Padding = new Thickness(8, 8),
                Children = { lblLanguage }
            };
        }
    }
}
