using NSW.StarCitizen.Tools.Forms;
using NSW.StarCitizen.Tools.Properties;

namespace NSW.StarCitizen.Tools.Adapters
{
    public sealed class InstallProgressDialogAdapter : IProgressDialog.IAdapter
    {
        public void Bind(IProgressDialog dialog)
        {
            dialog.CurrentTaskInfo = string.Empty;
            dialog.CurrentTaskProgress = 0;
            dialog.UserCancellable = false;
            UpdateLocalization(dialog);
        }

        public void Unbind(IProgressDialog dialog) { }

        public void UpdateLocalization(IProgressDialog dialog) => dialog.CurrentTaskName = Resources.Localization_Installing_Text;
    }
}
