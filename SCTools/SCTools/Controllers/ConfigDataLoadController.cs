using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Defter.StarCitizen.ConfigDB;
using Defter.StarCitizen.ConfigDB.Model;
using NLog;
using NSW.StarCitizen.Tools.Adapters;
using NSW.StarCitizen.Tools.Forms;
using NSW.StarCitizen.Tools.Properties;

namespace NSW.StarCitizen.Tools.Controllers
{
    public sealed class ConfigDataLoadController
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly ConfigDataLoader _configDataLoader;

        public ConfigDataLoadController(ConfigDataLoader configDataLoader)
        {
            _configDataLoader = configDataLoader;
        }

        public async Task<ConfigData?> LoadDatabaseAsync(Control window, string languageName)
        {
            using var progressDlg = new ProgressForm();
            try
            {
                progressDlg.BindAdapter(new CheckForUpdateDialogAdapter());
                progressDlg.Show(window);
                await _configDataLoader.LoadDatabaseAsync(progressDlg.CancelToken);
                progressDlg.CurrentTaskProgress = 0.5f;
                if (_configDataLoader.DatabaseLoaded)
                {
                    await LoadDatabaseLanguageAsync(languageName, progressDlg.CancelToken);
                    progressDlg.CurrentTaskProgress = 0.9f;
                    return _configDataLoader.BuildData(languageName);
                }
                else
                {
                    _logger.Error("Failed parse settings database");
                    MessageBox.Show(window, Resources.GameSettings_FailedParseDb_Text,
                        Resources.Localization_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception e)
            {
                if (!progressDlg.IsCanceledByUser)
                {
                    _logger.Error(e, "Error load settings database");
                    MessageBox.Show(window, Resources.GameSettings_FailedLoadDb_Text,
                        Resources.Localization_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                progressDlg.Hide();
            }
            return null;
        }

        private async Task<bool> LoadDatabaseLanguageAsync(string languageName, CancellationToken? cancellationToken = default)
        {
            try
            {
                if (_configDataLoader.GetSupportedLanguages().Contains(languageName))
                {
                    await _configDataLoader.LoadTranslationAsync(languageName, cancellationToken);
                    return _configDataLoader.LoadedLanguages.Contains(languageName);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Error load database language: {languageName}");
            }
            return false;
        }
    }
}
