namespace NSW.StarCitizen.Tools.Repositories
{
    public abstract class DownloadResult
    {
        public static DownloadResult FromArchivePath(string archiveFilePath)
            => new FullDownoadResult(archiveFilePath);
    }

    public sealed class FullDownoadResult : DownloadResult
    {
        public string ArchiveFilePath { get; }

        public FullDownoadResult(string archiveFilePath)
        {
            ArchiveFilePath = archiveFilePath;
        }
    }
}
