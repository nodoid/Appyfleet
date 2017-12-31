using System;
using System.Globalization;
using Xamarin.Forms;

namespace NewAppyFleet.Converters
{
    public class NotificationFilterToBGColor: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Color.White : Color.Transparent; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
