namespace NSW.StarCitizen.Tools.Lib.Update
{
    public interface IDownloadProgress
    {
        //
        // Summary:
        //     Reports a content download size in bytes.
        //
        // Parameters:
        //   value:
        //     The value of the content size.
        void ReportContentSize(long value);
        //
        // Summary:
        //     Reports a downloaded size in bytes.
        //
        // Parameters:
        //   value:
        //     The value of the download progress.
        void ReportDownloadedSize(long value);
    }
}
