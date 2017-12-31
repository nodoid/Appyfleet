using System;
using System.Collections.Generic;
using SQLite.Net.Attributes;

namespace mvvmframework.Models.JSon
{
    public class GetDriverScoreData
    {
        public double BusinessMileage { get; set; }
        public int FleetRank { get; set; }
        public int FleetRankOutOf { get; set; }
        public double FleetScore { get; set; }
        public int GroupRank { get; set; }
        public int GroupRankOutOf { get; set; }
        public double GroupScore { get; set; }
        public double PersonalMileage { get; set; }
        public double Score { get; set; }
    }

    public class GetDriverScoresResult
    {
        public GetDriverScoreData Data { get; set; }
        public StatusModel Status { get; set; }
    }
}
