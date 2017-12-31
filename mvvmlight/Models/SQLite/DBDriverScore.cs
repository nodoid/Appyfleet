using System;
using SQLite.Net.Attributes;

namespace mvvmframework.Models.SQLite
{
    public class DBDriverScore
    {
        public string BusinessMileage { get; set; }
        public int FleetRank { get; set; }
        public int FleetRankOutOf { get; set; }
        public string FleetScore { get; set; }
        public int GroupRank { get; set; }
        public int GroupRankOutOf { get; set; }
        public string GroupScore { get; set; }
        public int PersonalMileage { get; set; }
        public string Score { get; set; }
    }
}
