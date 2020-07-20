using System;
using System.Linq;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Localization;


namespace NSW.StarCitizen.Tools.Forms
{
    public partial class LocalizationForm : Form
    {
        private bool _holdUpdates;
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
        }

        private void cbRepository_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox cb)
            {
                Program.CurrentRepository = (ILocalizationRepository)cb.SelectedItem;
                tbCurrentVersion.Text = Program.CurrentRepository.CurrentVersion.Name;

                cbVersions.DataSource = Program.CurrentRepository.Versions ?? new[] { LocalizationInfo.Empty };

                cbCheckNewVersions.Checked = Program.CurrentInstallation.MonitorForUpdates;
                cbRefreshTime.SelectedItem = Program.CurrentInstallation.MonitorRefreshTime.ToString();
            }
        }

        private void cbVersions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox cb)
            {
                if (cb.SelectedItem is LocalizationInfo info)
                {
                    btnInstall.Enabled = !string.IsNullOrWhiteSpace(info.DownloadUrl);
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
                    var filePath = await Program.CurrentRepository.DownloadAsync(Program.CurrentRepository.CurrentVersion);
                    var result = Program.CurrentInstaller.Unpack(filePath, Program.CurrentGame.RootFolder.FullName, false);
                    if(result)
                        result = Program.CurrentInstaller.Validate(Program.CurrentGame.RootFolder.FullName, false);
                    if (result)
                    {
                        Program.CurrentInstallation.Repository = Program.CurrentRepository.Repository;
                        Program.CurrentInstallation.LastVersion = Program.CurrentRepository.CurrentVersion.Name;
                        Program.SaveAppSettings();
                        tbCurrentVersion.Text = Program.CurrentRepository.CurrentVersion.Name;
                    }
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                    Enabled = true;
                }
        }
    }
}
