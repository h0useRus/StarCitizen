using System.IO;
using NSW.StarCitizen.Tools.Helpers;

namespace NSW.StarCitizen.Tools
{
    public class PatchIndexes
    {
        public long OriginalIndex { get; set; } = -1;
        public long PatchIndex { get; set; } = -1;
    }

    public class Patcher
    {
        public readonly string _fileName;
        public readonly byte[] _originalPattern;
        public readonly byte[] _patchPattern;

        public Patcher(string fileName, byte[] originalPattern, byte[] patchPattern)
        {
            _fileName = fileName;
            _originalPattern = originalPattern;
            _patchPattern = patchPattern;
        }

        public PatchIndexes Find()
        {
            var result = new PatchIndexes();
            using var stream = File.OpenRead(_fileName);
            result.OriginalIndex = StreamHelper.IndexOf(stream, _originalPattern);
            stream.Seek(0, SeekOrigin.Begin);
            result.PatchIndex = StreamHelper.IndexOf(stream, _patchPattern);

            return result;
        }

        public bool Patch(PatchIndexes patch)
        {
            if (patch.OriginalIndex > 0)
            {
                StreamHelper.UpdateFile(_fileName, patch.OriginalIndex, _patchPattern);
                return true;
            }

            if (patch.PatchIndex > 0)
            {
                StreamHelper.UpdateFile(_fileName, patch.PatchIndex, _originalPattern);
            }

            return false;
        }
    }
}