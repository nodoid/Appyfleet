using Xamarin.Forms;

namespace NewAppyFleet
{
    public static class CorrectedSource
    {
        public static string CorrectedImageSource(this string filename)
        {
            var fn = filename;
            if (Device.RuntimePlatform == Device.Windows || Device.RuntimePlatform == Device.WinPhone)
                fn = string.Format("Images/{0}.png", fn);
            return fn;
        }

        public static string CorrectedFontSource(this string filename)
        {
            if (Device.RuntimePlatform == Device.Windows || Device.RuntimePlatform == Device.WinPhone)
                filename = $"Assets/Fonts/{filename}";
            return filename;
        }
    }
}
