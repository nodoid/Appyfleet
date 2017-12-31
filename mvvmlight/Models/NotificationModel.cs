using System;
using System.Collections.Generic;
using System.Globalization;

namespace mvvmframework
{
    public class NotificationModel
    {
        public NotificationModel(CultureInfo cult)
        {
            Culture = cult;
        }

        CultureInfo Culture { get; set; }

        public long JourneyId { get; set; }
        public int Id { get; set; }
        public DateTime JourneyDate { get; set; }
        public bool Read { get; set; }

        public string Date => JourneyDate.ToLocalizedString("dd MMM yyyy HH:mmtt", Culture);
        public string DateString => JourneyDate.ToString("dd.MMM.yyyy HH:mm");

        public List<EventModel> Events { get; set; }

        public int EventCount => Events.Count;
    }
}
