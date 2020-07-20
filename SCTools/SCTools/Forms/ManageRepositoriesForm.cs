using System;
using System.Linq;
using System.Web;
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
            var item = lvRepositories.Columns.Add("chName", "Name");
            item.Width = lvRepositories.Width / 2 - 5;
            lvRepositories.Columns.Add("chPath", "Path", lvRepositories.Width - item.Width - 5);
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
                MessageBox.Show($"Name {name} is empty or already exist.",
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
                MessageBox.Show($"Repository {repository} is empty or already exist.",
                    Resources.Localization_Install_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var result = new GitHubLocalizationRepository(name, repository);
            if (await result.CheckAsync())
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
                MessageBox.Show($"No access to repository {repository}.",
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
