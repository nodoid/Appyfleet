using System;
using System.Globalization;
using Xamarin.Forms;

namespace NewAppyFleet.Converters
{
    public class JourneyScoreToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (double)value;
            return val < 0 ? FormsConstants.AppyLightRed : FormsConstants.AppyDarkShade;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (Color)value;
            return val == FormsConstants.AppyLightRed ? -1 : 1;
        }
    }
}
