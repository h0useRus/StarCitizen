using System.Collections.Generic;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Lib.Localization;
using NSW.StarCitizen.Tools.Properties;
using NSW.StarCitizen.Tools.Settings;

namespace NSW.StarCitizen.Tools.Adapters
{
    public class RepositoriesListViewAdapter
    {
        private const string COLUMN_KEY_TYPE = "chType";
        private const string COLUMN_KEY_NAME = "chName";
        private const string COLUMN_KEY_PATH = "chPath";
        private readonly ListView _listView;

        public int RepositoriesCount => _listView.Items.Count;

        public RepositoriesListViewAdapter(ListView listView)
        {
            _listView = listView;
            Initialize();
        }

        public void UpdateLocalization()
        {
            foreach (var column in _listView.Columns)
            {
                if (column is ColumnHeader columnHeader)
                {
                    switch (columnHeader.Name)
                    {
                        case COLUMN_KEY_TYPE:
                            columnHeader.Text = string.Empty;
                            break;
                        case COLUMN_KEY_NAME:
                            columnHeader.Text = Resources.Localization_Name_Text;
                            break;
                        case COLUMN_KEY_PATH:
                            columnHeader.Text = Resources.Localization_Path_Text;
                            break;
                    }
                }
            }
        } 

        public void SetRepositoriesList(IEnumerable<ILocalizationRepository> repositories)
        {
            _listView.Items.Clear();
            foreach (var repository in repositories)
            {
                var item = _listView.Items.Add(string.Empty, repository.Type.ToString());
                item.Tag = repository;
                item.SubItems.Add(repository.Name);
                item.SubItems.Add(repository.Repository);
            }
        }

        public void SetRepositoriesList(IEnumerable<LocalizationSource> repositories)
        {
            _listView.Items.Clear();
            foreach (var repository in repositories)
            {
                var item = _listView.Items.Add(string.Empty, repository.Type.ToString());
                item.Tag = repository;
                item.SubItems.Add(repository.Name);
                item.SubItems.Add(repository.Repository);
            }
        }

        public ILocalizationRepository? GetRepository(int index)
        {
            var item = _listView.Items[index];
            if (item.Tag is ILocalizationRepository repository)
            {
                return repository;
            }
            return null;
        }

        public LocalizationSource? GetSource(int index)
        {
            var item = _listView.Items[index];
            if (item.Tag is LocalizationSource source)
            {
                return source;
            }
            return null;
        }

        public int GetSelectedIndex()
        {
            if (_listView.SelectedItems.Count > 0)
            {
                return _listView.SelectedItems[0].Index;
            }
            return -1;
        }

        public int GetRepositoryIndex(ILocalizationRepository repository)
        {
            for (int index = 0; index < _listView.Items.Count; index++)
            {
                if (GetRepository(index) == repository)
                {
                    return index;
                }
            }
            return -1;
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

        public void SetSelectedIndex(int itemIndex)
        {
            _listView.SelectedIndices.Clear();
            if (itemIndex >= 0)
            {
                _listView.SelectedIndices.Add(itemIndex);
            }
        }

        public void SetSelectedRepository(ILocalizationRepository repository)
        {
            int index = GetRepositoryIndex(repository);
            if (index >= 0)
            {
                SetSelectedIndex(index);
            }
        }

        private void Initialize()
        {
            var typeItem = _listView.Columns.Add(COLUMN_KEY_TYPE, string.Empty);
            typeItem.Width = 24;
            var nameItem = _listView.Columns.Add(COLUMN_KEY_NAME, Resources.Localization_Name_Text);
            nameItem.Width = _listView.Width / 2 - 5;
            _listView.Columns.Add(COLUMN_KEY_PATH, Resources.Localization_Path_Text,
                _listView.Width - nameItem.Width - typeItem.Width - 5);
        }
    }
}
