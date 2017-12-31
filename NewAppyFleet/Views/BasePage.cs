using System;

using Xamarin.Forms;

namespace NewAppyFleet
{
    public class BasePage : ContentPage
    {
        public RelativeLayout RelativeLayout { get; set; }
        public Image imgStream { get; set; }

        public BasePage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            if (Device.RuntimePlatform == Device.iOS)
                Padding = new Thickness(0, 20, 0, 0);

            CreateUI();
        }

        void CreateUI()
        {
            var imgBackground = new Image
            {
                Source = "blue_background".CorrectedImageSource(),
                Aspect = Aspect.Fill,
            };

            RelativeLayout = new RelativeLayout();
            RelativeLayout.Children.Add(imgBackground,
                                                        Constraint.Constant(0),
                                                        Constraint.Constant(0),
                                                        Constraint.RelativeToParent((parent) => App.ScreenSize.Width),
                                                        Constraint.RelativeToParent((parent) => App.ScreenSize.Height));
        }
    }
}

