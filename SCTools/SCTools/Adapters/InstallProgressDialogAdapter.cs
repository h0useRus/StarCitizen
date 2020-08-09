using NSW.StarCitizen.Tools.Forms;

namespace NSW.StarCitizen.Tools.Adapters
{
    public class InstallProgressDialogAdapter
    {
        private readonly IProgressDialog _progressDialog;

        public InstallProgressDialogAdapter(IProgressDialog progressDialog)
        {
            _progressDialog = progressDialog;
            _progressDialog.CurrentTaskName = "Installing...";
            _progressDialog.CurrentTaskInfo = "";
            _progressDialog.CurrentTaskProgress = 0;
            _progressDialog.UserCancellable = false;
        }
    }
}
