namespace NSW.StarCitizen.Tools.Repositories
{
    public interface IDownloadProgress
    {
        /// <summary>
        /// Set a content download size in bytes.
        /// </summary>
        /// <param name="value">The value of the content size.</param>
        void Total(long value);
        /// <summary>
        /// Set a downloaded size in bytes.
        /// </summary>
        /// <param name="value">The value of the download progress.</param>
        void Downloaded(long value);
    }
}
