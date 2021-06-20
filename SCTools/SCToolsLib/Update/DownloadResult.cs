namespace NSW.StarCitizen.Tools.Lib.Update
{
    public sealed class DownloadResult
    {
        public string? ArchiveFilePath { get; }
        public FilesIndex.DiffList? DiffList { get; }

        private DownloadResult(string archiveFilePath)
        {
            ArchiveFilePath = archiveFilePath;
        }

        private DownloadResult(FilesIndex.DiffList diffList)
        {
            DiffList = diffList;
        }

        public static DownloadResult FromArchivePath(string archiveFilePath) => new DownloadResult(archiveFilePath);

        public static DownloadResult FromFilesDiffList(FilesIndex.DiffList diffList) => new DownloadResult(diffList);
    }
}
