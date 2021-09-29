using System;
using System.Globalization;
using System.IO;
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

        public static ApplicationUpdater Updater { get; private set; } = new ApplicationUpdater(
            GetUpdateRepository(Program.Settings.Update.RepositoryType),
            Program.ExecutableDir, Resources.UpdateScript, new PackageVerifier());

        public static bool ChangeUpdateRepositoryType(UpdateRepositoryType updateRepositoryType)
        {
            if (Updater.RepositoryType != updateRepositoryType)
            {
                Updater = new ApplicationUpdater(GetUpdateRepository(updateRepositoryType),
                    Program.ExecutableDir, Resources.UpdateScript, new PackageVerifier());
                return true;
            }
            return false;
        }

        public static bool InstallUpdateOnLaunch(string[] args)
        {
            Updater.RemoveUpdateScript();
            if ((args.Length >= 2) && (args[0] == "update_status") && (args[1] != InstallUpdateStatus.Success.ToString("d")))
            {
                _logger.Error($"Failed install update: {args[1]}");
                RtlAwareMessageBox.Show(Resources.Application_FailedInstallUpdate_Text + @" - " + args[1], Program.Name,
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
                var result = RtlAwareMessageBox.Show(string.Format(CultureInfo.CurrentUICulture, Resources.Application_UpdateAvailableInstallAsk_Text,
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
                RtlAwareMessageBox.Show(Resources.Application_FailedInstallUpdate_Text + @" - " + result.ToString("d"), Program.Name,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private static IUpdateRepository GetUpdateRepository(UpdateRepositoryType updateRepositoryType)
        {
            IUpdateRepository updateRepository = updateRepositoryType switch
            {
                UpdateRepositoryType.GitHub => new GitHubUpdateRepository(HttpNetClient.Client, GitHubDownloadType.Assets,
                                       GitHubUpdateInfo.Factory.NewWithVersionByTagName(), $"{Program.Name} (GitHub)", "h0useRus/StarCitizen"),
                UpdateRepositoryType.Gitee => new GiteeUpdateRepository(HttpNetClient.Client,
                                       GiteeUpdateInfo.Factory.NewWithVersionByTagName(), $"{Program.Name} (Gitee)", "defter/StarCitizen"),
                _ => throw new NotSupportedException($"Application update repository type {updateRepositoryType} is not supported"),
            };
            updateRepository.SetCurrentVersion(Program.Version.ToString(3));
            return updateRepository;
        }

        private sealed class PackageVerifier : ApplicationUpdater.IPackageVerifier
        {
            public bool VerifyPackage(string path)
            {
                if (!File.Exists(Path.Combine(path, $"{Program.Name}.exe")) ||
                    !File.Exists(Path.Combine(path, $"{Program.Name}.exe.config")))
                {
                    return false;
                }
                return true;
            }
        }
    }
}
