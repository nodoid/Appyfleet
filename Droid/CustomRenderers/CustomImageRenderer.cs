using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Graphics;
using System.ComponentModel;
using System.Linq;
using Android.Graphics.Drawables;
using NewAppyFleet.Droid;
using NewAppyFleet.CustomViews;

[assembly: ExportRenderer(typeof(CustomImage), typeof(CustomImageRenderer))]
namespace NewAppyFleet.Droid
{
    public class CustomImageRenderer : ImageRenderer
    {
        GradientDrawable shape;

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                var img = e.NewElement as CustomImage;
            }
        }

        private bool isDecoded;

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var largeImage = (CustomImage)Element;

            if (e.PropertyName == "BorderThickness" || e.PropertyName == "BorderColor")
            {
                shape.SetStroke((int)largeImage.BorderThickness, largeImage.BorderColor.ToAndroid());
            }

            if (largeImage.DontResize)
            {
                if ((Element.Width > 0 && Element.Height > 0 && !isDecoded) || (e.PropertyName == "ImageSource" && largeImage.ImageSource != null))
                {
                    BitmapFactory.Options options = new BitmapFactory.Options();
                    options.InJustDecodeBounds = true;

                    //Get the resource id for the image
                    if (largeImage.ImageSource != null)
                    {
                        var field = typeof(Resource.Drawable).GetField(largeImage.ImageSource.Split('.').First());
                        var value = (int)field.GetRawConstantValue();

                        BitmapFactory.DecodeResource(Context.Resources, value, options);

                        //The with and height of the elements (LargeImage) will be used to decode the image
                        var width = (int)Element.Width;
                        var height = (int)Element.Height;
                        options.InSampleSize = CalculateInSampleSize(options, width, height);

                        options.InJustDecodeBounds = false;
                        var bitmap = BitmapFactory.DecodeResource(Context.Resources, value, options);

                        //Set the bitmap to the native control
                        Control.SetImageBitmap(bitmap);

                        isDecoded = true;
                    }
                }
            }
        }

        public int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Raw height and width of image
            float height = options.OutHeight;
            float width = options.OutWidth;
            double inSampleSize = 1D;

            if (height > reqHeight || width > reqWidth)
            {
                int halfHeight = (int)(height / 2);
                int halfWidth = (int)(width / 2);

                // Calculate a inSampleSize that is a power of 2 - the decoder will use a value that is a power of two anyway.
                while ((halfHeight / inSampleSize) > reqHeight && (halfWidth / inSampleSize) > reqWidth)
                {
                    inSampleSize *= 2;
                }
            }

            return (int)inSampleSize;
        }
    }
}

