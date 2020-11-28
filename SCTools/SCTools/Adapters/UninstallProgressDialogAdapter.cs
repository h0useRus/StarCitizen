using NSW.StarCitizen.Tools.Forms;
using NSW.StarCitizen.Tools.Properties;

namespace NSW.StarCitizen.Tools.Adapters
{
    public sealed class UninstallProgressDialogAdapter : IProgressDialog.IAdapter
    {
        public void Bind(IProgressDialog dialog)
        {
            dialog.CurrentTaskInfo = string.Empty;
            dialog.CurrentTaskProgress = 0;
            dialog.UserCancellable = false;
            UpdateLocalization(dialog);
        }

        public void Unbind(IProgressDialog dialog) { }

        public void UpdateLocalization(IProgressDialog dialog)
        {
            dialog.Text = Resources.Localization_UninstallLocalization_Text;
            dialog.CurrentTaskName = Resources.Localization_Uninstalling_Text;
        }
    }
}
