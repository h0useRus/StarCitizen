using System.Windows.Forms;
using NSW.StarCitizen.Tools.Services;

namespace NSW.StarCitizen.Tools.Forms
{
    public partial class LocalizationForm : Form
    {
        private static string _rootPath;

        public LocalizationForm()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(IWin32Window owner, GameInfo gameInfo)
        {
            return ShowDialog(owner);
        }
    }
}
