using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace NSW.StarCitizen.Tools.Lib.Helpers
{
    public static class HashAlgorithmExtensions
    {
        private const int BufferSize = 4096;

        public static async Task<byte[]> ComputeHashAsync(this HashAlgorithm hash, Stream inputStream,
            CancellationToken? cancellationToken = default)
        {
            hash.Initialize();
            var buffer = new byte[BufferSize];
            var streamLength = inputStream.Length;
            while (true)
            {
                cancellationToken?.ThrowIfCancellationRequested();
                var read = await inputStream.ReadAsync(buffer, 0, BufferSize).ConfigureAwait(false);
                if (inputStream.Position == streamLength)
                {
                    hash.TransformFinalBlock(buffer, 0, read);
                    break;
                }
                hash.TransformBlock(buffer, 0, read, default, default);
            }
            return hash.Hash;
        }
    }
}
