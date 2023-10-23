using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NSW.StarCitizen.Tools.Repositories;

namespace NSW.StarCitizen.Tools.Extensions
{
    public static class StreamExtensions
    {
        public static async Task CopyToAsync(this Stream source, Stream destination, int bufferSize, IDownloadProgress? downloadProgress, CancellationToken cancellationToken)
        {
            if (downloadProgress == null)
            {
                await source.CopyToAsync(destination, bufferSize, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                var buffer = new byte[bufferSize];
                int bytesRead;
                long totalRead = 0;
                IProgress<long> progress = new Progress<long>(downloadProgress.Downloaded);
                while ((bytesRead = await source.ReadAsync(buffer, 0, bufferSize, cancellationToken).ConfigureAwait(false)) > 0)
                {
                    await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);
                    cancellationToken.ThrowIfCancellationRequested();
                    totalRead += bytesRead;
                    progress.Report(totalRead);
                }
            }
        }
    }
}
