using System;
using System.Globalization;
using Xamarin.Forms;

namespace NewAppyFleet.Converters
{
    public class ScoreToColor: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (double)value;
            Color col;
            if (val < 0) col = FormsConstants.AppyLightRed;
            else
            {
                if (val >= 0 && val < 5)
                    col = FormsConstants.AppyYellow;
                else
                    col = FormsConstants.AppyGreen;
            }
            return col;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (Color)value;
            return val == FormsConstants.AppyGreen ? 5 : val == FormsConstants.AppyYellow ? 4 : -1;
        }
    }
}
