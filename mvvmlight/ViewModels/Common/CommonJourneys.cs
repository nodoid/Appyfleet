using System;
using System.Collections.Generic;
using System.Linq;
using mvvmframework.Enums;
using mvvmframework.Interfaces;
using mvvmframework.Languages;
using mvvmframework.Models;

namespace mvvmframework.ViewModels.Common
{
    public class CommonJourneys : BaseLocationViewModel
    {
        IRepository repoService;

        public CommonJourneys(ILocation loc, IRepository repo, ISockets sock) : base(loc, repo, sock)
        {
            repoService = repo;
        }

        static DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set { Set(() => StartDate, ref startDate, value, true); }
        }

        static DateTime endDate;
        public DateTime EndDate
        {
            get { return endDate; }
            set { Set(() => EndDate, ref endDate, value, true); }
        }

        public string DateFrom => StartDate.Date.ToString("D");
        public string DateTo => EndDate.Date.ToString("D");

        JourneyTypes journeyType;
        public JourneyTypes JourneyType
        {
            get { return journeyType; }
            set { Set(() => JourneyType, ref journeyType, value, true); }
        }

        List<DBJourneyModel> privateJourneys;
        public List<DBJourneyModel> PrivateJourneys
        {
            get { return privateJourneys; }
            set { privateJourneys = value.Where(t => t.JourneyType.ToLowerInvariant() == "private").ToList(); }
        }

        List<DBJourneyModel> businessJourneys;
        public List<DBJourneyModel> BusinessJourneys
        {
            get { return businessJourneys; }
            set { businessJourneys = value.Where(t => t.JourneyType.ToLowerInvariant() == "business").ToList(); }
        }

        List<DBJourneyModel> journeys;
        public List<DBJourneyModel> Journeys
        {
            get { return journeys; }
            set { Set(() => Journeys, ref journeys, value, true); }
        }

        public string Message => Langs.Const_Msg_Select_Date_Range_2;

        public JourneyData GetSpecificJourney(int journeyId)
        {
            var journey = repoService.GetList<JourneyData>().FirstOrDefault(t=>t.JourneyId == journeyId);

            if (journey == null)
            {
                var _ = JourneysList.FirstOrDefault(t => t.JourneyId == journeyId);
                if (_ != null)
                {
                    journey = new JourneyData();
                    journey.JourneyId = journeyId;
                    journey.JourneyEndDate = _.EndDate;
                    journey.JourneyStartDate = _.StartDate;
                    journey.JourneyNumber = _.JourneyNumber;
                }
            }

            if (journey != null)
            {
                journey.GPSData = new List<JourneyCoordinates>();
                journey.GPSData.AddRange(repoService.GetList<JourneyCoordinates>().Where(t=>t.JourneyId == journeyId).ToList());
            }

            return journey;
        }
    }
}
