using System.Collections.Generic;

using Android.App;
using Android.Views;
using Android.Widget;
using mvvmframework;

namespace NewAppyFleet.Droid
{
    public class ListViewData : BaseAdapter<EventModel>
    {
        List<EventModel> notifications;
        Activity context;

        public ListViewData(Activity ctx, List<EventModel> items) : base()
        {
            context = ctx;
            notifications = items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override EventModel this[int position] => notifications[position];

        public override int Count => notifications.Count;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = notifications[position];
            var view = convertView;
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.ModalListViewCell, null);
            
            view.FindViewById<TextView>(Resource.Id.txtEvent).Text = item.EventType;
            view.FindViewById<TextView>(Resource.Id.txtTime).Text = item.DateCreated.TimeOfDay.ToString("g");
            view.FindViewById<TextView>(Resource.Id.txtWrong).Text = item.SpeedInfo;
            return view;
        }
    }
}
