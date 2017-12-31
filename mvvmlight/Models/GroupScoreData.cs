using System.Collections.Generic;

namespace mvvmframework.Models
{
    public class DriverGroupScoreModel
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int Rank { get; set; }
        public int RankOutOf { get; set; }
        public double Score { get; set; }
    }

    public class DriverGroupScoreListModel
    {
        public List<DriverGroupScoreModel> Data { get; set; }
        public StatusModel Status { get; set; }
    }
}
