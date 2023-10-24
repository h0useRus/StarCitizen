using System;

namespace NSW.StarCitizen.Tools.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class ConfigurationFileAttribute : Attribute
    {
        public string FileName { get; }

        public ConfigurationFileAttribute(string fileName)
        {
            FileName = fileName;
        }
    }
}
