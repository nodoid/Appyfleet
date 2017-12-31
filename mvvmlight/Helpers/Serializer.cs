using System;
using Newtonsoft.Json;

namespace mvvmframework.Helpers
{
    public class Serializer
    {
        public static string SerializeToString<T>(T model)
        {
            return JsonConvert.SerializeObject(model);
        }

        public static T DeserializeFromString<T>(string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj);
        }
    }
}
