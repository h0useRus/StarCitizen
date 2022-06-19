using System.Windows.Forms;
using NSW.StarCitizen.Tools.Helpers;
using NSW.StarCitizen.Tools.Lib.Global;
using NSW.StarCitizen.Tools.Properties;

namespace NSW.StarCitizen.Tools.Controllers
{
    public static class GameMutexController
    {
        public static bool AcquireWithRetryDialog(IWin32Window window, GameMutex gameMutex)
        {
            while (Program.ProcessManager.IsAnyProcessRunning() || !gameMutex.TryAcquire())
            {
                var dialogResult = RtlAwareMessageBox.Show(window, Resources.Application_CloseGame_Text, Resources.Localization_Warning_Title,
                    MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (dialogResult != DialogResult.Retry)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
