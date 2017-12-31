using System;
using System.Globalization;

namespace mvvmframework
{
    public class JourneyModel
    {
        CultureInfo Culture { get; set; }
        public JourneyModel(CultureInfo info)
        {
            Culture = info;
        }

        public DateTime EndDate { get; set; }
        public string EndLocation { get; set; }
        public long Id { get; set; }
        public long JourneyId { get; set; }
        public int JourneyNumber { get; set; }
        public double Miles { get; set; }
        public string Nickname { get; set; }
        public double OverallScore { get; set; }
        public int PolicyId { get; set; }
        public bool Private { get; set; }
        public double SmoothScore { get; set; }
        public double SpeedScore { get; set; }
        public DateTime StartDate { get; set; }
        public string StartLocation { get; set; }
        public double UsageScore { get; set; }

        public string StartDesc => StartDate.ToLocalizedString("dd MMM", Culture);

        public string EndDesc => EndDate.ToLocalizedString("dd MMM", Culture);
    }
}
