using System;
using System.Collections.Generic;

namespace mvvmframework.Interfaces
{
    public interface IJourneyService
    {
        void InitialiseService(List<string> queue, string sim, int odo, bool gpson, string pair, int rec, DateTime app, int journey, string version);
        bool AddLocationUpdate();
        List<string> EndJourney();
    }
}
