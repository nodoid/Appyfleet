using System.Collections.Generic;

namespace mvvmframework
{
    public class GetOdometerReadingsResponseJson
    {
        public StatusModel Status { get; set; }
        public List<OdoReadingModel> Readings { get; set; }
    }
}
