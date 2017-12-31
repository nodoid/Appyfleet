using System;
using System.Collections.Generic;
using mvvmframework;
using Xamarin.Forms;

namespace NewAppyFleet.CustomViews
{
    public class CustomListView : ContentView
    {
        public static readonly BindableProperty EventsListSourceProperty =
            BindableProperty.Create(nameof(EventsListSource), typeof(List<EventModel>), typeof(CustomListView), null);
        public List<EventModel> EventsListSource
        {
            get { return (List<EventModel>)GetValue(EventsListSourceProperty); }
            set { SetValue(EventsListSourceProperty, value); }
        }

        public static readonly BindableProperty EventDateProperty =
            BindableProperty.Create(nameof(EventDate), typeof(string), typeof(CustomListView), string.Empty);
        public string EventDate
        {
            get { return (string)GetValue(EventDateProperty); }
            set { SetValue(EventDateProperty, value); }
        }

        public static readonly BindableProperty EventNumberProperty =
            BindableProperty.Create(nameof(EventNumber), typeof(string), typeof(CustomListView), string.Empty);
        public string EventNumber
        {
            get { return (string)GetValue(EventNumberProperty); }
            set { SetValue(EventNumberProperty, value); }
        }

        public static readonly BindableProperty EventJourneyIdProperty =
            BindableProperty.Create(nameof(EventJourneyId), typeof(long), typeof(CustomListView), 0L);
        public long EventJourneyId
        {
            get { return (long)GetValue(EventJourneyIdProperty); }
            set { SetValue(EventJourneyIdProperty, value); }
        }
    }
}

