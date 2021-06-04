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
        public sealed class DiffList
        {
            public ISet<string> ChangedFiles { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            public ISet<string> ReuseFiles { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            public ISet<string> RemoveFiles { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        public struct Record
        {
            public long Size;
            public byte[] Hash;

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

        private FilesIndex(Builder builder)
        {
            Index = builder.Index;
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
            while ((line = stream.ReadLine()) != null)
            {
                cancellationToken?.ThrowIfCancellationRequested();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var parts = line.Split(':');
                    if (parts.Length != 3)
                        throw new InvalidDataException();
                    Record record;
                    record.Size = long.Parse(parts[1]);
                    if (record.Size < 0)
                        throw new InvalidDataException();
                    record.Hash = Convert.FromBase64String(parts[2]);
                    builder.Index.Add(parts[0], record);
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
            public Dictionary<string, Record> Index { get; } = new Dictionary<string, Record>(StringComparer.OrdinalIgnoreCase);

            public FilesIndex Build() => new FilesIndex(this);
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
                Record record;
                record.Size = stream.Length;
                record.Hash = md5.ComputeHash(stream);
                Index.Add(relativeFilePath, record);
            }

            private void AddFileAtPath(FileInfo fileInfo, string? path = default, CancellationToken? cancellationToken = default)
            {
                var relativeFilePath = string.IsNullOrEmpty(path) ? fileInfo.Name : (path + Path.DirectorySeparatorChar + fileInfo.Name);
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
                    var relativeDirPath = string.IsNullOrEmpty(path) ? subDirInfo.Name : (path + Path.DirectorySeparatorChar + subDirInfo.Name);
                    AddDirectory(subDirInfo, relativeDirPath, cancellationToken);
                }
            }
        }
    }
}
