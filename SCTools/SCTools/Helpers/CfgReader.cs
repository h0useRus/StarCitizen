using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NSW.StarCitizen.Tools.Helpers
{
    public class CfgRow
    {
        public string Original { get; private set; }
        public string? Key { get; }
        public string? Value { get; private set; }
        public bool HasData => !string.IsNullOrWhiteSpace(Key);
        public bool IsChanged { get; private set; }

        public CfgRow(string original)
        {
            Original = original;
            if (IsDataString(original))
            {
                var pos = original.IndexOf('=');
                Key = original.Substring(0, pos).Trim();
                Value = original.Substring(pos + 1).Trim();
            }
        }

        public CfgRow(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

            Key = key;
            Value = value;
            Original = key + "=" + value;
        }

        private static bool IsDataString(string original)
            => !string.IsNullOrEmpty(original)
               && !original.StartsWith("--")
               && !original.StartsWith("//")
               && original.Contains("=");

        public void UpdateValue(string newValue)
        {
            if (HasData && string.CompareOrdinal(Original, newValue) != 0)
            {
                Original = Original.Replace(Value, newValue);
                Value = newValue;
                IsChanged = true;
            }
        }

        public override string ToString() => Original;
    }

    public class CfgData : IEnumerable<CfgRow>
    {
        private readonly List<CfgRow> _rows = new List<CfgRow>();
        private readonly CfgFile _reader;

        public string? this[string key] => GetRowByKey(key)?.Value;
        public IEnumerator<CfgRow> GetEnumerator() => _rows.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public CfgData(CfgFile reader)
        {
            _reader = reader;
        }

        public bool AddRow(string original)
        {
            var row = new CfgRow(original);
            if (row.HasData && ContainsKey(row.Key))
                return false;
            _rows.Add(row);
            return true;
        }

        public CfgRow AddOrUpdateRow(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

            var row = GetRowByKey(key);
            if (row == null)
            {
                row = new CfgRow(key, value);
                _rows.Add(row);
                return row;
            }

            row.UpdateValue(value);
            return row;
        }

        public CfgRow? RemoveRow(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            var row = GetRowByKey(key);
            if (row != null)
            {
                _rows.Remove(row);
                return row;
            }
            return null;
        }

        public bool TryGetValue(string key, out string? value)
        {
            var row = GetRowByKey(key);
            if (row != null)
            {
                value = row.Value;
                return true;
            }

            value = null;
            return false;
        }

        public Task<bool> SaveAsync() => _reader.SaveAsync(this);
        public bool Save() => _reader.Save(this);

        public IReadOnlyDictionary<string, string> ToDictionary()
            => _rows.Where(r => r.HasData).ToDictionary(row => row.Key, row => row.Value);

        private CfgRow GetRowByKey(string key)
            => _rows.SingleOrDefault(r => r.HasData && string.CompareOrdinal(key, r.Key) == 0);

        private bool ContainsKey(string key)
            => _rows.Any(r => string.CompareOrdinal(key, r.Key) == 0);
    }


    public class CfgFile
    {
        private readonly string _fileName;
        public CfgFile(string fileName)
        {
            _fileName = fileName;
        }

        public async Task<CfgData> ReadAsync()
        {
            var data = new CfgData(this);
            if (!File.Exists(_fileName))
                return data;

            try
            {
                using var reader = File.OpenText(_fileName);
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    data.AddRow(line);
                }
            }
            catch { }

            return data;
        }

        public CfgData Read()
        {
            var data = new CfgData(this);
            if (!File.Exists(_fileName))
                return data;

            try
            {
                using var reader = File.OpenText(_fileName);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    data.AddRow(line);
                }
            }
            catch { }

            return data;
        }

        public async Task<bool> SaveAsync(CfgData data)
        {
            try
            {
                using var writer = File.CreateText(_fileName);
                foreach (var row in data)
                {
                    await writer.WriteLineAsync(row.Original);
                }
                await writer.FlushAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Save(CfgData data)
        {
            try
            {
                using var writer = File.CreateText(_fileName);
                foreach (var row in data)
                {
                    writer.WriteLine(row.Original);
                }
                writer.Flush();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}