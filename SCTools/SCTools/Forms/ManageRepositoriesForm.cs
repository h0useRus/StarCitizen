using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Localization;
using NSW.StarCitizen.Tools.Properties;
using NSW.StarCitizen.Tools.Settings;

namespace NSW.StarCitizen.Tools.Forms
{
    public partial class ManageRepositoriesForm : Form
    {
        private const string GitHubUrl = "https://github.com/";
        public ManageRepositoriesForm()
        {
            InitializeComponent();
            InitializeLocalization();
            var item = lvRepositories.Columns.Add("chName", Resources.Localization_Name_Text);
            item.Width = lvRepositories.Width / 2 - 5;
            lvRepositories.Columns.Add("chPath", Resources.Localization_Path_Text, lvRepositories.Width - item.Width - 5);
        }

        private void InitializeLocalization()
        {
            Text = Resources.Localization_Repositories_Title;
            lblName.Text = Resources.Localization_Name_Text;
            lblPath.Text = Resources.Localization_GitHubURL_Text;
            btnAdd.Text = Resources.Localization_Add_Text;
            btnRemove.Text = Resources.Localization_Remove_Text;
        }

        private void ManageRepositoriesForm_Load(object sender, EventArgs e)
        {
            DataBindList();
        }

        private void DataBindList()
        {
            lvRepositories.Items.Clear();
            foreach (var repository in Program.LocalizationRepositories)
            {
                var item = lvRepositories.Items.Add(repository.Value.Name, repository.Value.Name);
                item.Tag = repository.Value;
                item.SubItems.Add(repository.Key);
            }

            btnRemove.Enabled = lvRepositories.Items.Count > 1;
        }

        private void lvRepositories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvRepositories.SelectedItems.Count > 0 &&
                lvRepositories.SelectedItems[0]?.Tag is ILocalizationRepository repository)
            {
                tbName.Text = repository.Name;
                tbUrl.Text = GitHubUrl + repository.Repository ;
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            var name = tbName.Text?.Trim();
            if (string.IsNullOrWhiteSpace(name)
                || Program.LocalizationRepositories.Values
                    .Any(v => string.Compare(v.Name, name, StringComparison.OrdinalIgnoreCase) == 0))
            {
                MessageBox.Show(string.Format(Resources.Localization_InvalidRepoName_Text, name),
                    Resources.Localization_Install_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string repository = null;
            var url = tbUrl.Text?.ToLower().Trim();
            if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri))
            {
                repository = uri.AbsolutePath.Trim('/');
            }

            if (string.IsNullOrWhiteSpace(repository)
                || Program.LocalizationRepositories.Values
                    .Any(v=> string.Compare(v.Repository, repository, StringComparison.OrdinalIgnoreCase) == 0))
            {
                MessageBox.Show(string.Format(Resources.Localization_InvalidRepoUrl_Text, repository),
                    Resources.Localization_Install_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(20000);
            var result = new GitHubLocalizationRepository(name, repository);
            if (await result.CheckAsync(cancellationTokenSource.Token))
            {
                Program.LocalizationRepositories[result.Repository] = result;
                Program.Settings.Localization.Repositories.Add(new LocalizationSource
                {
                    Name = result.Name,
                    Repository = result.Repository,
                    Type = result.Type.ToString()
                });
                Program.SaveAppSettings();
                DataBindList();
            }
            else
            {
                MessageBox.Show(string.Format(Resources.Localization_NoRepoAccess_Text, repository),
                    Resources.Localization_Install_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lvRepositories.SelectedItems.Count > 0 && lvRepositories.SelectedItems[0]?.Tag is ILocalizationRepository repository)
            {
                Program.LocalizationRepositories.Remove(repository.Repository);
                var lr = Program.Settings.Localization.Repositories.FirstOrDefault(r=> string.Compare(r.Repository, repository.Repository, StringComparison.OrdinalIgnoreCase) == 0);
                if (lr != null)
                {
                    Program.Settings.Localization.Repositories.Remove(lr);
                    Program.SaveAppSettings();
                }
                
                lvRepositories.Items.Remove(lvRepositories.SelectedItems[0]);
            }
        }
    }
}
