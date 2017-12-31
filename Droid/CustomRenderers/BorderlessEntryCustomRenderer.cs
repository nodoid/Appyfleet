using System.ComponentModel;
using Android.Views.InputMethods;
using Android.Widget;
using mvvmframework;
using NewAppyFleet.CustomViews;
using NewAppyFleet.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryCustomRenderer))]
namespace NewAppyFleet.Droid.CustomRenderers
{
    public class BorderlessEntryCustomRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var entry = (BorderlessEntry)Element;
                if (Control != null)
                {
                    Control.Background = Resources.GetDrawable(Resource.Drawable.CustomEntryDrawable);

                    if (entry != null)
                    {
                        SetReturnType(entry);
                        Control.SetRawInputType(entry.Keyboard.ToInputType());
                        Control.EditorAction += (object sender, TextView.EditorActionEventArgs args) =>
                        {
                            if (entry.ReturnType != ReturnKeyTypes.Next)
                                entry.Unfocus();

                            entry.InvokeCompleted();
                        };

                    }
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == BorderlessEntry.ReturnKeyPropertyName)
            {
                var entryExt = (sender as BorderlessEntry);
                Control.ImeOptions = entryExt.ReturnType.GetValueFromDescription();
                Control.SetImeActionLabel(entryExt.ReturnType.ToString(), Control.ImeOptions);
            }
        }

        void SetReturnType(BorderlessEntry entry)
        {
            var type = entry.ReturnType;

            switch (type)
            {
                case ReturnKeyTypes.Go:
                    Control.ImeOptions = ImeAction.Go;
                    Control.SetImeActionLabel("Go", ImeAction.Go);
                    break;
                case ReturnKeyTypes.Next:
                    Control.ImeOptions = ImeAction.Next;
                    Control.SetImeActionLabel("Next", ImeAction.Next);
                    break;
                case ReturnKeyTypes.Send:
                    Control.ImeOptions = ImeAction.Send;
                    Control.SetImeActionLabel("Send", ImeAction.Send);
                    break;
                case ReturnKeyTypes.Search:
                    Control.ImeOptions = ImeAction.Search;
                    Control.SetImeActionLabel("Search", ImeAction.Search);
                    break;
                default:
                    Control.ImeOptions = ImeAction.Done;
                    Control.SetImeActionLabel("Done", ImeAction.Done);
                    break;
            }

        }
    }
}