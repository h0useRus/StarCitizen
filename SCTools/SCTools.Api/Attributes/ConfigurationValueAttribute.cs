using System;

namespace NSW.StarCitizen.Tools.API.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    internal class ConfigurationValueAttribute : Attribute
    {
        public string ConfigurationValue { get; }

        public ConfigurationValueAttribute(string configurationValue)
        {
            ConfigurationValue = configurationValue;
        }
    }
}
