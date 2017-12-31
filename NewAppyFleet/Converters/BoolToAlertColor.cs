using System;
using System.Globalization;
using Xamarin.Forms;

namespace NewAppyFleet.Converters
{
    public class BoolToAlertColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (bool)value;
            return val ? FormsConstants.AppyDarkShade : FormsConstants.AppyRed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (Color)value;
            return val ==  FormsConstants.AppyDarkShade ? true : false;
        }
    }
}
