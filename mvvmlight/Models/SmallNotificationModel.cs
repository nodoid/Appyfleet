using System;
namespace mvvmframework.Models
{
    public class SmallNotificationModel
    {
        public SmallNotificationModel(int count)
        {
            EventCount = count;    
        }

        public long JourneyId { get; set; }
        public int Id { get; set; }
        public DateTime JourneyDate { get; set; }
        public bool Read { get; set; }

        public string DateString => JourneyDate.ToString("dd.MMM.yyyy HH:mm");

        public int EventCount { get; set; }
    }
}
