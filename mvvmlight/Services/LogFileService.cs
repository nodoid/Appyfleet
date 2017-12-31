using System;
using System.Collections.Generic;
using System.Linq;
using mvvmframework.Interfaces;
using Newtonsoft.Json;
using GalaSoft.MvvmLight.Ioc;

namespace mvvmframework
{
    public class LogFileService : ILogFileService
    {
        IRepository repoService { get => SimpleIoc.Default.GetInstance<IRepository>(); }
        IUserSettings settingService { get => SimpleIoc.Default.GetInstance<IUserSettings>(); }
        ICultureInfo cultureInfo { get => SimpleIoc.Default.GetInstance<ICultureInfo>(); }
        IEmailSms emailService { get => SimpleIoc.Default.GetInstance<IEmailSms>(); }

        public bool WriteLog(string area, string log)
        {
            try
            {
                repoService.SaveData(new DBLogData { User = settingService.LoadSetting<string>("Username", SettingType.String), 
                    TimeStamp = DateTime.Now, AppArea = area, 
                    Data = log });
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<DBLogData> LoadDataSet()
        {
            return repoService.GetList<DBLogData>().OrderBy(t => t.DateIndex.Year).ThenBy(t=>t.DateIndex.Month).ThenBy(t=>t.DateIndex.Day).ToList();
        }

        public bool SendLogFilesForDate(DateTime date)
        {
            var filter = new DateTime(date.Year, date.Month, date.Day);
            var dbData = repoService.GetList<DBLogData>().Where(t => t.DateIndex == filter).ToList();

            var jSONString = JsonConvert.SerializeObject(dbData);

            return emailService.SendEmailFile(Constants.LogFileEmail, "(" + date.ToLocalizedString("dd/MM/yy", cultureInfo.currentCulture) + ") Log file for " + settingService.LoadSetting<string>("Username", SettingType.String), jSONString);
        }
    }
}
