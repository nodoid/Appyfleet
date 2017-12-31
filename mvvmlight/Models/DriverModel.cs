using System;

namespace mvvmframework
{
    public class DriverModel
    {
        public int DriverId { get; set; }
        public string DriverTipsURL { get; set; }
        public string FleetName { get; set; }
        public double FleetScore { get; set; }
        public string Forename { get; set; }
        public int IsOnDefautFleet { get; set; }
        public string LanguageCode { get; set; }
        public int LastJourneyNumber { get; set; }
        public int LastRecordNumber { get; set; }
        public int LogsExpire { get; set; }
        public string NationalRank { get; set; }
        public int NationRank => NationalRank == "--" ? 0 : Convert.ToInt32(NationalRank);
        public string NotRegisteredPdf { get; set; }
        public int OdoReading { get; set; }
        public string OdoVehicleReg { get; set; }
        public string PhoneId { get; set; }
        public int? Rank { get; set; }
        public string SOSFleetManagerMobileNumber { get; set; }
        public double Score { get; set; }
        public string SimNumber { get; set; }
        public string Surname { get; set; }
        public string TermsAndConditionsURL { get; set; }
        public int TotalDrivers { get; set; }
        public string UserGuideURL { get; set; }

        public StatusModel Status { get; set; }
    }
}
