using AppyFleet.UWP.CustomRenderers;
using AppyFleet.UWP.Extensions;
using AppyFleet.UWP.UserControls;
using NewAppyFleet;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(RoundedLabel), typeof(CustomNewLabelRenderer))]
namespace AppyFleet.UWP.CustomRenderers
{
    internal class CustomNewLabelRenderer : ViewRenderer<RoundedLabel, CustomLabelControl>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<RoundedLabel> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control == null)
                    SetNativeControl(new CustomLabelControl());

                Control.Text = Element.Text;
                Control.LabelRadius = new CornerRadius(Element.RoundedCornerRadius);

                var color = Element.RoundedBackgroundColor;
                Control.LabelBackground = new SolidColorBrush(color.ToWindows());
                Control.Width = Element.WidthRequest;
            }
        }
    }
}
