using System;
using System.Collections.Generic;

namespace mvvmframework
{
    public interface ILogFileService
    {
        bool WriteLog(string area, string log);
        bool SendLogFilesForDate(DateTime date);
        List<DBLogData> LoadDataSet();
    }
}
