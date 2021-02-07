using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace NSW.StarCitizen.Tools.Lib.Helpers
{
    public static class JsonHelper
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly JsonSerializerSettings _jsonSettings = GetJsonSettings();
        private static JsonSerializerSettings GetJsonSettings()
        {
            var settings = new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore};
            settings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
            return settings;
        }
        public static T? Read<T>(string json) where T : class => JsonConvert.DeserializeObject<T>(json, _jsonSettings);

        public static string Write(object obj, Formatting formatting = Formatting.Indented) => JsonConvert.SerializeObject(obj, formatting, _jsonSettings);

        public static T? ReadFile<T>(string filePath) where T : class
        {
            if (File.Exists(filePath))
            {
                try
                {
                    return Read<T>(File.ReadAllText(filePath));
                }
                catch (Exception e)
                {
                    _logger.Error(e, $"Error read json file: {filePath}");
                }
            }
            return default;
        }

        public static bool WriteFile(string filePath, object obj)
        {
            try
            {
                File.WriteAllText(filePath, Write(obj));
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Error write json file: {filePath}");
                return false;
            }
        }
    }
}