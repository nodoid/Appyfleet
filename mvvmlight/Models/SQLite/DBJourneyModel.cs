using System;
using SQLite.Net.Attributes;

namespace mvvmframework
{
    public class DBJourneyModel
    {
        [PrimaryKey]
        public long JourneyId { get; set; }

        public DateTime EndDate { get; set; }

        [Ignore]
        public string EndDateString => EndDate.ToString("dd MMM");
        [Ignore]
        public string EndDateYearString => EndDate.Year.ToString();
        [Ignore]
        public string EndTime => $"{EndDate.TimeOfDay.Hours}:{EndDate.TimeOfDay.Minutes}";

        public string JourneyType { get; set; }
        public string EndLocation { get; set; }
        public long Id { get; set; }
        public int JourneyNumber { get; set; }
        public double Miles { get; set; }
        public string Nickname { get; set; }
        public double OverallScore { get; set; }
        public int PolicyId { get; set; }
        public bool Private { get; set; }
        public double SmoothScore { get; set; }
        public double SpeedScore { get; set; }
        public DateTime StartDate { get; set; }

        [Ignore]
        public bool HasNotifications { get; set;}

        [Ignore]
        public string StartDateString => StartDate.ToString("dd MMM");
        [Ignore]
        public string StartDateYearString => StartDate.Year.ToString();
        [Ignore]
        public string StartTime => $"{StartDate.TimeOfDay.Hours}:{StartDate.TimeOfDay.Minutes}";
        public string StartLocation { get; set; }
        public double UsageScore { get; set; }
    }
}
