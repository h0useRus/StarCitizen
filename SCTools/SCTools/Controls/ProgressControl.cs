using System.Threading;
using System.Windows.Forms;

namespace NSW.StarCitizen.Tools.Controls
{
    public partial class ProgressControl : UserControl
    {
        private CancellationTokenSource _cancellationTokenSource;

        public ProgressControl()
        {
            InitializeComponent();
        }

        public CancellationToken Start(int timeOut = 60_000)
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel(false);
                _cancellationTokenSource.Dispose();
            }

            _cancellationTokenSource = new CancellationTokenSource(timeOut);
            return _cancellationTokenSource.Token;
        }
    }
}
