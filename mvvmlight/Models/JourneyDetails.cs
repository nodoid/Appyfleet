using System;
using System.Collections.ObjectModel;

namespace mvvmframework.Models
{
    public class JourneyDetails
    {
        public string JourneyDateTime { get; set; }
        public ObservableCollection<DBJourneyModel> Journey { get; set; }
    }
}
