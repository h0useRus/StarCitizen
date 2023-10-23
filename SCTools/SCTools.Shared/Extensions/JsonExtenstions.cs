using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace NSW.StarCitizen.Tools.Extensions
{
    public static class JsonExtenstions
    {
        private static readonly Lazy<JsonSerializerSettings> _jsonSettings = new(GetJsonSettings);
        private static JsonSerializerSettings GetJsonSettings()
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            settings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
            return settings;
        }

        public static T? FromJson<T>(this string json, JsonSerializerSettings? settings = default)
            => JsonConvert.DeserializeObject<T>(json, settings ?? _jsonSettings.Value);
    }
}
