using System;
using System.Collections.Generic;
using Android.App;
using Android.Widget;
using mvvmframework;
using NewAppyFleet.CustomViews;
using NewAppyFleet.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomListView), typeof(CustomListViewRenderer))]
namespace NewAppyFleet.Droid.CustomRenderers
{
    public class CustomListViewRenderer : ViewRenderer<CustomListView, Android.Views.View>
    {
        string date, title;
        List<EventModel> events;
        long journeyId;

        protected override void OnElementChanged(ElementChangedEventArgs<CustomListView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                var dialog = e.NewElement;

                dialog.HorizontalOptions = LayoutOptions.Center;
                dialog.VerticalOptions = LayoutOptions.Center;

                date = dialog.EventDate;
                title = dialog.EventNumber;
                journeyId = dialog.EventJourneyId;
                events = dialog.EventsListSource;

                CreateDialog(dialog);
            }
        }

        public void CreateDialog(CustomListView dialog)
        {
            var dispModal = new Dialog(MainActivity.Active, Resource.Style.lightbox_dialog);
            dispModal.SetContentView(Resource.Layout.ModalView);

            // create the links to the UI elements
            ((Android.Widget.ImageView)dispModal.FindViewById(Resource.Id.imgClose)).Click += delegate
            {
                MessagingCenter.Send("modal", "closed");
                dispModal.Dismiss();
                dispModal.Dispose();
            };
            var txtDate = dispModal.FindViewById<TextView>(Resource.Id.txtDate);
            var txtWarnings = dispModal.FindViewById<TextView>(Resource.Id.txtNotice);

            txtDate.Text = date;
            txtWarnings.Text = title;

            var lstView = dispModal.FindViewById<Android.Widget.ListView>(Resource.Id.listView);
            lstView.Adapter = new ListViewData(MainActivity.Active, events);
            lstView.ItemClick += (sender, e) => 
            {
                var posn = e.Position;
                MessagingCenter.Send<string, int>("modal", "map", posn);
                dispModal.Dismiss();
                dispModal.Dispose();
            };

            // data is in, let's show the dialog box
            dispModal.Show();
        }
    }
}

