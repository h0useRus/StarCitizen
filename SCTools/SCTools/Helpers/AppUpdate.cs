using System.Windows.Forms;
using NLog;
using NSW.StarCitizen.Tools.Lib.Update;
using NSW.StarCitizen.Tools.Properties;
using NSW.StarCitizen.Tools.Repository;

namespace NSW.StarCitizen.Tools.Helpers
{
    public static class AppUpdate
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static ApplicationUpdater Updater { get; } = new ApplicationUpdater(GetUpdateRepository(),
            Program.ExecutableDir, Resources.UpdateScript);

        public static bool InstallUpdateOnLaunch(string[] args)
        {
            Updater.RemoveUpdateScript();
            if ((args.Length >= 2) && (args[0] == "update_status") && (args[1] != InstallUpdateStatus.Success.ToString("d")))
            {
                _logger.Error($"Failed install update: {args[1]}");
                MessageBox.Show(Resources.Application_FailedInstallUpdate_Text + @" - " + args[1], Program.Name,
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
                var result = MessageBox.Show(string.Format(Resources.Application_UpdateAvailableInstallAsk_Text,
                    scheduledUpdateInfo.GetVersion()), Program.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
                _logger.Error($"Failed launch install update: {result}");
                MessageBox.Show(Resources.Application_FailedInstallUpdate_Text + @" - " + result.ToString("d"), Program.Name,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private static IUpdateRepository GetUpdateRepository()
        {
            var updateInfoFactory = GitHubUpdateInfo.Factory.NewWithVersionByTagName();
            var updateRepository = new GitHubUpdateRepository(HttpNetClient.Client,
                GitHubDownloadType.Assets, updateInfoFactory, Program.Name, "h0useRus/StarCitizen");
            updateRepository.SetCurrentVersion(Program.Version.ToString(3));
            return updateRepository;
        }
    }
}
