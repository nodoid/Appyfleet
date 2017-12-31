using System.Collections.Generic;

namespace mvvmframework
{
    public class JourneyListModel
    {
        public JourneyListModel()
        {
            Status = new StatusModel();
            Journeys = new List<JourneyModel>();
        }

        public List<JourneyModel> Journeys { get; set; }
        public StatusModel Status { get; set; }
    }
}
