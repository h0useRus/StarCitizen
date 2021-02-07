using NSW.StarCitizen.Tools.Forms;
using NSW.StarCitizen.Tools.Lib.Update;
using NSW.StarCitizen.Tools.Properties;

namespace NSW.StarCitizen.Tools.Adapters
{
    public class DownloadProgressDialogAdapter : IProgressDialog.IAdapter, IDownloadProgress
    {
        private readonly string? _localizationVersion;
        private IProgressDialog? _progressDialog;
        private long _totalContentSize;
        private long _downloadedSize;

        public DownloadProgressDialogAdapter(string? localizationVersion)
        {
            _localizationVersion = localizationVersion;
        }

        public void Bind(IProgressDialog dialog)
        {
            _progressDialog = dialog;
            dialog.CurrentTaskInfo = string.Empty;
            dialog.CurrentTaskProgress = 0;
            dialog.UserCancellable = true;
            UpdateLocalization(dialog);
        }

        public void Unbind(IProgressDialog dialog) => _progressDialog = null;

        public void UpdateLocalization(IProgressDialog dialog)
        {
            if (_localizationVersion != null)
                dialog.Text = string.Format(Resources.Localization_InstallVersion_Title, _localizationVersion);
            dialog.CurrentTaskName = Resources.Localization_Downloading_Text;
            dialog.UserCancelText = Resources.Localization_Cancel_Text;
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
            if (_progressDialog == null) return;
            float downloadSizeMBytes = (float)_downloadedSize / (1024 * 1024);
            if (_totalContentSize > 0)
            {
                _progressDialog.CurrentTaskProgress = (float)_downloadedSize / _totalContentSize;
                float contentSizeMBytes = (float)_totalContentSize / (1024 * 1024);
                _progressDialog.CurrentTaskInfo = $"{downloadSizeMBytes:0.00} MB/{contentSizeMBytes:0.00} MB";
            }
            else
            {
                _progressDialog.CurrentTaskInfo = $"{downloadSizeMBytes:0.00} MB";
            }
        }
    }
}
