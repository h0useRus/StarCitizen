using NSW.StarCitizen.Tools.Forms;
using NSW.StarCitizen.Tools.Properties;

namespace NSW.StarCitizen.Tools.Adapters
{
    public class UninstallProgressDialogAdapter
    {
        private readonly IProgressDialog _progressDialog;

        public UninstallProgressDialogAdapter(IProgressDialog progressDialog)
        {
            _progressDialog = progressDialog;
            _progressDialog.CurrentTaskName = Resources.Localization_Uninstalling_Text;
            _progressDialog.CurrentTaskInfo = string.Empty;
            _progressDialog.CurrentTaskProgress = 0;
            _progressDialog.UserCancellable = false;
        }
    }
}
