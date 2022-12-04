using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using NLog;
using NSW.StarCitizen.Tools.Controllers;
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

        public bool IsAnyProcessRunning() => _runningProcesses.Count != 0;

        public bool IsProcessRunnnig(string profileName) => _runningProcesses.ContainsKey(profileName);

        public Process? GetProcess(string profileName)
        {
            if (_runningProcesses.TryGetValue(profileName, out var process))
            {
                return process.Process;
            }
            return null;
        }

        public void StopProcesses(bool autoEnableLocalization)
        {
            var gameInfoList = new List<GameInfo>(_runningProcesses.Count);
            foreach (var pair in _runningProcesses)
            {
                try
                {
                    var process = pair.Value;
                    gameInfoList.Add(process.GameInfo);
                    process.Stop();
                    process.WaitForExit(1000);
                }
                catch (Exception e)
                {
                    _logger.Error(e, $"Failed kill process for profile: {pair.Key}");
                }
            }
            if (autoEnableLocalization)
            {
                foreach (var gameInfo in gameInfoList)
                {
                    SetEnableLocalization(null, gameInfo, false);
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

        public bool LaunchProcess(IWin32Window window, GameInfo gameInfo, string profileName, bool autoEnableLocalization)
        {
            if (_runningProcesses.ContainsKey(profileName))
                return true;
            if (autoEnableLocalization &&
                !IsAnyGameModeInstanceRunning(gameInfo.Mode) &&
                !SetEnableLocalization(window, gameInfo, true))
            {
                return false;
            }
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
                if (autoEnableLocalization)
                {
                    DisableLocalizationIfNotUsed(window, gameInfo);
                }
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
                var gameInfo = process.GameInfo;
                var eventArgs = new ProcessExitedEventArgs(process.ProfileName)
                {
                    ExitCode = process.ExitCode,
                    Stopped = process.Stopped,
                };
                _runningProcesses.Remove(process.ProfileName);
                process.Dispose();
                if (!Program.Settings.ManualEnableCore)
                {
                    DisableLocalizationIfNotUsed(null, gameInfo);
                }
                ProcessExited?.Invoke(this, eventArgs);
            }
        }

        private bool IsAnyGameModeInstanceRunning(GameMode gameMode) => _runningProcesses.Any(p => p.Value.GameInfo.Mode == gameMode);

        private bool SetEnableLocalization(IWin32Window? window, GameInfo gameInfo, bool enabled)
        {
            var localizationController = new LocalizationController(gameInfo);
            return localizationController.SetEnableLocalization(window, enabled);
        }

        private void DisableLocalizationIfNotUsed(IWin32Window? window, GameInfo gameInfo)
        {
            if (!IsAnyGameModeInstanceRunning(gameInfo.Mode))
            {
                SetEnableLocalization(window, gameInfo, false);
            }
        }
    }
}
