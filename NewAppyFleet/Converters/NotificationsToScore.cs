using System;
using System.Globalization;
using Xamarin.Forms;

namespace NewAppyFleet.Converters
{
    public class NotificationsToScore : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (int)value;
            return val <= 10 ? FormsConstants.AppyDarkRed : FormsConstants.AppyDarkShade;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (Color)value;
            return val == FormsConstants.AppyDarkRed ? 9 : 11;
        }
    }
}
