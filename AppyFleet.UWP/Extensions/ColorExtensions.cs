using System;

namespace AppyFleet.UWP.Extensions
{
    public static class ColorExtensions
    {
        public static Windows.UI.Color ToWindows(this Xamarin.Forms.Color color)
        {
            var a = color.A * 255;
            var r = color.R * 255;
            var g = color.G * 255;
            var b = color.B * 255;
            return Windows.UI.Color.FromArgb(Convert.ToByte(a), Convert.ToByte(r), Convert.ToByte(g), Convert.ToByte(b));
        }
    }
}
