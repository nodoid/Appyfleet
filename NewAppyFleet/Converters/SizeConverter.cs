using System;
using Xamarin.Forms;
using System.Globalization;

namespace NewAppyFleet.Converters
{
    public class WidthSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var scale = (string)parameter;
            var size = System.Convert.ToDouble(scale);
            return (App.ScreenSize.Width * size).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HeightSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var scale = (string)parameter;
            var size = System.Convert.ToDouble(scale);
            return (App.ScreenSize.Height * size).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
