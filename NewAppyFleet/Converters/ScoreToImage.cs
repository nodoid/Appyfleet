using System;
using System.Globalization;
using Xamarin.Forms;

namespace NewAppyFleet.Converters
{
    public class ScoreToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (double)value;
            return val > 0 ? "positive_needle".CorrectedImageSource() : "negative_needle".CorrectedImageSource();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (string)value;
            return val.Contains("pos") ? 1 : 0;
        }
    }
}