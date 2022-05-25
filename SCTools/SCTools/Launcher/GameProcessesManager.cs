using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using NLog;
using NSW.StarCitizen.Tools.Lib.Global;
using NSW.StarCitizen.Tools.Lib.Helpers;

namespace NSW.StarCitizen.Tools.Launcher
{
    public sealed class GameProcessesManager : IDisposable
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly SynchronizationContext _synchronizationContext = SynchronizationContext.Current;
        private readonly IDictionary<string, GameProcess> _runningProcesses = new Dictionary<string, GameProcess>();

        public event EventHandler<ProcessExitedEventArgs>? ProcessExited;

        public void Dispose()
        {
            DisposableUtils.Dispose(_runningProcesses);
            _runningProcesses.Clear();
        }

        public bool IsProcessRunnnig(string profileName) => _runningProcesses.ContainsKey(profileName);

        public Process? GetProcess(string profileName)
        {
            if (_runningProcesses.TryGetValue(profileName, out var process))
            {
                return process.Process;
            }
            return null;
        }

        public void StopProcesses()
        {
            foreach (var pair in _runningProcesses)
            {
                try
                {
                    pair.Value.Stop();
                }
                catch (Exception e)
                {
                    _logger.Error(e, $"Failed kill process for profile: {pair.Key}");
                }
            }
        }

        public bool StopProcess(string profileName)
        {
            if (_runningProcesses.TryGetValue(profileName, out var process))
            {
                try
                {
                    process.Stop();
                    return true;
                }
                catch (Exception e)
                {
                    _logger.Error(e, $"Failed kill process for profile: {profileName}");
                }
            }
            return false;
        }

        public bool LaunchProcess(GameInfo gameInfo, string profileName)
        {
            if (_runningProcesses.ContainsKey(profileName))
                return true;
            GameProcess? runningProcess = null;
            try
            {
                runningProcess = new GameProcess(_synchronizationContext, gameInfo, profileName);
                runningProcess.Exited += RunningProcess_Exited;
                runningProcess.ErrorDataReceived += RunningProcess_ErrorDataReceived;
                runningProcess.OutputDataReceived += RunningProcess_OutputDataReceived;
                if (!runningProcess.Start())
                {
                    throw new Exception("Failed launch game process");
                }
                _runningProcesses.Add(profileName, runningProcess);
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Unable start game with profile: " + profileName);
                runningProcess?.Dispose();
                return false;
            }
            return true;
        }

        private void RunningProcess_OutputDataReceived(object sender, DataReceivedEventArgs e) => _logger.Info(e.Data);

        private void RunningProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e) => _logger.Error(e.Data);

        private void RunningProcess_Exited(object sender, EventArgs e)
        {
            if (sender is GameProcess process)
            {
                var eventArgs = new ProcessExitedEventArgs(process.ProfileName)
                {
                    ExitCode = process.ExitCode,
                    Stopped = process.Stopped,
                };
                _runningProcesses.Remove(process.ProfileName);
                process.Dispose();
                ProcessExited?.Invoke(this, eventArgs);
            }
        }
    }
}
