using System;
using System.Threading;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Adapters;
using NSW.StarCitizen.Tools.Localization;
using NSW.StarCitizen.Tools.Properties;
using NSW.StarCitizen.Tools.Settings;

namespace NSW.StarCitizen.Tools.Forms
{
    public partial class ManageRepositoriesForm : Form, ILocalizedForm
    {
        private const string GitHubUrl = "https://github.com/";
        private readonly RepositoriesListViewAdapter _repositoriesListAdapter;
        private readonly RepositoriesListViewAdapter _stdRepositoriesListAdapter;

        public ManageRepositoriesForm()
        {
            InitializeComponent();
            _repositoriesListAdapter = new RepositoriesListViewAdapter(lvRepositories);
            _stdRepositoriesListAdapter = new RepositoriesListViewAdapter(lvStdRepositories);
            _stdRepositoriesListAdapter.SetRepositoriesList(LocalizationSource.StandardList);
            UpdateLocalizedControls();
        }

        public void UpdateLocalizedControls()
        {
            Text = Resources.Localization_Repositories_Title;
            tabPageUserRepositories.Text = Resources.Localization_UserRepos_Title;
            tabPageStdRepositories.Text = Resources.Localization_StdRepos_Title;
            lblName.Text = Resources.Localization_Name_Text;
            lblPath.Text = Resources.Localization_GitHubURL_Text;
            btnAdd.Text = Resources.Localization_Add_Text;
            btnRemove.Text = Resources.Localization_Remove_Text;
            _repositoriesListAdapter.UpdateLocalization();
            _stdRepositoriesListAdapter.UpdateLocalization();
        }

        private void ManageRepositoriesForm_Load(object sender, EventArgs e) => DataBindList();

        private void DataBindList()
        {
            _repositoriesListAdapter.SetRepositoriesList(Program.RepositoryManager.GetRepositoriesList());
            UpdateButtons();
        }

        private void lvRepositories_SelectedIndexChanged(object sender, EventArgs e)
        {
            var repository = _repositoriesListAdapter.GetSelectedRepository();
            if (repository != null)
            {
                tbName.Text = repository.Name;
                tbUrl.Text = GitHubUrl + repository.Repository ;
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            var name = tbName.Text?.Trim();
            if (name == null || string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show(string.Format(Resources.Localization_InvalidRepoName_Text, name),
                    Resources.Localization_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string repositoryUrl;
            var url = tbUrl.Text?.ToLower().Trim();
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri) ||
                string.IsNullOrWhiteSpace(repositoryUrl = uri.AbsolutePath.Trim('/')))
            {
                MessageBox.Show(string.Format(Resources.Localization_InvalidRepoUrl_Text, url),
                       Resources.Localization_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using var cancellationTokenSource = new CancellationTokenSource(20000);
            switch (await Program.RepositoryManager.AddRepositoryAsync(name, repositoryUrl, cancellationTokenSource.Token))
            {
                case RepositoryManager.AddStatus.Success:
                    DataBindList();
                    break;
                case RepositoryManager.AddStatus.DuplicateName:
                    MessageBox.Show(string.Format(Resources.Localization_DuplicateRepoName_Text, name),
                        Resources.Localization_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case RepositoryManager.AddStatus.DuplicateUrl:
                    MessageBox.Show(string.Format(Resources.Localization_DuplicateRepoUrl_Text, repositoryUrl),
                        Resources.Localization_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case RepositoryManager.AddStatus.Unreachable:
                    MessageBox.Show(string.Format(Resources.Localization_NoRepoAccess_Text, name),
                        Resources.Localization_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var repository = _repositoriesListAdapter.GetSelectedRepository();
            if (repository != null)
            {
                var usedByGameMode = Program.RepositoryManager.GetRepositoryUsedGameMode(repository);
                if (usedByGameMode != null)
                {
                    if (MessageBox.Show(string.Format(Resources.Localization_RemoveUsedRepoWarning_Text, repository.Name, usedByGameMode.ToString()),
                        Resources.Localization_Warning_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                        return;
                }
                Program.RepositoryManager.RemoveRepository(repository);
                _repositoriesListAdapter.RemoveSelectedItem();
                UpdateButtons();
            }
        }

        private void lvStdRepositories_SelectedIndexChanged(object sender, EventArgs e)
        {
            var source = _stdRepositoriesListAdapter.GetSelectedSource();
            if (source != null)
            {
                tbName.Text = source.Name;
                tbUrl.Text = GitHubUrl + source.Repository;
            }
        }

        private void tabRepositories_SelectedIndexChanged(object sender, EventArgs e) => UpdateButtons();

        private void UpdateButtons() => btnRemove.Enabled = _repositoriesListAdapter.RepositoriesCount > 1 && tabRepositories.SelectedIndex == 0;
    }
}
