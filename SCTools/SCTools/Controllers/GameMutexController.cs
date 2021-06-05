using System.Windows.Forms;
using NSW.StarCitizen.Tools.Lib.Global;
using NSW.StarCitizen.Tools.Properties;

namespace NSW.StarCitizen.Tools.Controllers
{
    public static class GameMutexController
    {
        public static bool AcquireWithRetryDialog(Control window, GameMutex gameMutex)
        {
            while (!gameMutex.TryAcquire())
            {
                var dialogResult = MessageBox.Show(window, Resources.Application_CloseGame_Text, Resources.Localization_Warning_Title,
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
