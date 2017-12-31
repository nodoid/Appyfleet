using System;
using System.Globalization;
using Xamarin.Forms;

namespace NewAppyFleet.Converters
{
    public class DiffScoreToColor: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (double)value;
            Color col;
            if (val < 0) col = FormsConstants.AppyLightRed;
            if (val == 0)
                col = FormsConstants.AppyYellow;
            else
                col = FormsConstants.AppyGreen;
            return col;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (Color)value;
            return val == FormsConstants.AppyGreen ? 1 : val == FormsConstants.AppyYellow ? 0 : -1;
        }
    }
}
