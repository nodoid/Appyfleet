using System;
using System.Collections.Generic;

namespace mvvmframework
{
    public class JourneyDetailsOld
    {
        public List<LocationDetails> Journey { get; set; }
        public DateTime StartTime { get; set; }
        public int JourneyNumber { get; set; }

        public JourneyDetailsOld()
        {
            Journey = new List<LocationDetails>();
        }

        public JourneyDetailsOld(List<LocationDetails> locations, int journeyNumber, DateTime startTime)
        {
            Journey = locations;
            JourneyNumber = journeyNumber;
            StartTime = startTime;
        }
    }
}
