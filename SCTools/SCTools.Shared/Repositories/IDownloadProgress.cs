namespace NSW.StarCitizen.Tools.Repositories
{
    public interface IDownloadProgress
    {
        /// <summary>
        /// Reports a content download size in bytes.
        /// </summary>
        /// <param name="value">The value of the content size.</param>
        void SetContentSize(long value);
        /// <summary>
        /// Reports a downloaded size in bytes.
        /// </summary>
        /// <param name="value">The value of the download progress.</param>
        void SetDownloadedSize(long value);
    }
}
