namespace NSW.StarCitizen.Tools.Lib.Update
{
    public struct DownloadResult
    {
        public string? ArchiveFilePath { get; set; }
        public FilesIndex.DiffList? DiffList { get; set; }
    }
}
