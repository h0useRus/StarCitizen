using System;

namespace NSW.StarCitizen.Tools.API.Attributes
{
    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
    internal class ConfigurationKeyAttribute : Attribute
    {
        public string ConfigurationKey { get; }

        public ConfigurationKeyAttribute(string configurationKey)
        {
            ConfigurationKey = configurationKey;
        }
    }
}
