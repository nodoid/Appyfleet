using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using mvvmframework.Models.JSon;
using mvvmframework.Interfaces;
using GalaSoft.MvvmLight.Ioc;
using mvvmframework.Models;
using System.Linq;

namespace mvvmframework
{
    public class WebInterface : IWebSevices
    {
        ILogFileService logService { get => SimpleIoc.Default.GetInstance<ILogFileService>(); }
        IEmailSms emailService { get => SimpleIoc.Default.GetInstance<IEmailSms>(); }
        IFileSystem fileService { get => SimpleIoc.Default.GetInstance<IFileSystem>(); }

        public static string LangString { get; set; }

        HttpMethod GetHttpMethod(string method)
        {
            return method.ToLowerInvariant() == "get" ? HttpMethod.Get : method.ToLowerInvariant() == "put" ? HttpMethod.Put : HttpMethod.Post;
        }

        StatusModel ErrorStatusModel => new StatusModel { Success = false, Message = string.Empty };

        T GetSelectedToken<T>(string json, string token, string method = "", bool errNoMatch = true)
        {
            T jsonRes = default(T);
            try
            {
                var jsonObj = JObject.Parse(json);
                jsonRes = jsonObj.SelectToken(token, errNoMatch).ToObject<T>();
            }
            catch (Exception ex)
            {
                logService.WriteLog($"WebInterface: {method}", ex.ToString());
            }
            return jsonRes;
        }

        async Task<T> GetResultForCall<T>(string method, string methodName, object jsonModel, string resToken, bool errRes = true) where T : class
        {

            var result = await WCFRESTServiceCall(method, methodName, JsonConvert.SerializeObject(jsonModel));
            T resultModel = default(T);
            try
            {
                var jo = JObject.Parse(result);
                resultModel = jo.SelectToken(resToken, errRes).ToObject<T>();
            }
            catch (Exception ex)
            {
                logService.WriteLog($"WebInterface: {method}", ex.ToString());
            }
            return resultModel;
        }

        async Task<string> WCFRESTServiceCall(string methodRequestType, string methodName, string bodyParam = "")
        {
            var result = string.Empty;
            try
            {
                var httpClient = new HttpClient();
                var request = new HttpRequestMessage(GetHttpMethod(methodRequestType), Constants.ServiceUrl + methodName);
                if (!string.IsNullOrEmpty(bodyParam))
                {
                    request.Content = new StringContent(bodyParam, Encoding.UTF8, "application/json");
                }
                var response = await httpClient.SendAsync(request);
                result = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                logService.WriteLog("WebInterface: WCFRESTServiceCall", ex.ToString());
            }
            return result;
        }

        public async Task<GetDriverScoreData> GetDriverScores(string user, string pass, int refineBy)
        {
            var model = new { emailAddress = user, password = pass, periodTypeId = refineBy };
            var resultModel = new GetDriverScoresResult
            {
                Status = ErrorStatusModel
            };
            var res = await GetResultForCall<GetDriverScoresResult>("post", "GetDriverScores", model, "GetDriverScoresResult");
            if (res != null)
                resultModel = res;

            return resultModel.Data;
        }

        public async Task<DriverGroupScoreListModel> GetDriverGroupScores(string user, string pass, int refineBy)
        {
            var model = new { emailAddress = user, password = pass, periodTypeId = refineBy };
            var resultModel = new DriverGroupScoreListModel
            {
                Status = ErrorStatusModel
            };
            var res = await GetResultForCall<DriverGroupScoreListModel>("post", "GetDriverGroupScores", model, "GetDriverGroupScoresResult");
            if (res != null)
                resultModel = res;

            return resultModel;
        }

        public async Task<SetLanguageResultJSon> SetLanguagePreference(string user, string pass, string code)
        {
            var model = new
            {
                emailAddress = user,
                password = pass,
                code
            };

            var resultModel = new SetLanguageResultJSon
            {
                Status = ErrorStatusModel
            };
            var res = await GetResultForCall<SetLanguageResultJSon>("post", "SetLanguage", model, "SetLanguageResult");
            if (res != null)
                resultModel = res;

            return resultModel;
        }

        public async Task<ExistingDriverModel> CheckForExistingDriver(string emailAddress, string lang)
        {
            var model = new
            {
                emailAddress,
                languageCode = lang,
                token = emailService.GenerateEmailHashingString(Constants.EMAIL_HASHING_KEY, emailAddress),
            };

            var resultModel = new ExistingDriverModel();
            var res = await GetResultForCall<ExistingDriverModel>("post", "ExistingDriver", model, "ExistingDriverResult");
            if (res != null)
                resultModel = res;
            return resultModel;
        }

        public async Task<RemoveVehicleResultJSon> RemoveVehicleParing(string user, string pass, VehicleModel vehicle)
        {
            var model = new
            {
                emailAddress = user,
                password = pass,
                vehicleId = vehicle.Id,
            };

            var resultModel = new RemoveVehicleResultJSon
            {
                Status = ErrorStatusModel
            };
            var res = await GetResultForCall<RemoveVehicleResultJSon>("post", "RemoveVehicle", model, "RemoveVehicleResult");
            if (res != null)
                resultModel = res;

            return resultModel;
        }

        public async Task<AddVehicleResultJSon> AddVehicle(string user, string pass, VehicleModel dataToAdd)
        {
            var model = new
            {
                emailAddress = user,
                password = pass,
                registration = dataToAdd.Registration,
                make = dataToAdd.Make,
                model = dataToAdd.Model,
                nickname = dataToAdd.Nickname,
            };

            var resultModel = new AddVehicleResultJSon
            {
                Status = ErrorStatusModel
            };
            var res = await GetResultForCall<AddVehicleResultJSon>("post", "AddVehicle", model, "AddVehicleResult");
            if (res != null)
                resultModel = res;

            return resultModel;
        }

        public async Task<GetLanguagesResponse> GetLanguageList(string user, string pass)
        {
            var model = new
            {
                emailAddress = user,
                password = pass
            };

            var resultModel = new GetLanguagesResponse
            {
                Status = ErrorStatusModel
            };
            var res = await GetResultForCall<GetLanguagesResponse>("post", "GetLanguages", model, "GetLanguagesResult");
            if (res != null)
                resultModel = res;

            return resultModel;
        }

        public async Task<GetScoreHistoryResponse> GetScoreHistory(string user, string pass, DateTime filterStartDate, DateTime filterEndDate)
        {
            try
            {
                var model = new
                {
                    emailAddress = user,
                    password = pass,
                    fromDate = filterStartDate,
                    toDate = filterEndDate,
                };

                var resultModel = new GetScoreHistoryResponse
                {
                    Status = ErrorStatusModel
                };
                var res = await GetResultForCall<GetScoreHistoryResponse>("post", "GetScoreHistory", model, "GetScoreHistoryResult");
                if (res != null)
                    resultModel = res;

                return resultModel;
            }
            catch (Exception ex)
            {
                logService.WriteLog("WebInterface: GetScoreHistory", ex.ToString());
            }

            return null;
        }

        public async Task<JourneyListModel> GetExpensesList(string user, string pass, DateTime filterStartDate, DateTime filterEndDate)
        {
            try
            {
                var model = new
                {
                    emailAddress = user,
                    password = pass,
                    start = filterStartDate,
                    end = filterEndDate,
                };

                var resultModel = new JourneyListModel
                {
                    Status = ErrorStatusModel
                };
                var res = await GetResultForCall<JourneyListModel>("post", "expenseslist", model, "ExpensesListResult");
                if (res != null)
                    resultModel = res;

                return resultModel;
            }
            catch (Exception ex)
            {
                logService.WriteLog("WebInterface: GetScoreHistory", ex.ToString());
            }

            return null;
        }

        public async Task<VehicleListModel> GetPairedVehicles(string user, string pass)
        {
            try
            {
                var model = new { emailAddress = user, password = pass };
                var resultModel = new VehicleListModel
                {
                    Status = ErrorStatusModel
                };
                var res = await GetResultForCall<VehicleListModel>("post", "VehiclePairingList", model, "VehiclePairingListResult");
                if (res != null)
                    resultModel = res;

                return resultModel;
            }
            catch (Exception ex)
            {
                logService.WriteLog("WebInterface: GetPairedVehicles(Send)", ex.ToString());
            }
            return null;
        }

        public async Task<VehiclePairingModel> PairVehicle(string user, string pass, long vehicleId, string bluetoothId)
        {
            var model = new
            {
                emailAddress = user,
                password = pass,
                vehicleId,
                bluetoothId
            };
            var resultModel = new VehiclePairingModel
            {
                Status = ErrorStatusModel
            };
            var res = await GetResultForCall<VehiclePairingModel>("post", "PairVehicle", model, "PairVehicleResult");
            if (res != null)
                resultModel = res;

            return resultModel;
        }

        public async Task<NotificationsListModel> GetNotifications(string user, string pass)
        {
            var model = new { emailAddress = user, password = pass };
            var resultModel = new NotificationsListModel
            {
                Status = ErrorStatusModel
            };
            var res = await GetResultForCall<NotificationsListModel>("post", "notificationslistv2", model, "NotificationsListV2Result");
            if (res != null)
                resultModel = res;

            return resultModel;
        }

        public async Task<StatusModel> MarkNotificationRead(string user, string pass, long id)
        {
            var model = new { emailAddress = user, password = pass, notificationId = id };
            var resultModel = ErrorStatusModel;
            var res = await GetResultForCall<StatusModel>("post", "notificationread", model, "NotificationReadResult");
            if (res != null)
                resultModel = res;

            return resultModel;
        }

        public async Task<JoinFleetResponseJson> RequestJoinFleet(string user, string pass, string accessCode)
        {
            var model = new { emailAddress = user, password = pass, accessCode };
            var resultModel = new JoinFleetResponseJson
            {
                Status = ErrorStatusModel
            };
            var res = await GetResultForCall<JoinFleetResponseJson>("POST", "JoinFleet", model, "JoinFleetResult");
            if (res != null)
                resultModel = res;

            return resultModel;
        }

        public async Task<RegisterResponseJSon> Register(RegisterRequestJSon model)
        {
            model.dateOfBirth = $"{model.dateOfBirth}-01-01";
            var json = JsonConvert.SerializeObject(model);
            var result = await WCFRESTServiceCall("POST", "V3Register", json);

            var resultModel = new RegisterResponseJSon { RegisterV3Result = new RegisterModel { Status = ErrorStatusModel } };

            var jr = GetSelectedToken<RegisterModel>(result, "RegisterV3Result", "Register");
            if (jr != null)
            {
                resultModel.RegisterV3Result = jr;
            }

            return resultModel;
        }

        public async Task<DriverModel> Login(string username, string password, string phoneId, string languageCode)
        {
            var model = new { emailAddress = username, password, phoneId, languageCode };
            var resultModel = new DriverModel
            {
                Status = ErrorStatusModel
            };

            var res = await GetResultForCall<DriverModel>("POST", "Login3", model, "Login3Result");
            if (res != null)
                resultModel = res;
            return resultModel;
        }

        public async Task<JourneyCoordinatesModel> GetJourneyCoordinates(string user, string password, long journeyID)
        {
            try
            {
                var model = new { emailAddress = user, password, journeyId = journeyID };

                var resultModel = new JourneyCoordinatesModel
                {
                    Status = ErrorStatusModel
                };
                var res = await GetResultForCall<JourneyCoordinatesModel>("post", "journeycoordinates", model, "JourneyCoordinatesResult");
                if (res != null)
                    resultModel = res;

                return resultModel;
            }
            catch (Exception ex)
            {
                logService.WriteLog("WebInterface: GetJourneyCoordinates", ex.ToString());
            }

            return null;
        }

        public async Task<JourneyListModel> GetJourneys(string user, string password)
        {
            try
            {
                var model = new { emailAddress = user, password };
                var resultModel = new JourneyListModel
                {
                    Status = ErrorStatusModel
                };
                var res = await GetResultForCall<JourneyListModel>("post", "journeylist", model, "JourneyListResult");
                if (res != null)
                    resultModel = res;

                return resultModel;
            }
            catch (Exception ex)
            {
                logService.WriteLog("WebInterface: GetJourneys", ex.ToString());
            }

            return null;
        }

        public async Task<AddOdometerReadingResultJSon> AddOdoReading(string user, string password, OdoReadingModel reading)
        {
            var model = new
            {
                emailAddress = user,
                password,
                vehicleId = reading.SelectedVehicle.Id,
                reading = reading.Reading,
                dateRead = reading.DateRead
            };
            var resultModel = new AddOdometerReadingResultJSon { Status = ErrorStatusModel };

            var res = await GetResultForCall<AddOdometerReadingResultJSon>("post", "AddOdometerReading", model, "AddOdometerReadingResult");
            if (res != null)
                resultModel = res;

            return resultModel;
        }

        public async Task<GetOdometerReadingsResponseJson> GetOdoReadings(string user, string password, long vehicleId)
        {
            try
            {
                var model = new { emailAddress = user, password, vehicleId };
                var resultModel = new GetOdometerReadingsResponseJson
                {
                    Status = ErrorStatusModel
                };
                var res = await GetResultForCall<GetOdometerReadingsResponseJson>("post", "GetOdometerReadings", model, "GetOdometerReadingsResult");
                if (res != null)
                    resultModel = res;

                return resultModel;
            }
            catch (Exception ex)
            {
                logService.WriteLog("WebInterface: GetOdoReadings", ex.ToString());
            }

            return null;
        }

        public async Task<StatusModel> ChangeMobileNumber(string user, string password, string mobileNumber)
        {
            var model = new { emailAddress = user, password, mobileNumber };
            var resultModel = ErrorStatusModel;
            var res = await GetResultForCall<StatusModel>("post", "changemobilenumber", model, "ChangeMobileNumberResult");
            if (res != null)
                resultModel = res;

            return resultModel;
        }

        public async Task<StatusModel> ChallengeRoadSpeedLimit(string user, string pass, int id, int newSpeed, string comments = "Android user")
        {
            var model = new
            {
                emailAddress = user,
                password = pass,
                eventId = id,
                suggestedSpeed = newSpeed,
                comments
            };

            var resultModel = ErrorStatusModel;
            var res = await GetResultForCall<StatusModel>("post", "SendRoadForValidation", model, "SendRoadForValidationResult");
            if (res != null)
                resultModel = res;

            return resultModel;
        }

        public async Task<StatusModel> ChangePassword(string user, string pass, string password, string newPassword)
        {
            var model = new { emailAddress = user, password = pass, currentPassword = password, newPassword };
            var resultModel = ErrorStatusModel;
            var res = await GetResultForCall<StatusModel>("post", "changepassword", model, "ChangePasswordResult");
            if (res != null)
                resultModel = res;

            return resultModel;
        }

        public async Task<SubmitSOSResponseModel> SendSOS(string user, string pass, double lat, double lon, DateTime timeStamp)
        {
            var model = new
            {
                emailAddress = user,
                password = pass,
                dateTimeOfSOS = timeStamp,
                longitude = lon,
                latitude = lat
            };

            var resultModel = new SubmitSOSResponseModel { Status = ErrorStatusModel };
            var res = await GetResultForCall<SubmitSOSResponseModel>("POST", "SubmitSOS", model, "SubmitSOSResponse");
            if (res != null)
                resultModel = res;

            return resultModel;
        }

        public async Task<ForgottenPassword2ResponseModel> ForgottenPasswordRequest(string username, string dob)
        {
            var model = new { emailAddress = username, dateOfBirth = $"{dob}-01-01" };
            var resultModel = new ForgottenPassword2ResponseModel { Status = ErrorStatusModel };
            var res = await GetResultForCall<ForgottenPassword2ResponseModel>("POST", "ForgottenPassword", model, "ForgottenPasswordResult");
            if (res != null)
                resultModel = res;

            return resultModel;
        }

        public async Task<ReceiveMarketingEmailsResponseJson> SetMarketingPreference(string user, string pass, bool agree)
        {
            var model = new
            {
                emailAddress = user,
                password = pass,
                agree
            };
            var resultModel = new ReceiveMarketingEmailsResponseJson { Status = ErrorStatusModel };
            var res = await GetResultForCall<ReceiveMarketingEmailsResponseJson>("post", "ReceiveMarketingEmails", model, "ReceiveMarketingEmailsResult");
            if (res != null)
                resultModel = res;

            return resultModel;
        }

        public async Task<GetMarketingFlagStateResponseJson> GetMarketingPreference(string user, string pass)
        {
            var model = new
            {
                emailAddress = user,
                password = pass
            };

            var resultModel = new GetMarketingFlagStateResponseJson { Status = ErrorStatusModel };
            var res = await GetResultForCall<GetMarketingFlagStateResponseJson>("post", "GetMarketingFlagState", model, "GetMarketingFlagStateResult");
            if (res != null)
                resultModel = res;

            return resultModel;
        }

        public async Task<bool> DownloadAndSaveFile(string url)
        {
            var filename = url.Split('/').Last();
            if (fileService.FileExists(filename))
                return true;
            else
            {
                await fileService.DownloadFile(url, filename);
                return true;
            }
        }

        public string GetTermsAndConditionsText => tandc;

        static string tandc = "Terms and Conditions";
    }
}
