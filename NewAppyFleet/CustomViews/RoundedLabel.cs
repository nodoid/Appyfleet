using Xamarin.Forms;
namespace NewAppyFleet
{
    public class RoundedLabel : Label
    {
        public static readonly BindableProperty RoundedCornerRadiusProperty = 
            BindableProperty.Create(nameof(RoundedCornerRadius), typeof(double), typeof(RoundedLabel), 12.0);
        public double RoundedCornerRadius
        {
            get { return (double)GetValue(RoundedCornerRadiusProperty); }
            set { SetValue(RoundedCornerRadiusProperty, value); }
        }

        public static readonly BindableProperty RoundedBackgroundColorProperty = BindableProperty.Create(nameof(RoundedBackgroundColor), typeof(Color), typeof(RoundedLabel), Color.Default);
        public Color RoundedBackgroundColor
        {
            get { return (Color)GetValue(RoundedBackgroundColorProperty); }
            set { SetValue(RoundedBackgroundColorProperty, value); }
        }

        public static readonly BindableProperty InsidePaddingProperty = BindableProperty.Create(nameof(InsidePadding), typeof(Thickness), typeof(RoundedLabel), new Thickness(0, 0, 0, 0));
        public Thickness InsidePadding
        {
            get { return (Thickness)GetValue(InsidePaddingProperty); }
            set { SetValue(InsidePaddingProperty, value); }
        }
    }
}
