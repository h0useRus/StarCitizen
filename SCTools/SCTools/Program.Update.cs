using System.Windows.Forms;
using NSW.StarCitizen.Tools.Properties;
using NSW.StarCitizen.Tools.Update;

namespace NSW.StarCitizen.Tools
{
    public static partial class Program
    {
        public static ApplicationUpdater Updater { get; } = new ApplicationUpdater();

        public static bool InstallUpdateOnLaunch(string[] args)
        {
            Updater.RemoveUpdateScript();
            if ((args.Length >= 2) && (args[0] == "update_status") && (args[1] != InstallUpdateStatus.Success.ToString("d")))
            {
                MessageBox.Show(Resources.Localization_FailedInstallAppUpdate_Text + " - " + args[1], Name,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            var scheduledUpdateInfo = Updater.GetScheduledUpdateInfo();
            if (scheduledUpdateInfo != null)
            {
                if (Updater.IsAlreadyInstalledVersion(scheduledUpdateInfo))
                {
                    Updater.CancelScheduleInstallUpdate();
                    return false;
                }
                Updater.ApplyScheduledUpdateProps(scheduledUpdateInfo);
                var result = MessageBox.Show(string.Format(Resources.Localization_UpdateAvailableInstallAsk_Text,
                    scheduledUpdateInfo.GetVersion()), Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    return InstallScheduledUpdate();
                }
            }
            return false;
        }

        public static bool InstallScheduledUpdate()
        {
            var result = Updater.InstallScheduledUpdate();
            if (result != InstallUpdateStatus.Success)
            {
                MessageBox.Show(Resources.Localization_FailedInstallAppUpdate_Text + " - " + result.ToString("d"), Name,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}
