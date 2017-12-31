namespace mvvmframework.Models
{
    public class Rankings
    {
        public int Rank { get; set; }
        public int Total { get; set; }
        public double FleetScore { get; set; }
        public double Score { get; set; }
        public bool IsCompany { get; set; } = false;
        public double Difference => FleetScore - Score;
    }
}
