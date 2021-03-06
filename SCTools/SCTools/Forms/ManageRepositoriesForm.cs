using System;
using System.Threading;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Adapters;
using NSW.StarCitizen.Tools.Controls;
using NSW.StarCitizen.Tools.Helpers;
using NSW.StarCitizen.Tools.Properties;
using NSW.StarCitizen.Tools.Repository;
using NSW.StarCitizen.Tools.Settings;

namespace NSW.StarCitizen.Tools.Forms
{
    public partial class ManageRepositoriesForm : FormEx, ILocalizedForm
    {
        private readonly RepositoryManager _repositoryManager;
        private readonly RepositoriesListViewAdapter _repositoriesListAdapter;
        private readonly RepositoriesListViewAdapter _stdRepositoriesListAdapter;

        public ManageRepositoriesForm(RepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
            InitializeComponent();
            _repositoriesListAdapter = new RepositoriesListViewAdapter(lvRepositories);
            _stdRepositoriesListAdapter = new RepositoriesListViewAdapter(lvStdRepositories);
            _stdRepositoriesListAdapter.SetRepositoriesList(LocalizationSource.StandardList);
            UpdateLocalizedControls();
        }

        public void UpdateLocalizedControls()
        {
            Text = Resources.Localization_Repositories_Title + " - " + _repositoryManager.GameMode;
            tabPageUserRepositories.Text = Resources.Localization_UserRepos_Title;
            tabPageStdRepositories.Text = Resources.Localization_StdRepos_Title;
            lblName.Text = Resources.Localization_Name_Text;
            lblPath.Text = Resources.Localization_RepositoryURL_Text;
            btnAdd.Text = Resources.Localization_Add_Text;
            btnRemove.Text = Resources.Localization_Remove_Text;
            btnUp.Text = char.ConvertFromUtf32(0x2191);
            btnDown.Text = char.ConvertFromUtf32(0x2193);
            _repositoriesListAdapter.UpdateLocalization();
            _stdRepositoriesListAdapter.UpdateLocalization();
        }

        private void ManageRepositoriesForm_Load(object sender, EventArgs e) => DataBindList();

        private void DataBindList()
        {
            _repositoriesListAdapter.SetRepositoriesList(_repositoryManager.GetRepositoriesList());
            UpdateButtons();
        }

        private void lvRepositories_SelectedIndexChanged(object sender, EventArgs e)
        {
            var repository = _repositoriesListAdapter.GetSelectedRepository();
            if (repository != null)
            {
                tbName.Text = repository.Name;
                tbUrl.Text = repository.RepositoryUrl;
            }
            UpdateButtons();
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            var name = tbName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show(this,string.Format(Resources.Localization_InvalidRepoName_Text, name),
                    Resources.Localization_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var url = tbUrl.Text.ToLower().Trim();
            LocalizationSource? localizationSource = LocalizationSource.CreateFromUrl(name, url);
            if (localizationSource == null)
            {
                MessageBox.Show(this, string.Format(Resources.Localization_InvalidRepoUrl_Text, url),
                    Resources.Localization_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using var cancellationTokenSource = new CancellationTokenSource(20000);
            switch (await _repositoryManager.AddRepositoryAsync(localizationSource, cancellationTokenSource.Token))
            {
                case RepositoryManager.AddStatus.Success:
                    DataBindList();
                    break;
                case RepositoryManager.AddStatus.DuplicateName:
                    MessageBox.Show(this, string.Format(Resources.Localization_DuplicateRepoName_Text, name),
                        Resources.Localization_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case RepositoryManager.AddStatus.DuplicateUrl:
                    MessageBox.Show(this, string.Format(Resources.Localization_DuplicateRepoUrl_Text, url),
                        Resources.Localization_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case RepositoryManager.AddStatus.Unreachable:
                    MessageBox.Show(this, string.Format(Resources.Localization_NoRepoAccess_Text, name),
                        Resources.Localization_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var repository = _repositoriesListAdapter.GetSelectedRepository();
            if (repository != null)
            {
                var usedByGameMode = _repositoryManager.GetRepositoryUsedGameMode(repository);
                if (usedByGameMode != null)
                {
                    if (MessageBox.Show(this, string.Format(Resources.Localization_RemoveUsedRepoWarning_Text, repository.Name, usedByGameMode.ToString()),
                        Resources.Localization_Warning_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                        return;
                }
                _repositoryManager.RemoveRepository(repository);
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
                tbUrl.Text = source.GetUrl();
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            var repository = _repositoriesListAdapter.GetSelectedRepository();
            if (repository != null && _repositoryManager.MoveRepositoryUp(repository))
            {
                _repositoriesListAdapter.SetRepositoriesList(_repositoryManager.GetRepositoriesList());
                _repositoriesListAdapter.SetSelectedRepository(repository);
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            var repository = _repositoriesListAdapter.GetSelectedRepository();
            if (repository != null && _repositoryManager.MoveRepositoryDown(repository))
            {
                _repositoriesListAdapter.SetRepositoriesList(_repositoryManager.GetRepositoriesList());
                _repositoriesListAdapter.SetSelectedRepository(repository);
            }
        }

        private void tabRepositories_SelectedIndexChanged(object sender, EventArgs e) => UpdateButtons();

        private void UpdateButtons()
        {
            var visible = tabRepositories.SelectedIndex == 0;
            var repository = _repositoriesListAdapter.GetSelectedRepository();
            btnRemove.Visible = visible;
            btnRemove.Enabled = repository != null && _repositoriesListAdapter.RepositoriesCount > 1;
            btnUp.Visible = visible;
            btnUp.Enabled = repository != null && _repositoryManager.CanMoveRepositoryUp(repository);
            btnDown.Visible = visible;
            btnDown.Enabled = repository != null && _repositoryManager.CanMoveRepositoryDown(repository);
        }
    }
}
