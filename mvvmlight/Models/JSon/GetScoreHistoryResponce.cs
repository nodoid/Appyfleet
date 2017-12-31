using System.Collections.Generic;
using mvvmframework.Models;

namespace mvvmframework
{
    public class GetScoreHistoryResponse
    {
        public List<ScoreHistoryData> Scores { get;set;}
        public StatusModel Status { get; set; }
    }
}
