using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SpaceCtrl.Data.Helpers
{
    public static class JsonSerializer
    {
        public static string ToJson<TObject>(this TObject @object, bool startWithLowerCase = true)
            where TObject : class => Serialize(@object, startWithLowerCase);

        public static string Serialize<TObject>(TObject @object, bool startWithLowerCase = true) where TObject : class
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            if (!startWithLowerCase)
                serializerSettings.ContractResolver = new DefaultContractResolver();

            return JsonConvert.SerializeObject(@object, serializerSettings);
        }

        public static TType Deserialize<TType>(string json) where TType : class => JsonConvert.DeserializeObject<TType>(json);
    }
}