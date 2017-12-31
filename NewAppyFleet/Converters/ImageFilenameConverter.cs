using System;
using System.Globalization;
using Xamarin.Forms;

namespace NewAppyFleet.Converters
{
    public class ImageFilenameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var prop = (string)value;
            return prop.CorrectedImageSource();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
