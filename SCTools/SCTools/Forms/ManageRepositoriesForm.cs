using System;
using System.Threading;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Global;
using NSW.StarCitizen.Tools.Localization;
using NSW.StarCitizen.Tools.Properties;

namespace NSW.StarCitizen.Tools.Forms
{
    public partial class ManageRepositoriesForm : Form
    {
        private const string GitHubUrl = "https://github.com/";

        public ManageRepositoriesForm(GameInfo currentGame)
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
            foreach (var repository in Program.RepositoryManager.GetRepositoriesList())
            {
                var item = lvRepositories.Items.Add(repository.Name, repository.Name);
                item.Tag = repository;
                item.SubItems.Add(repository.Repository);
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
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show(string.Format(Resources.Localization_InvalidRepoName_Text, name),
                    Resources.Localization_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string repositoryUrl = null;
            var url = tbUrl.Text?.ToLower().Trim();
            if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri))
            {
                repositoryUrl = uri.AbsolutePath.Trim('/');
            }

            if (string.IsNullOrWhiteSpace(repositoryUrl))
            {
                MessageBox.Show(string.Format(Resources.Localization_InvalidRepoUrl_Text, repositoryUrl),
                    Resources.Localization_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var repository = new GitHubLocalizationRepository(name, repositoryUrl);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(20000);
            switch (await Program.RepositoryManager.AddRepositoryAsync(repository, cancellationTokenSource.Token))
            {
                case RepositoryManager.AddStatus.Success:
                    DataBindList();
                    break;
                case RepositoryManager.AddStatus.DuplicateName:
                    MessageBox.Show(string.Format(Resources.Localization_DuplicateRepoName_Text, repository.Name),
                        Resources.Localization_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case RepositoryManager.AddStatus.DuplicateUrl:
                    MessageBox.Show(string.Format(Resources.Localization_DuplicateRepoUrl_Text, repository.Repository),
                        Resources.Localization_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case RepositoryManager.AddStatus.Unreachable:
                    MessageBox.Show(string.Format(Resources.Localization_NoRepoAccess_Text, repository),
                        Resources.Localization_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lvRepositories.SelectedItems.Count > 0 && lvRepositories.SelectedItems[0]?.Tag is ILocalizationRepository repository)
            {
                var usedByGameMode = Program.RepositoryManager.GetRepositoryUsedGameMode(repository);
                if (usedByGameMode != null)
                {
                    if (MessageBox.Show(string.Format(Resources.Localization_RemoveUsedRepoWarning_Text, repository.Name, usedByGameMode.ToString()),
                        Resources.Localization_Warning_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                        return;
                }
                Program.RepositoryManager.RemoveRepository(repository);
                lvRepositories.Items.Remove(lvRepositories.SelectedItems[0]);
            }
        }
    }
}
