using System;
using System.Globalization;
using Xamarin.Forms;

namespace NewAppyFleet.Converters
{
    public class BoolToMarcketImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (bool)value;
            return val ? "main_menu_notifications".CorrectedImageSource() : "red_circle_selected".CorrectedImageSource();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (string)value;
            return val.Contains("notifications") ? true : false;
        }
    }
}
