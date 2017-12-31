using System;
using System.Globalization;
using Xamarin.Forms;

namespace NewAppyFleet.Converters
{
    public class OptToFilename : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (bool)value;
            return !val ? "about".CorrectedImageSource() : "accept_icon".CorrectedImageSource();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (string)value;
            return val.Contains("about") ? false : true;
        }
    }
}
