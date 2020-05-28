using System.Windows.Forms;
using NSW.StarCitizen.Tools.Services;

namespace NSW.StarCitizen.Tools.Forms
{
    public partial class LocalizationForm : Form
    {
        private PatchInfo _current;

        public LocalizationForm()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(IWin32Window owner, GameInfo gameInfo)
            => Init(gameInfo) ? ShowDialog(owner) : DialogResult.Cancel;

        private bool Init(GameInfo gameInfo)
        {
            _current = LocalizationService.Instance.GetPatchSupport(gameInfo);
            UpdateControls();
            return true;
        }

        private void btnLocalization_Click(object sender, System.EventArgs e)
        {
            _current = LocalizationService.Instance.Patch(_current);
            UpdateControls();
        }

        private void UpdateControls()
        {
            btnLocalization.Visible = _current.Status != PatchStatus.NotSupported;
            btnLocalization.Text = _current.Status == PatchStatus.Original
                ? "Включить поддержку локализации"
                : "Отключить поддержку локализации";
        }
    }
}
