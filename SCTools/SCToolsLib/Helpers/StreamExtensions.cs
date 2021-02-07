using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NSW.StarCitizen.Tools.Lib.Helpers
{
    public static class StreamExtensions
    {
        public static async Task CopyToAsync(this Stream source, Stream destination, int bufferSize,
            CancellationToken cancellationToken, IProgress<long> progress)
        {
            var buffer = new byte[bufferSize];
            int bytesRead;
            long totalRead = 0;
            while ((bytesRead = await source.ReadAsync(buffer, 0, bufferSize, cancellationToken)
                .ConfigureAwait(false)) > 0)
            {
                await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken)
                    .ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();
                totalRead += bytesRead;
                progress.Report(totalRead);
            }
        }
    }
}
