using System;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Util;
using NewAppyFleet;
using NewAppyFleet.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(RoundedLabel), typeof(RoundedLabelRenderer))]
namespace NewAppyFleet.Droid
{
    public class RoundedLabelRenderer : LabelRenderer
    {
        private GradientDrawable gradientBackground;

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            var view = (RoundedLabel)Element;
            if (view == null) return;

            // creating gradient drawable for the curved background
            gradientBackground = new GradientDrawable();
            gradientBackground.SetSize((int)view.WidthRequest, (int)view.Height);
            gradientBackground.SetShape(ShapeType.Rectangle);
            gradientBackground.SetColor(view.RoundedBackgroundColor.ToAndroid());

            // Thickness of the stroke line
            gradientBackground.SetStroke(4, view.RoundedBackgroundColor.ToAndroid());

            // Radius for the curves
            gradientBackground.SetCornerRadius(DpToPixels(MainActivity.Active, Convert.ToSingle(view.RoundedCornerRadius)));

            Control.SetPadding((int)view.InsidePadding.Left, (int)view.InsidePadding.Top, (int)view.InsidePadding.Right, (int)view.InsidePadding.Bottom);

            // set the background of the label
            Control.SetBackground(gradientBackground);
        }

        public static float DpToPixels(Context context, float valueInDp)
        {
            var metrics = context.Resources.DisplayMetrics;
            return TypedValue.ApplyDimension(ComplexUnitType.Dip, valueInDp, metrics);
        }
    }
}
