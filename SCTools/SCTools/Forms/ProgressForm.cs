using System;
using System.Threading;
using System.Windows.Forms;

namespace NSW.StarCitizen.Tools.Forms
{
    public interface IProgressDialog
    {
        public CancellationToken CancelToken { get; }

        public bool IsCanceledByUser { get; }

        public string Text { set; }

        public string CurrentTaskName { set; }

        public string CurrentTaskInfo { set; }

        public float CurrentTaskProgress { set; }

        public bool UserCancellable { set; }

        public string UserCancelText { set; }
    }

    public partial class ProgressForm : Form, IProgressDialog
    {
        private readonly CancellationTokenSource _cancellationTokenSource;

        public CancellationToken CancelToken => _cancellationTokenSource.Token;

        public bool IsCanceledByUser { get; private set; } = false;

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
