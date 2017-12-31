using System;
using mvvmframework.Languages;

namespace mvvmframework.Models
{
    public class ExpenseModel
    {
        public long JourneyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double OverallScore { get; set; }
        public double SmoothScore { get; set; }
        public double SpeedScore { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
        public bool Private { get; set; }
        public double Miles { get; set; }
        public bool Selected { get; set; } = false;
        public bool HasNotifications { get; set; } = false;

        public string EndDateString => EndDate.ToString("dd MMM");
        public string EndDateYearString => EndDate.Year.ToString();
        public string EndTime => $"{EndDate.TimeOfDay.Hours}:{EndDate.TimeOfDay.Minutes}";
        public string StartDateString => StartDate.ToString("dd MMM");
        public string StartDateYearString => StartDate.Year.ToString();
        public string StartTime => $"{StartDate.TimeOfDay.Hours}:{StartDate.TimeOfDay.Minutes}";
        public string JourneyType => Private ? Langs.Const_Label_Private : Langs.Const_Label_Business;

        public string ImageName { get; set; } = "switch_button_off";
        public string SelectedText { get; set; } = Langs.Const_Label_Select;
    }
}