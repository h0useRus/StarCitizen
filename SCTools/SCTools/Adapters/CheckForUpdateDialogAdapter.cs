using NSW.StarCitizen.Tools.Forms;
using NSW.StarCitizen.Tools.Properties;

namespace NSW.StarCitizen.Tools.Adapters
{
    public class CheckForUpdateDialogAdapter : IProgressDialog.IAdapter
    {
        public void Bind(IProgressDialog dialog)
        {
            dialog.CurrentTaskInfo = string.Empty;
            dialog.CurrentTaskProgress = 0;
            dialog.UserCancellable = true;
            UpdateLocalization(dialog);
        }

        public void Unbind(IProgressDialog dialog) {}

        public void UpdateLocalization(IProgressDialog dialog)
        {
            dialog.Text = Resources.Application_Update_Title;
            dialog.CurrentTaskName = Resources.Application_CheckForUpdates_Text;
        }
    }
}
