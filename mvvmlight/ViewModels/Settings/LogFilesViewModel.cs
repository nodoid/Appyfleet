using System;
using System.Collections.Generic;

namespace mvvmframework.ViewModels.Settings
{
    public class LogFilesViewModel : SettingsViewModel
    {
        ILogFileService logService;

        public LogFilesViewModel(IWebSevices web, IUserSettings settings, IConnection con, ILogFileService log):base(settings, web, con)
        {
            logService = log;
        }

        public List<DBLogData> GetLogs => logService.LoadDataSet();

        DateTime sendLog;
        public DateTime SendLog
        {
            get { return sendLog; }
            set 
            { 
                Set(() => SendLog, ref sendLog, value); 
                logService.SendLogFilesForDate(value);
            }
        }
    }
}
