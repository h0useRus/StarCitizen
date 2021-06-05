using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;

namespace NSW.StarCitizen.Tools.Lib.Update
{
    public sealed class FilesIndex
    {
        public static readonly char DirectorySeparatorChar = Path.DirectorySeparatorChar;

        public sealed class DiffList
        {
            public ISet<string> ChangedFiles { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            public ISet<string> ReuseFiles { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            public ISet<string> RemoveFiles { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        public struct Record
        {
            public long Size { get; }
            public byte[] Hash { get; }

            public Record(long size, byte[] hash)
            {
                if (size < 0)
                    throw new InvalidDataException($"Record file size is less than zero");
                if (hash == null)
                    throw new NullReferenceException("Record file hash is null");
                Size = size;
                Hash = hash;
            }

            public override bool Equals(object obj)
            {
                if (obj is Record other)
                {
                    return Size == other.Size && Hash.SequenceEqual(other.Hash);
                }
                return false;
            }

            public override int GetHashCode()
            {
                var hashCode = -303803187;
                hashCode = hashCode * -1521134295 + Size.GetHashCode();
                hashCode = hashCode * -1521134295 + EqualityComparer<byte[]>.Default.GetHashCode(Hash);
                return hashCode;
            }
        }

        public static FilesIndex Empty { get; } = new Builder().Build();

        public IReadOnlyDictionary<string, Record> Index { get; }

        private FilesIndex(Dictionary<string, Record> index)
        {
            Index = index;
        }

        public bool IsEmpty() => Index.Count == 0;

        public long GetFileSize(string fileName)
        {
            if (Index.TryGetValue(fileName, out var record))
            {
                return record.Size;
            }
            return 0;
        }

        public long GetFilesSize()
        {
            long result = 0;
            foreach (var pair in Index)
            {
                result += pair.Value.Size;
            }
            return result;
        }

        public long GetFilesSize(ISet<string> fileNames)
        {
            long result = 0;
            foreach (var fileName in fileNames)
            {
                result += GetFileSize(fileName);
            }
            return result;
        }

        public void WriteToFile(string filePath)
        {
            using var writer = File.CreateText(filePath);
            WriteToSteam(writer);
        }

        public void WriteToSteam(StreamWriter stream)
        {
            foreach (var rec in Index)
            {
                stream.WriteLine($"{rec.Key}:{rec.Value.Size}:{Convert.ToBase64String(rec.Value.Hash)}");
            }
        }

        public static FilesIndex LoadFromStream(StreamReader stream, CancellationToken? cancellationToken = default)
        {
            var builder = new Builder();
            string line;
            char[] delim = new char[] { ':' };
            while ((line = stream.ReadLine()) != null)
            {
                cancellationToken?.ThrowIfCancellationRequested();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var parts = line.Split(delim, 3);
                    if (parts.Length != 3)
                        throw new InvalidDataException($"Invalid number of record elements: {parts.Length}");
                    builder.Add(parts[0], new Record(long.Parse(parts[1]), Convert.FromBase64String(parts[2])));
                }
            }
            return builder.Build();
        }

        public static FilesIndex LoadFromFile(string filePath, CancellationToken? cancellationToken = default)
        {
            using var stream = File.OpenText(filePath);
            return LoadFromStream(stream, cancellationToken);
        }

        public static DiffList BuildDiffList(FilesIndex source, FilesIndex target)
        {
            var diffList = new DiffList();
            foreach (var targetInfo in target.Index)
            {
                if (source.Index.TryGetValue(targetInfo.Key, out var hash) &&
                    targetInfo.Value.Equals(hash))
                {
                    diffList.ReuseFiles.Add(targetInfo.Key);
                }
                else
                {
                    diffList.ChangedFiles.Add(targetInfo.Key);
                }
            }
            foreach (var sourceInfo in source.Index)
            {
                if (!target.Index.ContainsKey(sourceInfo.Key))
                {
                    diffList.RemoveFiles.Add(sourceInfo.Key);
                }
            }
            return diffList;
        }

        public class Builder
        {
            private readonly Dictionary<string, Record> _index = new Dictionary<string, Record>(StringComparer.OrdinalIgnoreCase);

            public void Add(string rawFilePath, Record record)
            {
                if (string.IsNullOrWhiteSpace(rawFilePath))
                    throw new InvalidDataException("File path is empty");
                var filePath = rawFilePath.Replace(Path.AltDirectorySeparatorChar, DirectorySeparatorChar);
                if (filePath.Contains(".."))
                    throw new InvalidDataException($"File path contains backward elements: {filePath}");
                _index.Add(filePath, record);
            }

            public bool Remove(string rawFilePath) => _index.Remove(rawFilePath.Replace(Path.AltDirectorySeparatorChar, DirectorySeparatorChar));

            public FilesIndex Build() => new FilesIndex(_index);
        }

        public sealed class HashBuilder : Builder
        {
            public bool AddFile(string path, string relativeFilePath, CancellationToken? cancellationToken = default)
            {
                var fileInfo = new FileInfo(path);
                if (fileInfo.Exists)
                {
                    AddFile(fileInfo, relativeFilePath, cancellationToken);
                    return true;
                }
                return false;
            }

            public bool AddDirectory(string path, string? pathPrefix, CancellationToken? cancellationToken = default)
            {
                var directoryInfo = new DirectoryInfo(path);
                if (directoryInfo.Exists)
                {
                    AddDirectory(directoryInfo, pathPrefix, cancellationToken);
                    return true;
                }
                return false;
            }

            private void AddFile(FileInfo fileInfo, string relativeFilePath, CancellationToken? cancellationToken = default)
            {
                using var md5 = MD5.Create();
                using var stream = File.Open(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
                cancellationToken?.ThrowIfCancellationRequested();
                Add(relativeFilePath, new Record(stream.Length, md5.ComputeHash(stream)));
            }

            private void AddFileAtPath(FileInfo fileInfo, string? path = default, CancellationToken? cancellationToken = default)
            {
                var relativeFilePath = string.IsNullOrEmpty(path) ? fileInfo.Name : (path + DirectorySeparatorChar + fileInfo.Name);
                AddFile(fileInfo, relativeFilePath, cancellationToken);
            }

            private void AddDirectory(DirectoryInfo directoryInfo, string? path = default, CancellationToken? cancellationToken = default)
            {
                foreach (var fileInfo in directoryInfo.EnumerateFiles())
                {
                    cancellationToken?.ThrowIfCancellationRequested();
                    AddFileAtPath(fileInfo, path, cancellationToken);
                }
                foreach (var subDirInfo in directoryInfo.EnumerateDirectories())
                {
                    cancellationToken?.ThrowIfCancellationRequested();
                    var relativeDirPath = string.IsNullOrEmpty(path) ? subDirInfo.Name : (path + DirectorySeparatorChar + subDirInfo.Name);
                    AddDirectory(subDirInfo, relativeDirPath, cancellationToken);
                }
            }
        }
    }
}
