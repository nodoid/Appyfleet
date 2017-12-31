using System;
using System.Collections.Generic;

namespace mvvmframework.Interfaces
{
    public interface IExpenses
    {
        void ExportJourneysToEmail(List<JourneyModel> journeys);
    }
}
