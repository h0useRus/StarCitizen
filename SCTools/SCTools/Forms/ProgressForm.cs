using System;
using System.Threading;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Helpers;
using NSW.StarCitizen.Tools.Lib.Localization;

namespace NSW.StarCitizen.Tools.Forms
{
    public interface IProgressDialog
    {
        public interface IAdapter
        {
            public void Bind(IProgressDialog dialog);

            public void Unbind(IProgressDialog dialog);

            public void UpdateLocalization(IProgressDialog dialog);
        }

        public CancellationToken CancelToken { get; }

        public bool IsCanceledByUser { get; }

        public string Text { set; }

        public string CurrentTaskName { set; }

        public string CurrentTaskInfo { set; }

        public float CurrentTaskProgress { set; }

        public bool UserCancellable { set; }

        public string UserCancelText { set; }
    }

    public partial class ProgressForm : Form, IProgressDialog, ILocalizedForm
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private IProgressDialog.IAdapter? _adapter;

        public CancellationToken CancelToken => _cancellationTokenSource.Token;

        public bool IsCanceledByUser { get; private set; }

        public string CurrentTaskName
        {
            set
            {
                lblTaskName.Text = value;
                lblTaskName.Refresh();
            }
        }

        public string CurrentTaskInfo
        {
            set
            {
                lblTaskInfo.Text = value;
                lblTaskInfo.Refresh();
            }
        }

        public float CurrentTaskProgress
        {
            set => SetTaskProgressValue(value);
        }

        public bool UserCancellable
        {
            get => btnStop.Visible;
            set => btnStop.Visible = value;
        }

        public string UserCancelText
        {
            get => btnStop.Text;
            set => btnStop.Text = value;
        }

        public ProgressForm()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            InitializeComponent();
            CurrentTaskName = string.Empty;
            CurrentTaskInfo = string.Empty;
        }

        public ProgressForm(int timeoutMilliseconds)
        {
            _cancellationTokenSource = new CancellationTokenSource(timeoutMilliseconds);
            InitializeComponent();
            CurrentTaskName = string.Empty;
            CurrentTaskInfo = string.Empty;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UnbindAdapter();
                components?.Dispose();
                _cancellationTokenSource.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (StartPosition == FormStartPosition.CenterParent)
            {
                CenterToParent();
            }
        }

        public void UnbindAdapter()
        {
            if (_adapter != null)
            {
                _adapter.Unbind(this);
                _adapter = null;
            }
        }

        public void BindAdapter(IProgressDialog.IAdapter adapter)
        {
            if (!ReferenceEquals(_adapter, adapter))
            {
                UnbindAdapter();
                _adapter = adapter;
                _adapter.Bind(this);
            }
        }

        public void UpdateLocalizedControls() => _adapter?.UpdateLocalization(this);

        private void ProgressForm_FormClosing(object sender, FormClosingEventArgs e) => e.Cancel = !btnStop.Visible;

        private void ProgressForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsCanceledByUser = true;
            _cancellationTokenSource.Cancel();
        }

        private void btnStop_Click(object sender, EventArgs e) => Close();

        private void SetTaskProgressValue(float value)
        {
            if (value <= 0)
                prTaskProgress.Value = prTaskProgress.Minimum;
            else if (value >= 1)
                prTaskProgress.Value = prTaskProgress.Maximum;
            else
                prTaskProgress.Value = (int)Math.Truncate(value * (prTaskProgress.Maximum - prTaskProgress.Minimum) + prTaskProgress.Minimum);
        }
    }
}
