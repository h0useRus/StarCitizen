using System.Collections.Generic;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Localization;
using NSW.StarCitizen.Tools.Properties;
using NSW.StarCitizen.Tools.Settings;

namespace NSW.StarCitizen.Tools.Adapters
{
    public class RepositoriesListViewAdapter
    {
        private readonly ListView _listView;

        public int RepositoriesCount => _listView.Items.Count;

        public RepositoriesListViewAdapter(ListView listView)
        {
            _listView = listView;
            Initialize();
        }

        public void SetRepositoriesList(IEnumerable<ILocalizationRepository> repositories)
        {
            _listView.Items.Clear();
            foreach (var repository in repositories)
            {
                var item = _listView.Items.Add(repository.Name, repository.Name);
                item.Tag = repository;
                item.SubItems.Add(repository.Repository);
            }
        }

        public void SetRepositoriesList(IEnumerable<LocalizationSource> repositories)
        {
            _listView.Items.Clear();
            foreach (var repository in repositories)
            {
                var item = _listView.Items.Add(repository.Name, repository.Name);
                item.Tag = repository;
                item.SubItems.Add(repository.Repository);
            }
        }

        public ILocalizationRepository? GetSelectedRepository()
        {
            if (_listView.SelectedItems.Count > 0 &&
                _listView.SelectedItems[0]?.Tag is ILocalizationRepository repository)
            {
                return repository;
            }
            return null;
        }

        public LocalizationSource? GetSelectedSource()
        {
            if (_listView.SelectedItems.Count > 0 &&
                _listView.SelectedItems[0]?.Tag is LocalizationSource source)
            {
                return source;
            }
            return null;
        }

        public void RemoveSelectedItem()
        {
            if (_listView.SelectedItems.Count > 0)
            {
                _listView.Items.Remove(_listView.SelectedItems[0]);
            }
        }

        private void Initialize()
        {
            var item = _listView.Columns.Add("chName", Resources.Localization_Name_Text);
            item.Width = _listView.Width / 2 - 5;
            _listView.Columns.Add("chPath", Resources.Localization_Path_Text, _listView.Width - item.Width - 5);
        }
    }
}
