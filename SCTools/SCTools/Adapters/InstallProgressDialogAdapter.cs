using NSW.StarCitizen.Tools.Forms;
using NSW.StarCitizen.Tools.Properties;

namespace NSW.StarCitizen.Tools.Adapters
{
    public class InstallProgressDialogAdapter
    {
        private readonly IProgressDialog _progressDialog;

        public InstallProgressDialogAdapter(IProgressDialog progressDialog)
        {
            _progressDialog = progressDialog;
            _progressDialog.CurrentTaskName = Resources.Localization_Installing_Text;
            _progressDialog.CurrentTaskInfo = string.Empty;
            _progressDialog.CurrentTaskProgress = 0;
            _progressDialog.UserCancellable = false;
        }
    }
}
