using System;
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

        public static TType? DeserializeToObject<TType>(this string json, bool throwException = true)
            where TType : class => Deserialize<TType>(json, throwException);

        public static TType? Deserialize<TType>(string? json, bool throwException = true) where TType : class
        {
            if (string.IsNullOrEmpty(json))
            {
                if (throwException)
                    throw new InvalidOperationException($"{nameof(json)} value can't be empty");

                return null;
            }

            try
            {
                var result = JsonConvert.DeserializeObject<TType>(json);
                return result;
            }
            catch
            {
                if (throwException)
                    throw;

                return null;
            }
        }
    }
}