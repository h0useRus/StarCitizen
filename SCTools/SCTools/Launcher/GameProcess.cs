using System;
using System.Diagnostics;
using System.Threading;
using NSW.StarCitizen.Tools.Lib.Global;

namespace NSW.StarCitizen.Tools.Launcher
{
    sealed class GameProcess : IDisposable
    {
        private readonly Process _process = new Process();
        private readonly SynchronizationContext _dispatcher;
        private readonly string _profileName;

        public bool Stopped { get; private set; } = false;
        public int ExitCode => _process.ExitCode;
        public string ProfileName => _profileName;
        public Process Process => _process;

        public event DataReceivedEventHandler? ErrorDataReceived;
        public event DataReceivedEventHandler? OutputDataReceived;
        public event EventHandler? Exited;

        public GameProcess(SynchronizationContext dispatcher, GameInfo gameInfo, string profileName)
        {
            _dispatcher = dispatcher;
            _profileName = profileName;
            _process.StartInfo.FileName = gameInfo.ExeFilePath;
            _process.StartInfo.WorkingDirectory = gameInfo.RootFolderPath;
            _process.StartInfo.RedirectStandardError = true;
            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.Arguments = "-allowMultipleInstances";
            _process.EnableRaisingEvents = true;
            _process.Exited += Process_Exited;
            _process.ErrorDataReceived += Process_ErrorDataReceived;
            _process.OutputDataReceived += Process_OutputDataReceived;
        }

        public void Dispose() => _process.Dispose();

        public bool Start()
        {
            if (_process.Start())
            {
                _process.BeginErrorReadLine();
                _process.BeginOutputReadLine();
                return true;
            }
            return false;
        }

        public void Stop()
        {
            if (!Stopped)
            {
                Stopped = true;
                _process.Kill();
            }
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e) => _dispatcher.Post(new SendOrPostCallback((o) =>
        {
            OutputDataReceived?.Invoke(this, e);
        }), null);

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e) => _dispatcher.Post(new SendOrPostCallback((o) =>
        {
            ErrorDataReceived?.Invoke(this, e);
        }), null);

        private void Process_Exited(object sender, EventArgs e) => _dispatcher.Post(new SendOrPostCallback((o) =>
        {
            Exited?.Invoke(this, EventArgs.Empty);
        }), null);
    }
}
