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
                var model = new {
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

            var resultModel = new RegisterResponseJSon { RegisterV3Result = new RegisterModel {Status = ErrorStatusModel} };

                var jr = GetSelectedToken<RegisterModel>(result, "RegisterV3Result", "Register");
                if (jr != null)
                {
                    resultModel.RegisterV3Result = jr;
                }

            return resultModel;
        }

        public async Task<DriverModel> Login(string username, string password, string phoneId, string languageCode)
        {
            var model = new { emailAddress = username, password, phoneId, languageCode};
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
            catch(Exception ex)
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
                emailAddress = user, password = pass,
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
            var model = new { emailAddress = user, password = pass, currentPassword = password, newPassword};
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

        static string tandc = "APPY FLEET LIMITED – APPY FLEET DRIVER APP TERMS AND CONDITIONS\nWe license the use of our Appy Fleet Driver App to you on the basis of these Conditions.\nWe do not sell the Driver App to you and we remain the owners of the Driver App and\nany associated documents at all times.\nReferences to the Driver in these Conditions are to the individual downloading the Driver\nApp.The Driver's attention is drawn in particular to the provisions of condition 7.\nPLEASE NOTE: the Driver App is only intended for use by individuals employed as\ndrivers by a customer of Appy Fleet Limited for the purposes of fleet management. The\nDriver App is not intended for use by any consumers. A consumer is an individual acting\nfor purposes which are wholly or mainly outside that individual's trade, business, craft\nor profession.\n1.Definitions and Interpretation\n1.1 Definitions In these Conditions, the following definitions apply:\nApplicable Law in any jurisdiction in which the Services are to be are to\nbe supplied under these Conditions, any and all\napplicable laws, regulations and industry standards or\nguidance including any applicable and binding judgment\nof a relevant court of law or competent regulator;\nBusiness Day a day other than a Saturday, Sunday or public holiday in\nEngland when banks in London are open for business;\nCommencement Date has the meaning given in condition 2.1;\nConditions the terms and conditions set out in this document as\namended from time to time in accordance with condition\n12.3;\nConfidential Information any and all know-how, documentation and information,\nwhether commercial, financial, technical, operational or\notherwise, relating to the business, affairs, customers,\nsuppliers, employees, affiliates, products and/or\nmethods of the Supplier or the Fleet Manager and\ndisclosed to or otherwise obtained by the other party in\nconnection with these Conditions;\nContract the contract between the Supplier and the Fleet\nManager;\nDriver the individual who downloads the Driver App and who is\nan employee of a customer or potential customer of the\nSupplier;\nDriver App the app to be downloaded by the Driver to utilise the\nServices(for iPhones download here, for Android \n2\n53868398-1\nphones download here, for Windows phones download\nhere).\nDriver Availability the availability of each driver to be able to use the Driver\nApp as and when the Fleet Manager reasonably\nrequires;\nDeliverables all documents, products and materials developed by the\nSupplier or the Supplier Personnel as part of or in\nrelation to the Services in any form or media, including\ndrawings, maps, plans, diagrams, designs, pictures,\ncomputer programs, data, specifications and reports\n(including drafts), whether made available to the Driver\nthrough the Driver App or otherwise;\nFleet Manager an employer of the Driver who adds the Driver to their\nPortal;\nForce Majeure Event has the meaning given in condition 11;\nIntellectual Property Rights patents, rights to inventions, copyright and related rights,\nmoral rights, trade marks, business names and domain\nnames, rights in get-up, goodwill and the right to sue for\npassing off, rights in designs, database rights, rights to\nuse and protect the confidentiality of, confidential\ninformation(including know-how and trade secrets) and\nall other intellectual property rights, in each case\nwhether registered or unregistered and including all\napplications and rights to apply for and be granted\nrenewals or extensions of, and rights to claim priority\nfrom, such rights and all similar or equivalent rights or\nforms of protection which subsist or will subsist now or\nin the future in any part of the world;\nLicence has the meaning set out in condition 2.1;\nPrivacy Policy the Supplier's privacy policy, as updated from time to\ntime and notified to the Customer and the Drivers, the\ncurrent version of which is set out here\nwww.appyfleet.co.uk/DownloadFile?fileName=cookie_a\nnd_privacy_policy ;\nPortal the Fleet Manager Portal created and maintained by the\nSupplier, through which the Fleet Manager can access\nthe driver tracking services;\nServices the driver tracking services, including use of the Driver\nApp and any Deliverables, to be provided by the Supplier\nto the Fleet Manager under the Contract;\n3\n53868398-1\nSupplier Appy Fleet Limited (registered in England and Wales\nwith company number 10387540);\nSupplier Materials all materials, equipment and tools, drawings,\nspecifications and data supplied or made available by\nthe Supplier to the Driver;\nSupplier Personnel the personnel, including subcontractors, engaged by the\nSupplier in the provision of the Driver App and the\nperformance of the Services;\nVehicle a vehicle owned or used by the Fleet Manager or, where\napplicable, its Drivers in the course of the Fleet\nManager's business which is registered within the Portal\nand allocated to a slot;\nVirus any thing or device(including any software, code, file or\nprogramme) which may:\n(a) prevent, impair or otherwise adversely affect the\noperation of any computer software, hardware\nor network, any telecommunications service,\nequipment or network or any other service or\ndevice;\n(b) prevent, impair or otherwise adversely affect\naccess to or the operation of any programme or\ndata, including the reliability of any programme\nor data(whether by re-arranging, altering or\nerasing the programme or data in whole or part\nor otherwise); or\n(c) adversely affect the user experience, including\nworms, trojan horses, viruses and other similar\nthings or devices.\n1.2 Interpretation In these Conditions, the following rules apply:\n(a) a reference to a statute or statutory provision is a reference to such statute or provision\nas amended, re-enacted or superseded from time to time and includes any subordinate\nlegislation made under that statute or statutory provision, as amended or re-enacted;\nany phrase introduced by the terms including, include, in particular or any similar\nexpression shall be construed as illustrative and shall not limit the sense of the words\npreceding those terms.\n2.Basis of contract\n2.1 In consideration of the Driver agreeing to abide by the terms of these Conditions, the Supplier\ngrants to the Driver a non-exclusive, non-transferable licence to use the Driver App on the terms\nof these Conditions(the Licence) for a period of:\n(a) a minimum of 30 days(or any longer period which may be agreed between the\nSupplier and the Fleet Manager) prior to the Driver being linked to the Portal; and \n4\n53868398-1\n(b) once the Driver has been linked to a Portal, for so long as the Fleet Manager continues\nto use the Portal;\nuntil the Licence is terminated in accordance with these Conditions.These Conditions apply to\nthe exclusion of any other terms that the Driver seeks to impose or incorporate, or which are\nimplied by trade, custom, practice or course of dealing.\n2.2 Any drawings, descriptive matter, or advertising produced by the Supplier and any illustrations\ncontained on the Supplier's website are produced for the sole purpose of giving an approximate\nidea of the Services described in them. They shall not form part of the Agreement or have any\ncontractual force.\n2.3 The Driver waives any right it might otherwise have to rely on any term endorsed upon,\ndelivered with or contained in any document that is inconsistent with these Conditions.\n3. Performance of the Services\n3.1 The Supplier shall, for the duration of the Licence, make the Driver App available to the Driver\nin accordance with these Conditions in all material respects.\n3.2 The Supplier shall use reasonable endeavours to ensure the availability of the Services,\nhowever the Driver acknowledges that:\n(a) the provision of the Driver App and the Services is dependent upon the Driver fulfilling\nits obligations under these conditions, Driver Availability, and the Fleet Manager\ncomplying with its obligations under the agreement between the Fleet Manager and the\nSupplier;\n(b) there may be circumstances beyond the Supplier's reasonable control which prevent\nthe Driver App from being available including but not limited to disablement of the Driver\nApp whether actively caused by a party other than the Supplier or the Driver or due to\nloss of connectivity;\nsuch that the Supplier cannot guarantee the availability of the Driver App or the\nServices.\n3.3 The Driver acknowledges that, unless otherwise agreed between the Fleet Manager and the\nSupplier or notified by the Supplier, each Driver may only be registered to one fleet manager\nat any one time.Accordingly, if the Driver is already registered and using Appy Fleet with\nanother fleet manager this will affect the Driver Availability (in accordance with condition Error!\nReference source not found.) and that Driver will not be able to be linked with the Fleet\nManager's Portal.\n3.4 The Supplier warrants that it shall provide the Services with reasonable care and skill. The\nterms implied by sections 3 to 5 of the Supply of Goods and Services Act 1982 are, to the fullest\nextent permitted by law, excluded from these Conditions.\n5\n53868398-1\n4. Driver obligations\n4.1 The Driver shall:\n(a) provide such information to the Supplier as the Supplier may reasonably request for\nthe purpose of providing the Services and ensure that such information is accurate in\nall material respects;\n(b) ensure that the Driver App is downloaded to a compatible device and update the Driver\nApp from time to time. The Driver acknowledges that in order to receive updates to the\nDriver App, it will need to ensure that:\n(i) push notifications are enabled on the device or devices on which the Driver\nApp is downloaded; and\n(ii) any instructions, howsoever issued, of the Supplier or the Fleet Manager\nrelating to actions required to facilitate any and all updates to the Driver App\nare promptly followed;\n(c) where the Driver downloads the Driver App without it first being linked to the Portal via\nthe relevant Fleet Manager, such instance being referred to as Unlinked, the Driver\nshall provide the following information, which shall include personal data:\n(i) First name and last name;\n(ii) Name of the Fleet Manager's organisation;\n(iii) Role within the Fleet Manager's organisation;\n(iv) Contact number;\n(v) Date of birth;\n(vi) Business email address; and\n(vii) Vehicle registration, make and model;\nand the Driver acknowledges that such data will be provided by the Supplier to\nthe Fleet Manager and shall be processed in accordance with condition 9.\n(d) comply with all Applicable Law with respect to its activities under the Licence and these\nConditions; and\n(e) comply with condition 4.2.\n4.2 When using the Driver App the Services, the Driver must comply with all Applicable Law and\nthese Conditions. In particular, the Driver must not:\n(a) use Driver App or the Services in any unlawful manner or in a manner which promotes\nor encourages illegal activity or act fraudulently or maliciously, for example, by hacking\ninto or inserting malicious code, including Viruses, or harmful data, into the Driver App,\nany Service or any operating system;\n(b) attempt to gain unauthorised access to the Portal or any networks, servers, operating\nsystems or computer systems connected to the Portal; \n6\n53868398-1\n(c) modify, adapt, translate or reverse engineer any part of the Driver App or the Portal or\nre-format or frame any portion of the pages comprising the Driver App, save to the\nextent expressly permitted by these Conditions or by Applicable Law;\n(d) infringe the Supplier's Intellectual Property Rights (including, without limitation,\ncopyright infringement) or those of any third party in relation to the use of the Driver\nApp, the Portal the Services (to the extent that such use is not licensed by these\nConditions);\n(e) transmit any material that is defamatory, offensive or otherwise objectionable or which\nmay or is likely to damage the reputation of the Supplier in relation to the Driver's use\nof the Driver App; and/or\n(f) collect any information or data from the Driver App or the Portal (save as required for\nthe purpose of obtaining the benefit of the Services) or the Supplier's systems or\nattempt to decipher any transmissions to or from the servers hosting the Portal or the\nDriver App or running any Service.\n4.3 The Driver shall indemnify the Supplier in respect of any claims arising from a breach of\ncondition 4.2.\n4.4 If the Supplier's performance of any of its obligations under these Conditions is prevented or\ndelayed by any act or omission of the Driver or failure by the Driver to perform any relevant\nobligation (Driver Default):\n(a) the Supplier shall, without limiting its other rights or remedies, have the right to suspend\nprovision of the Services until the Driver remedies the Driver Default;\n(b) the Supplier shall not be liable for any losses sustained or incurred by the Driver arising\ndirectly or indirectly from the Supplier's failure or delay to perform any of its obligations\nas set out in these Conditions unless otherwise agreed with the Fleet Manager;\n(c) the Driver shall reimburse the Supplier on demand for any losses sustained or incurred\nby the Supplier to the Fleet Manager arising directly from the Driver Default.\n5.Charges(reservation of the Supplier's position)\n5.1 The Supplier shall provide the Services to the Driver at no cost, however, the Supplier reserves\nthe right to introduce charges from time to time, in which case the Agreement will terminate\nand the parties shall enter into a new contract for the provision of paid services.\n6. Intellectual Property Rights\n6.1 The Driver acknowledges that all Intellectual Property Rights used by or subsisting in the Driver\nApp are and shall remain the sole property of the Supplier or (as the case may be) the third\nparty rights owner.\n6.2 All Intellectual Property Rights arising in the performance of the Services, including the use of\nthe Driver App, shall be owned by the Supplier.\n6.3 The Driver may use the Deliverables only for the purpose of receiving the Services and for the\nbenefit of the Fleet Manager. The Driver shall not supply the Deliverables to any third party or\nuse the Services or the Deliverables to provide a service to any third party other than the Fleet\nManager.\n7\n53868398-1\n7. Liability\n7.1 Nothing in these Conditions shall limit or exclude the Supplier's liability for:\n(a) death or personal injury caused by its negligence, or the negligence of its Supplier\nPersonnel(as applicable);\n(b) fraud or fraudulent misrepresentation;\n(c) any matter in respect of which it would be unlawful for the Supplier to exclude or restrict\nliability.\n7.2 Subject to condition 7.1,\nthe Supplier shall under no circumstances whatever be liable to the Driver, whether in contract, tort\n(including negligence), misrepresentation, breach of statutory duty, or otherwise, for:\n(i) any indirect, special, consequential or pure economic loss or damage;\n(ii) any loss of profits, anticipated profits, revenue or business opportunities; or\n(iii) damage to goodwill;\n(iv) data roaming charges incurred by the Drivers;\n(v) any liability relating to the use of the Driver App other than wholly in the course\nof the Driver's trade, craft, business or profession;\nin each case arising as a direct or indirect result of the relevant claim.\n8. Confidentiality\n8.1 Subject to condition 8.2, each party to these Conditions (the Recipient) shall:\n(a) use the other party's(the Disclosing Party's) Confidential Information solely for the\nperformance of the Licence; and\n(b) keep the Disclosing Party's Confidential Information strictly confidential and not, without\nthe Disclosing Party’s prior written consent, disclose it to any other person.\n8.2 The Recipient may disclose the Disclosing Party's Confidential Information:\n(a) to its employees, officers, representatives or advisers who need to know such\ninformation for the purposes of exercising the Recipient's rights or carrying out its\nobligations under or in connection with the Licence and these Conditions and the\nRecipient shall ensure that such persons comply with this condition 8.2;\n(b) as may be required by law, a court of competent jurisdiction or any governmental or\nregulatory authority; and\n(c) if such information is public knowledge or already known to the Recipient at the time of\ndisclosure or subsequently becomes public knowledge other than by breach of any\nduty of confidentiality(contractual or otherwise).\n8\n53868398-1\n8.3 This condition 0 shall survive termination of the Licence.\n9.Data Protection\n9.1 The Supplier acknowledges and agrees that for the purposes of these Conditions the Supplier\nis the data controller, unless, as set out in the Privacy Policy, the Fleet Manager is the data\ncontroller.\n9.2 The Supplier shall comply with the provisions of the Privacy Policy in its capacity as data\ncontroller and with the terms of the contract between the Fleet Manager and the Supplier in\ncircumstances where the Fleet Manager is the data controller and the Supplier is the data\nprocessor.\n9.3 For the purpose of this condition 9,\n(a) the terms personal data, data controller, data processor and process have the\nmeanings given to them in the Data Protection Act 1998.\n10.Termination\n10.1 Without limiting its other rights or remedies, the Driver may terminate the Licence with\nimmediate effect by deleting the Driver App.\n10.2 Without limiting its other rights or remedies, the Supplier may suspend or terminate the Licence\nwith immediate effect:\n(a) by giving written notice to the Driver if the Driver commits a material breach of any term\nof these Conditions and (if such a breach is remediable) fails to remedy that breach\nwithin 5 Business Days of being notified in writing to do so;\n(b) where the contract between the Fleet Manager and the Supplier is suspended or\nterminated.\n10.3 The Driver acknowledges that because the Driver App and the Services are provided on a no\ncost basis, the Supplier may need to withdraw provision of the Driver App at any time where\nthe Supplier, believes, in its sole discretion, that it is commercially necessary to suspend or\ndiscontinue the Services. Accordingly the Driver acknowledges and agrees that the Supplier\nmay therefore suspend or terminate the Licence with immediate effect.\n10.4 On expiry or termination of the Licence for any reason:\n(a) the parties' rights and remedies that have accrued as at termination shall be unaffected;\nand\n(b) conditions that expressly or by implication survive termination of the Licence shall\ncontinue in full force and effect;\n(c) the Driver App shall be switched off by the Supplier within 5 Business Days of the date\nof notice of termination or, if later, the date of termination.\n9\n53868398-1\n11. Force majeure\nNeither party shall be liable for any failure or delay in performing its obligations under the\nLicence and these Conditions to the extent that such failure or delay is caused by a Force\nMajeure Event. A Force Majeure Event means any event which hinders, delays or prevents\nperformance of a party's obligations and which is either beyond that party's reasonable control,\nwhich by its nature could not have been foreseen, or, if it could have been foreseen, was\nunavoidable, including strikes, lock-outs or other industrial disputes (whether involving its own\nworkforce or a third party's), failure or interruption of energy sources, other utility service or\ntransport network, acts of God, war, threat of or preparation for war, armed conflict, terrorism,\nriot, civil commotion, interference by civil or military authorities, sanctions, embargo, export or\nimport restriction, quota or prohibition, breaking off of diplomatic relations, national or\ninternational calamity, malicious damage, breakdown of plant or machinery, nuclear, chemical\nor biological contamination, sonic boom, explosion, collapse of building structures, fire, flood,\ndrought, storm, earthquake, volcanic eruption, loss at sea, epidemic, pandemic or similar\nevents, natural disasters or extreme adverse weather conditions, or default of suppliers or\nsubcontractors.\n12. General\n12.1 Assignment subcontracting and other dealings\n(a) The Supplier may at any time assign, transfer, mortgage, charge, declare a trust over,\nsubcontract, delegate or deal in any other manner with any or all of its rights or\nobligations under the Licence.\n(b) The Driver shall not assign, transfer, mortgage, charge, declare a trust over,\nsubcontract, delegate or deal in any other manner with any or all of its rights or\nobligations under the Licence without the Supplier's written consent.\n12.2 Entire agreement The Licence (incorporating these Conditions) constitutes the entire\nagreement between the parties and supersedes and extinguishes all previous agreements,\npromises, assurances, warranties, representations and understandings between them, whether\nwritten or oral, relating to its subject matter. The Driver acknowledges that it has not relied on\nany statement, promise, representation, assurance or warranty made or given by or on behalf\nof the Supplier which is not set out in these Conditions.\n12.3 Variation Except as set out in these Conditions, no variation of the Licence, including the\nintroduction of any additional terms and conditions, shall be effective unless it is agreed in\nwriting and signed by the Supplier.\n12.4 Waiver Except as set out in condition 2.3, no failure or delay by a party to exercise any right or\nremedy provided under these Conditions or by law shall constitute a waiver of or prevent or\nrestrict the further exercise of that or any other right or remedy. No single or partial exercise of\nsuch right or remedy shall prevent or restrict the further exercise of that or any other right or\nremedy.\n12.5 Severance If any provision of these Conditions is or becomes invalid, illegal or unenforceable,\nit shall be deemed modified to the minimum extent necessary to make it valid, legal and \n10\n53868398-1\nenforceable. If such modification is not possible, the relevant provision shall be deemed\ndeleted. Any modification or deletion of a provision under this condition shall not affect the\nvalidity and enforceability of the rest of the Conditions.\n12.6 Notices\n(a) Any notice given to a party under or in connection with the Conditions shall be in writing\nand shall be delivered by hand or by pre-paid first-class post or by a signed-for next\nworking day delivery service at its registered office (if a company) or its principal place\nof business (in any other case).\n(b) Any notice shall be deemed to have been received: (i) if delivered by hand, on signature\nof a delivery receipt or, if not signed for, at the time the notice is left at the correct\naddress; (ii) if sent by pre-paid first-class post, at 09:00 on the second Business Day\nafter posting; and (iii) if sent by a signed-for next working day delivery service, at the\ntime recorded by the delivery service.\n(c) This condition does not apply to the service of any proceedings or other documents in\nany legal action or, where applicable, any arbitration or other method of dispute\nresolution.\n(d) A notice given under the Conditions is not valid if sent by email.\n12.7 Third party rights No one other than a party to these Conditions and their permitted assignees\nshall have any right to enforce any of its terms.\n12.8 Governing law The Licence and these Conditions and any dispute or claim arising out of or in\nconnection with it or its subject matter or formation (including non-contractual disputes or\nclaims) shall be governed by, and construed in accordance with, the law of England and Wales.\n12.9 Jurisdiction Each party irrevocably agrees that the courts of England and Wales shall have\nexclusive jurisdiction to settle any dispute or claim arising out of or in connection with the\nLicence and these Conditions or their subject matter or formation (including non-contractual\ndisputes or claims).\n";
    }
}
