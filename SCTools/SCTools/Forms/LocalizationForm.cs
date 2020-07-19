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
                }
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            if (Program.CurrentRepository != null)
            {
                btnRefresh.Enabled = false;
                await Program.CurrentRepository.RefreshVersionsAsync();
                cbVersions.DataSource = Program.CurrentRepository.Versions ?? new[] { LocalizationInfo.Empty };
                btnRefresh.Enabled = true;
            }
        }
    }
}
