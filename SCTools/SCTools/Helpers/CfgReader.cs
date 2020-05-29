using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace NSW.StarCitizen.Tools.Helpers
{
    public class CfgReader
    {
        private readonly string _fileName;
        public CfgReader(string fileName)
        {
            _fileName = fileName;
        }

        public Dictionary<string, string> ReadKeys()
        {
            var result = new Dictionary<string, string>();
            if (!File.Exists(_fileName))
                return result;

            try
            {
                using var reader = File.OpenText(_fileName);
                var line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        if (line.StartsWith("--"))
                            continue;
                        var parts = line.Split('=');
                        if (parts.Length == 1)
                            continue;

                        result.Add(parts[0].Trim(), parts[1].Trim());
                    }
                }
            }
            catch { }

            return result;
        }

        public async Task<bool> WriteKeysAsync(Dictionary<string, string> keys)
        {
            if (!File.Exists(_fileName))
                return false;

            try
            {
                using var writer = File.CreateText(_fileName);
                foreach (var key in keys)
                {
                    await writer.WriteLineAsync($"{key.Key}={key.Value}");
                }

                await writer.FlushAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateKeyAsync(string key, string value)
        {
            var keys = ReadKeys();
            if (keys.ContainsKey(key) && string.Compare(keys[key], value,StringComparison.OrdinalIgnoreCase)!=0)
            {
                keys[key] = value;
                return await WriteKeysAsync(keys);
            }

            return false;
        }
    }
}