using Xamarin.Forms;

namespace NewAppyFleet.CustomViews
{
    public class MyRect
    {
        public MyRect(double left = 0, double right = 0, double top = 0, double bottom = 0)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        public MyRect(double lr = 0, double tb = 0)
        {
            Left = Right = lr;
            Top = Bottom = tb;
        }

        public MyRect(double all = 0)
        {
            Top = Bottom = Left = Right = all;
        }

        public double Top { get; private set; }

        public double Bottom { get; private set; }

        public double Left { get; private set; }

        public double Right { get; private set; }
    }

    public class CustomImage : Image
    {
        public static readonly BindableProperty DontResizeProperty = 
            BindableProperty.Create(nameof(DontResize), typeof(bool), typeof(CustomImage), true);

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(CustomImage), default(Color));

        public static readonly BindableProperty ResizeProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(CustomImage), default(Color));

        public static readonly BindableProperty BorderRectangleProperty =
            BindableProperty.Create(nameof(BorderRectangle), typeof(MyRect), typeof(CustomImage), default(MyRect));

        public static readonly BindableProperty BorderThicknessProperty =
            BindableProperty.Create(nameof(BorderThickness), typeof(int), typeof(CustomImage), 0);

        public static readonly BindableProperty ImageSourceProperty =
            BindableProperty.Create(nameof(ImageSource), typeof(string), typeof(CustomImage), default(string)
            , propertyChanged: (bindable, oldValue, newValue) =>
            {
            if (Device.RuntimePlatform != Device.Android)
                {
                    var image = (CustomImage)bindable;

                    var baseImage = (Image)bindable;
                    baseImage.Source = image.ImageSource;
                }
            });

        public bool DontResize
        {
            get { return (bool)GetValue(DontResizeProperty); }
            set { SetValue(DontResizeProperty, value); }
        }

        public string ImageSource
        {
            get { return GetValue(ImageSourceProperty) as string; }
            set { SetValue(ImageSourceProperty, value); }
        }

        public MyRect BorderRectangle
        {
            get { return GetValue(BorderRectangleProperty) as MyRect; }
            set { SetValue(BorderRectangleProperty, value); }
        }

        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public int BorderThickness
        {
            get { return (int)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }
    }
}
