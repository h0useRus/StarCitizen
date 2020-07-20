using System;
using System.Linq;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Localization;
using NSW.StarCitizen.Tools.Properties;


namespace NSW.StarCitizen.Tools.Forms
{
    public partial class LocalizationForm : Form
    {
        //private bool _holdUpdates;
        public LocalizationForm()
        {
            InitializeComponent();
        }

        private void LocalizationForm_Load(object sender, EventArgs e)
        {
            // Repositories
            cbRepository.DataSource = Program.LocalizationRepositories.Values.ToList();
            var current = Program.GetCurrentLocalizationRepository();
            if (current != null) cbRepository.SelectedItem = current;
        }

        private void cbRepository_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox cb)
            {
                Program.CurrentRepository = (ILocalizationRepository)cb.SelectedItem;
                cbVersions.DataSource = Program.CurrentRepository.Versions ?? new[] { LocalizationInfo.Empty };
            }
            UpdateControls();
        }

        private void UpdateControls()
        {
            tbCurrentVersion.Text = Program.CurrentRepository.CurrentVersion.Name;
            //Languages
            var lng = Program.GetLanguagesConfiguration();
            if (lng.Languages.Any())
            {
                cbLanguages.DataSource = lng.Languages;
                cbLanguages.SelectedItem = lng.Current;
                cbLanguages.Enabled = true;
            }
            else
            {
                cbLanguages.Enabled = false;
            }
            // enable disable
            switch (Program.CurrentRepository.Installer.GetInstallationType(Program.CurrentGame.RootFolder.FullName))
            {
                case LocalizationInstallationType.None:
                    btnLocalizationDisable.Visible = false;
                    break;
                case LocalizationInstallationType.Enabled:
                    btnLocalizationDisable.Visible = true;
                    btnLocalizationDisable.Text = Resources.Localization_Button_Disable_localization;
                    break;
                case LocalizationInstallationType.Disabled:
                    btnLocalizationDisable.Visible = true;
                    btnLocalizationDisable.Text = Resources.Localization_Button_Enable_localization;
                    break;
            }
            // monitoring
            cbCheckNewVersions.Checked = Program.CurrentInstallation.MonitorForUpdates;
            cbRefreshTime.SelectedItem = Program.CurrentInstallation.MonitorRefreshTime.ToString();
        }

        private void cbVersions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox cb)
            {
                if (cb.SelectedItem is LocalizationInfo info)
                {
                    btnInstall.Enabled = info.Actual;
                    if(info.Actual)
                        Program.CurrentRepository.CurrentVersion = info;
                }
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            if (Program.CurrentRepository != null)
                try
                {
                    Enabled = false;
                    Cursor.Current = Cursors.WaitCursor;
                    await Program.CurrentRepository.RefreshVersionsAsync();
                    cbVersions.DataSource = Program.CurrentRepository.Versions ?? new[] { LocalizationInfo.Empty };
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                    Enabled = true;
                }
        }

        private async void btnInstall_Click(object sender, EventArgs e)
        {
            if (Program.CurrentRepository?.CurrentVersion != null)
                try
                {
                    Enabled = false;
                    Cursor.Current = Cursors.WaitCursor;

                    var disabledMode = Program.CurrentRepository.Installer.GetInstallationType(Program.CurrentGame.RootFolder.FullName) == LocalizationInstallationType.Disabled;
                    var filePath = await Program.CurrentRepository.DownloadAsync(Program.CurrentRepository.CurrentVersion);
                    var result = Program.CurrentRepository.Installer.Unpack(filePath, Program.CurrentGame.RootFolder.FullName, disabledMode);
                    if (result)
                        result = Program.CurrentRepository.Installer.Validate(Program.CurrentGame.RootFolder.FullName, disabledMode);
                    if (result)
                    {
                        Program.CurrentInstallation.Repository = Program.CurrentRepository.Repository;
                        Program.CurrentInstallation.LastVersion = Program.CurrentRepository.CurrentVersion.Name;
                        Program.SaveAppSettings();
                        UpdateControls();
                    }
                    else
                    {
                        MessageBox.Show(Resources.Localization_Install_ErrorText,
                            Resources.Localization_Install_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                    Enabled = true;
                }
        }

        private void btnLocalizationDisable_Click(object sender, EventArgs e)
        {
            try
            {
                Enabled = false;
                Cursor.Current = Cursors.WaitCursor;
                Program.CurrentRepository.Installer.RevertLocalization(Program.CurrentGame.RootFolder.FullName);
                UpdateControls();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                Enabled = true;
            }
        }

        private void cbCheckNewVersions_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCheckNewVersions.Checked != Program.CurrentInstallation.MonitorForUpdates)
            {
                Program.CurrentInstallation.MonitorForUpdates = cbCheckNewVersions.Checked;
                Program.SaveAppSettings();
                Program.RunMonitors();
            }
        }

        private void cbRefreshTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbRefreshTime.SelectedItem.ToString() != Program.CurrentInstallation.MonitorRefreshTime.ToString())
            {
                Program.CurrentInstallation.MonitorRefreshTime = int.Parse(cbRefreshTime.SelectedItem.ToString());
                Program.SaveAppSettings();
                Program.RunMonitors();
            }
        }
    }
}
