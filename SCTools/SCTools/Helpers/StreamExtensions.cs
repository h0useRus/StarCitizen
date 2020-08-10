using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NSW.StarCitizen.Tools.Helpers
{
    public static class StreamExtensions
    {
        public static async Task CopyToAsync(this Stream source, Stream destination, int bufferSize,
            CancellationToken cancellationToken, IProgress<long> progress)
        {
            var buffer = new byte[bufferSize];
            int bytesRead;
            long totalRead = 0;
            while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
            {
                await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                totalRead += bytesRead;
                progress.Report(totalRead);
            }
        }
    }
}
