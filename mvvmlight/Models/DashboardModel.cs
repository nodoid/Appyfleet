using System.Collections.ObjectModel;

namespace mvvmframework.Models
{
    public class DashboardListModel
    {
        public double RotateAngle { get; set; }
        public double TotalMiles { get; set; }
        public double BusinessMiles { get; set; }
        public double PersonalMiles { get; set; }
        public DBJourneyModel LatestJourney { get; set; }
        public int NotificationCount { get; set; }
        public ObservableCollection<Rankings> Rankings { get; set; }
        public string Registration { get; set; }
        public string Odo { get; set; }
        public double Score { get; set; }
    }
}
