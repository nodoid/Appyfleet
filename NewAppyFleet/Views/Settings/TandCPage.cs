using System;

using Xamarin.Forms;

namespace NewAppyFleet
{
    public class TandCPage : BasePage
    {
        public TandCPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

