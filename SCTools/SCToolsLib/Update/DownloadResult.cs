namespace NSW.StarCitizen.Tools.Lib.Update
{
    public abstract class DownloadResult
    {
        public static DownloadResult FromArchivePath(string archiveFilePath)
            => new FullDownoadResult(archiveFilePath);

        public static DownloadResult FromFilesDiffList(string downloadPath, FilesIndex.DiffList diffList)
            => new IncrementalDownloadResult(downloadPath, diffList);
    }

    public sealed class FullDownoadResult : DownloadResult
    {
        public string ArchiveFilePath { get; }

        public FullDownoadResult(string archiveFilePath)
        {
            ArchiveFilePath = archiveFilePath;
        }
    }

    public sealed class IncrementalDownloadResult : DownloadResult
    {
        public string DownloadPath { get; } 
        public FilesIndex.DiffList DiffList { get; }

        public IncrementalDownloadResult(string downloadPath, FilesIndex.DiffList diffList)
        {
            DownloadPath = downloadPath;
            DiffList = diffList;
        }
    }
}
