using NSW.StarCitizen.Tools.Forms;
using NSW.StarCitizen.Tools.Properties;
using NSW.StarCitizen.Tools.Update;

namespace NSW.StarCitizen.Tools.Adapters
{
    public class DownloadProgressDialogAdapter : IDownloadProgress
    {
        private readonly IProgressDialog _progressDialog;
        private long _totalContentSize = 0;
        private long _downloadedSize = 0;

        public DownloadProgressDialogAdapter(IProgressDialog progressDialog)
        {
            _progressDialog = progressDialog;
            _progressDialog.CurrentTaskName = Resources.Localization_Downloading_Text;
            _progressDialog.CurrentTaskInfo = string.Empty;
            _progressDialog.CurrentTaskProgress = 0;
            _progressDialog.UserCancellable = true;
            _progressDialog.UserCancelText = Resources.Localization_Cancel_Text;
        }

        public void ReportContentSize(long value)
        {
            _totalContentSize = value;
            UpdateDialogTaskInfo();
        }

        public void ReportDownloadedSize(long value)
        {
            _downloadedSize = value;
            UpdateDialogTaskInfo();
        }

        private void UpdateDialogTaskInfo()
        {
            float downloadSizeMBytes = (float)_downloadedSize / (1024 * 1024);
            if (_totalContentSize > 0)
            {
                _progressDialog.CurrentTaskProgress = (float)_downloadedSize / _totalContentSize;
                float contentSizeMBytes = (float)_totalContentSize / (1024 * 1024);
                _progressDialog.CurrentTaskInfo = string.Format("{0:0.00} MB/{1:0.00} MB", downloadSizeMBytes, contentSizeMBytes);
            }
            else
            {
                _progressDialog.CurrentTaskInfo = string.Format("{0:0.00} MB", downloadSizeMBytes);
            }
        }
    }
}
