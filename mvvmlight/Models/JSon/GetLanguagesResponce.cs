using System.Collections.Generic;

namespace mvvmframework
{
    public class GetLanguagesResponse
    {
        public List<LanguageModel> Languages { get; set; }
        public StatusModel Status { get; set; }
    }
}
