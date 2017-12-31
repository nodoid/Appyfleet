using System;
using System.Threading.Tasks;
using mvvmframework.Models.JSon;
using mvvmframework.Models;

namespace mvvmframework
{
    public interface IWebSevices
    {
        Task<SetLanguageResultJSon> SetLanguagePreference(string user, string pass, string code);
        Task<ExistingDriverModel> CheckForExistingDriver(string emailAddress, string lang);
        Task<RemoveVehicleResultJSon> RemoveVehicleParing(string user, string pass, VehicleModel vehicle);
        Task<AddVehicleResultJSon> AddVehicle(string user, string pass, VehicleModel dataToAdd);
        Task<GetLanguagesResponse> GetLanguageList(string user, string pass);
        Task<GetScoreHistoryResponse> GetScoreHistory(string user, string pass, DateTime filterStartDate, DateTime filterEndDate);
        Task<JourneyListModel> GetExpensesList(string user, string pass, DateTime filterStartDate, DateTime filterEndDate);
        Task<VehicleListModel> GetPairedVehicles(string user, string pass);
        Task<VehiclePairingModel> PairVehicle(string user, string pass, long vehicleId, string bluetoothId);
        Task<NotificationsListModel> GetNotifications(string user, string pass);
        Task<StatusModel> MarkNotificationRead(string user, string pass, long id);
        Task<JoinFleetResponseJson> RequestJoinFleet(string user, string pass, string accessCode);
        Task<RegisterResponseJSon> Register(RegisterRequestJSon model);
        Task<DriverModel> Login(string username, string password, string phoneId, string languageCode);
        Task<JourneyListModel> GetJourneys(string user, string password);
        Task<AddOdometerReadingResultJSon> AddOdoReading(string user, string password, OdoReadingModel reading);
        Task<GetOdometerReadingsResponseJson> GetOdoReadings(string user, string password, long vehicleId);
        Task<StatusModel> ChangeMobileNumber(string user, string password, string mobileNumber);
        Task<StatusModel> ChallengeRoadSpeedLimit(string user, string pass, int id, int newSpeed, string comments = "Android user");
        Task<StatusModel> ChangePassword(string user, string pass, string password, string newPassword);
        Task<SubmitSOSResponseModel> SendSOS(string user, string pass, double lat, double lon, DateTime timeStamp);
        Task<ForgottenPassword2ResponseModel> ForgottenPasswordRequest(string username, string dob);
        Task<ReceiveMarketingEmailsResponseJson> SetMarketingPreference(string user, string pass, bool agree);
        Task<GetMarketingFlagStateResponseJson> GetMarketingPreference(string user, string pass);
        string GetTermsAndConditionsText { get; }
        Task<GetDriverScoreData> GetDriverScores(string user, string pass, int refineBy);
        Task<JourneyCoordinatesModel> GetJourneyCoordinates(string user, string password, long journeyID);
        Task<bool> DownloadAndSaveFile(string url);
        Task<DriverGroupScoreListModel> GetDriverGroupScores(string user, string pass, int refineBy);
    }
}
