using Xamarin.Forms;

namespace NewAppyFleet.UIHelpers
{
    public class FormattedProgress
    {
        public static FormattedString GenerateProgress(params bool[] makeBold)
        {
            var fs = new FormattedString();
            fs.Spans.Add(new Span { Text = "1", FontSize = 14, FontFamily = makeBold[0] ? Helper.BoldFont : Helper.RegFont, ForegroundColor = Color.White });
            fs.Spans.Add(new Span { Text = " - ", FontSize = 14, FontFamily = makeBold[1] ? Helper.BoldFont : Helper.RegFont, ForegroundColor = Color.White });
            fs.Spans.Add(new Span { Text = "2", FontSize = 14, FontFamily = makeBold[1] ? Helper.BoldFont : Helper.RegFont, ForegroundColor = Color.White });
            fs.Spans.Add(new Span { Text = " - ", FontSize = 14, FontFamily = makeBold[2] ? Helper.BoldFont : Helper.RegFont, ForegroundColor = Color.White });
            fs.Spans.Add(new Span { Text = "3", FontSize = 14, FontFamily = makeBold[2] ? Helper.BoldFont : Helper.RegFont, ForegroundColor = Color.White });

            return fs;
        }
    }
}
