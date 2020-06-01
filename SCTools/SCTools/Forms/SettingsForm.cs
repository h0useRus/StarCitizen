using System;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Services;

namespace NSW.StarCitizen.Tools.Forms
{
    public partial class SettingsForm : Form
    {
        private bool _stopGeneral;
        public SettingsForm()
        {
            InitializeComponent();
            InitGeneral();
        }

        private void InitGeneral()
        {
            _stopGeneral = true;
            cbGeneralRunMinimized.Checked = SettingsService.Instance.AppSettings.RunMinimized;
            cbGeneralRunWithWindows.Checked = SettingsService.Instance.AppSettings.RunWithWindows;
            _stopGeneral = false;
        }

        private void cbGeneralRunWithWindows_CheckedChanged(object sender, EventArgs e)
        {
            if (_stopGeneral) return;
            SettingsService.Instance.AppSettings.RunWithWindows = cbGeneralRunWithWindows.Checked;
        }

        private void cbGeneralRunMinimized_CheckedChanged(object sender, EventArgs e)
        {
            if (_stopGeneral) return;
            SettingsService.Instance.AppSettings.RunMinimized = cbGeneralRunMinimized.Checked;
            SettingsService.Instance.SaveAppSettings();
        }
    }
}
