using System;
using Xamarin.Forms;
using System.Globalization;
using mvvmframework.Languages;

namespace NewAppyFleet.Converters
{
    public class LanguageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var prop = (string)parameter;
            if (string.IsNullOrEmpty(prop))
                return string.Empty;
            return Langs.ResourceManager.GetString(prop);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
