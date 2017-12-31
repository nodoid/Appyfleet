using System.Linq;
using System.Collections.Generic;
using mvvmframework.Models;
using mvvmframework.ViewModels.Common;
using System;
using mvvmframework.Interfaces;

namespace mvvmframework.ViewModels
{
    public class ScoreHistoryViewModel : CommonJourneys
    {
        IWebSevices webService;
        IConnection connectService;
        IRepository repoService;

        public ScoreHistoryViewModel(IConnection connect, IWebSevices web, IRepository repo, ILocation loc, ISockets sock) :
        base(loc, repo,sock)
        {
            webService = web;
            connectService = connect;
            repoService = repo;
        }

        List<DBJourneyModel> FilteredJourneys => (from j in JourneysList
                                                  where j.StartDate >= StartDate
                                                  where j.EndDate <= EndDate
                                                  select j).OrderBy(t => t.EndDate.Year).
                            ThenBy(t => t.EndDate.Month).
                            ThenBy(t => t.EndDate.Day).
                            ThenBy(t => t.EndDate.Hour).
                                                         ThenBy(t => t.EndDate.Minute).ToList();

        public List<ScoreData> GetScoreData
        {
            get
            {
                var scores = new List<ScoreData>();
                var _ = FilteredJourneys;
                if (_.Count > 0)
                {
                    foreach (var f in _)
                        scores.Add(new ScoreData { Day = f.StartDate.Day, Score = f.SpeedScore, Date = f.StartDate.ToString("dd-MMM") });
                }

                return scores.DistinctBy(t=>t.Date).ToList();
            }
        }

        public int GetDateSpan
        {
            get
            {
                var _ = FilteredJourneys;
                var first = FilteredJourneys[0];
                var last = FilteredJourneys[FilteredJourneys.Count - 1];

                return (int)(last.StartDate - first.StartDate).TotalDays;
            }
        }

        public Tuple<double, double> GetMinMax => new Tuple<double, double>(FilteredJourneys.Min(t => t.SpeedScore), 
                                                                            FilteredJourneys.Max(t => t.SpeedScore));

        public string DateRange => $"{StartDate.Date.ToString("dd MMMM yyyy")} - {EndDate.Date.ToString("dd MMMM yyyy")}";

        public string StartDateText => $"{StartDate.Date.ToString("dd MMM yyyy")}";

        public string EndDateText => $"{EndDate.Date.ToString("dd MMM yyyy")}";

        public string GetAverageScore => ((GetMinMax.Item2 - GetMinMax.Item1) / 7).ToString("F1");
    }
}
