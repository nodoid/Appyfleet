using SQLite.Net.Attributes;

namespace mvvmframework
{
    public class DBDriverModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; private set; }

        public string UserName { get; set; }

        public int DriverId { get; set; }
        public string DriverTipsURL { get; set; }
        public string Forename { get; set; }
        public int LastJourneyNumber { get; set; }
        public int LastRecordNumber { get; set; }
        public string NationalRank { get; set; }
        public string PhoneId { get; set; }
        public string Rank { get; set; }
        public double Score { get; set; }
        public string SimNumber { get; set; }
        public string Surname { get; set; }
        public string LanguageCode { get; set; }
        public int LogsExpire { get; set; }
        public string TermsAndConditionsURL { get; set; }
        public string UserGuideURL { get; set; }
        public string SOSFleetManagerMobileNumber { get; set; }
    }
}
