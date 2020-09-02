using System.Windows.Forms;
using NSW.StarCitizen.Tools.Properties;
using NSW.StarCitizen.Tools.Update;

namespace NSW.StarCitizen.Tools
{
    public static partial class Program
    {
        public static ApplicationUpdater Updater { get; } = new ApplicationUpdater();

        public static bool InstallUpdateOnLaunch()
        {
            var scheduledUpdatInfo = Updater.GetScheduledUpdateInfo();
            if (scheduledUpdatInfo != null)
            {
                if (Updater.IsAlreadyInstalledVersion(scheduledUpdatInfo))
                {
                    Updater.CancelScheduleInstallUpdate();
                    return false;
                }
                var result = MessageBox.Show(string.Format(Resources.Localization_UpdateAvailableInstallAsk_Text,
                    scheduledUpdatInfo.GetVersion()), Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    return InstallScheduledUpdate();
                }
            }
            return false;
        }

        public static bool InstallScheduledUpdate()
        {
            if (!Updater.InstallScheduledUpdate())
            {
                MessageBox.Show(Resources.Localization_FailedInstallAppUpdate_Text, Name,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}
