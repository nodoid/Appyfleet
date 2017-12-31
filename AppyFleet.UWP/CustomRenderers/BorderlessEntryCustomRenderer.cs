using AppyFleet.UWP.CustomRenderers;
using NewAppyFleet.CustomViews;
using Windows.System;
using Windows.UI.Xaml.Input;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryCustomRenderer))]
namespace AppyFleet.UWP.CustomRenderers
{
    public class BorderlessEntryCustomRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            var entry = (BorderlessEntry)Element;

            if (e.NewElement != null)
            {
                if (Control != null)
                {
                    Control.BorderThickness = new Windows.UI.Xaml.Thickness(0);

                    if (entry != null)
                    {
                        Control.InputScope = entry.Keyboard.ToInputScope(); 
                        Control.KeyDown += (object sender, KeyRoutedEventArgs eventArgs) =>
                        {
                            if (eventArgs.Key == VirtualKey.Enter)
                            {
                                entry.InvokeCompleted();
                                eventArgs.Handled = true;
                            }
                        };
                    }
                }
            }
        }
    }
}
