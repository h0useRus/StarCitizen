using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace NSW.StarCitizen.Tools.Lib.Helpers
{
    public abstract class CfgRow {}

    public sealed class CfgDataRow : CfgRow
    {
        public string Key { get; }
        public string Value { get; private set; }

        public static CfgDataRow? Create(string key, string value) =>
            !string.IsNullOrWhiteSpace(key) ? new CfgDataRow(key, value) : null;

        private CfgDataRow(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public void UpdateValue(string value) => Value = value;

        public override string ToString() => Key + "=" + Value;

        public override bool Equals(object value) =>
            ReferenceEquals(this, value) || (value is CfgDataRow dataRow &&
            string.Compare(Key, dataRow.Key, StringComparison.OrdinalIgnoreCase) == 0 && Value == dataRow.Value);

        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Key);
    }

    public sealed class CfgTextRow : CfgRow
    {
        public static CfgTextRow Empty { get; } = new CfgTextRow(string.Empty);

        public static CfgTextRow Create(string content) => string.IsNullOrWhiteSpace(content) ? Empty : new CfgTextRow(content);

        public string Content { get; }

        private CfgTextRow(string content)
        {
            Content = content;
        }

        public override string ToString() => Content;

        public override bool Equals(object value) =>
            ReferenceEquals(this, value) || (value is CfgTextRow textRow && Content == textRow.Content);

        public override int GetHashCode() => Content.GetHashCode();
    }

    public class CfgData : IEnumerable<CfgRow>
    {
        private readonly List<CfgRow> _rows = new List<CfgRow>();

        public string? this[string key] => GetRowByKey(key)?.Value;
        public IEnumerator<CfgRow> GetEnumerator() => _rows.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public CfgData() {}

        public CfgData(int capacity)
        {
            _rows = new List<CfgRow>(capacity);
        }

        public CfgData(IEnumerable<CfgRow> collection)
        {
            _rows = new List<CfgRow>(collection);
        }

        public CfgData(string content)
        {
            string? line;
            using var reader = new StringReader(content);
            while ((line = reader.ReadLine()) != null)
            {
                AddRow(line);
            }
        }

        public void Clear() => _rows.Clear();

        public bool AddRow(string original)
        {
            if (IsTextString(original))
            {
                _rows.Add(CfgTextRow.Create(original));
                return true;
            }
            int pos = original.IndexOf('=');
            if (pos < 0)
            {
                _rows.Add(CfgTextRow.Create(original));
                return true;
            }
            string key = original.Substring(0, pos).Trim();
            string value = original.Substring(pos + 1).Trim();
            var row = CfgDataRow.Create(key, value);
            if (row == null)
            {
                _rows.Add(CfgTextRow.Create(original));
                return true;
            }
            if (ContainsKey(key))
                return false;
            _rows.Add(row);
            return true;
        }

        public CfgDataRow? AddOrUpdateRow(string key, string value)
        {
            var row = GetRowByKey(key);
            if (row != null)
            {
                row.UpdateValue(value);
                return row;
            }
            var addRow = CfgDataRow.Create(key, value);
            if (addRow != null)
            {
                _rows.Add(addRow);
                return addRow;
            }
            return null;
        }

        public CfgDataRow? RemoveRow(string key)
        {
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

        public IReadOnlyDictionary<string, string> ToDictionary()
            => _rows.OfType<CfgDataRow>().ToDictionary(row => row.Key, row => row.Value, StringComparer.OrdinalIgnoreCase);

        public IReadOnlyDictionary<string, CfgDataRow> ToRowDictionary()
            => _rows.OfType<CfgDataRow>().ToDictionary(row => row.Key, row => row, StringComparer.OrdinalIgnoreCase);

        private CfgDataRow? GetRowByKey(string key)
        {
            if (!string.IsNullOrWhiteSpace(key))
                return _rows.OfType<CfgDataRow>().SingleOrDefault(r => string.Compare(key, r.Key, StringComparison.OrdinalIgnoreCase) == 0);
            return null;
        }

        private bool ContainsKey(string key) =>
            !string.IsNullOrWhiteSpace(key) &&
            _rows.OfType<CfgDataRow>().Any(r => string.Compare(key, r.Key, StringComparison.OrdinalIgnoreCase) == 0);

        private static bool IsTextString(string original) =>
            string.IsNullOrWhiteSpace(original) ||
            original.StartsWith("--") ||
            original.StartsWith("//");

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var row in this)
            {
                builder.AppendLine(row.ToString());
            }
            return builder.ToString();
        }
    }

    public class CfgFile
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly string _fileName;

        public CfgFile(string fileName)
        {
            _fileName = fileName;
        }

        public async Task<CfgData> ReadAsync()
        {
            var data = new CfgData();
            if (!File.Exists(_fileName))
                return data;

            try
            {
                using var reader = File.OpenText(_fileName);
                string line;
                while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
                {
                    data.AddRow(line);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Failed read file: {_fileName}");
            }
            return data;
        }

        public CfgData Read()
        {
            var data = new CfgData();
            if (!File.Exists(_fileName))
                return data;

            try
            {
                string? line;
                using var reader = File.OpenText(_fileName);
                while ((line = reader.ReadLine()) != null)
                {
                    data.AddRow(line);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Failed read file: {_fileName}");
            }
            return data;
        }

        public async Task<bool> SaveAsync(CfgData data)
        {
            try
            {
                using var writer = File.CreateText(_fileName);
                foreach (var row in data)
                {
                    await writer.WriteLineAsync(row.ToString()).ConfigureAwait(false);
                }
                await writer.FlushAsync().ConfigureAwait(false);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Failed save file: {_fileName}");
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
                    writer.WriteLine(row.ToString());
                }
                writer.Flush();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Failed save file: {_fileName}");
                return false;
            }
        }
    }
}