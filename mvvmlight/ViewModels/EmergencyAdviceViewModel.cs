using System;
using mvvmframework.Interfaces;
using mvvmframework.Languages;

namespace mvvmframework.ViewModels
{
    public class EmergencyAdviceViewModel : BaseViewModel
    {
        IEmailSms emailSMSService;
        IUserSettings userService;

        public EmergencyAdviceViewModel(IEmailSms emsos, IUserSettings user)
        {
            emailSMSService = emsos;
            userService = user;
        }

        public void SendSMS()
        {
            var msgFromLang = Langs.Const_Msg_SOS_1;
            var splitLine = msgFromLang.Split(new string[] { "@%" }, StringSplitOptions.None);
            var name = userService.LoadSetting<string>("RealName", SettingType.String);
            var smsNumber = Driver.SOSFleetManagerMobileNumber;
            var msg = $"{splitLine[0]} {name} {splitLine[1]} {Driver.OdoVehicleReg} {splitLine[2]} {DateTime.Now.ToString("R")}{splitLine[3]}";
            emailSMSService.SendSOSSMS(msg, smsNumber);
        }
    }
}
