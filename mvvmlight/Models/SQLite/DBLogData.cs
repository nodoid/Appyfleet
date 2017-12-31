using System;
using SQLite.Net.Attributes;

namespace mvvmframework
{
    public class DBLogData
    {
        [Indexed]
        public DateTime DateIndex { get; set; }

        public DateTime TimeStamp { get; set; }        
        public string User { get; set; }
        public string AppArea { get; set; }
        public string Data { get; set; }

        [Ignore]
        public string DateString => DateIndex.Date.ToString("dd.MM.yyyy");
        [Ignore]
        public string DateId => DateIndex.ToString();
    }
}
