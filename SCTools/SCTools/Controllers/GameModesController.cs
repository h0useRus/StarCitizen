using System;
using System.IO;
using System.Windows.Forms;
using NLog;
using NSW.StarCitizen.Tools.Helpers;
using NSW.StarCitizen.Tools.Lib.Global;
using NSW.StarCitizen.Tools.Properties;

namespace NSW.StarCitizen.Tools.Controllers
{
    public sealed class GameModesController
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly string _gameFolder;
        private readonly GameInfo? _currentGameInfo;

        public GameModesController(string gameFolder, GameInfo? currentGameInfo)
        {
            _gameFolder = gameFolder;
            _currentGameInfo = currentGameInfo;
        }

        public bool MoveGameMode(IWin32Window window, GameMode srcMode, GameMode destMode)
        {
            var destPath = GameConstants.GetGameModePath(_gameFolder, destMode);
            if (Directory.Exists(destPath))
            {
                RtlAwareMessageBox.Show(window, Resources.Localization_File_ErrorText,
                    Resources.Localization_File_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            var srcPath = GameConstants.GetGameModePath(_gameFolder, srcMode);
            if (!Directory.Exists(srcPath))
            {
                RtlAwareMessageBox.Show(window, Resources.Localization_File_ErrorText,
                    Resources.Localization_File_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (_currentGameInfo != null && _currentGameInfo.Mode == srcMode)
            {
                var controller = new LocalizationController(_currentGameInfo);
                if (!controller.Uninstall(window))
                {
                    return false;
                }
            }
            using var gameMutex = new GameMutex();
            if (!GameMutexController.AcquireWithRetryDialog(window, gameMutex))
            {
                return false;
            }
            try
            {
                Directory.Move(srcPath, destPath);
            }
            catch (Exception e)
            {
                gameMutex.Release();
                _logger.Error(e, "Failed rename game folder: " + srcPath);
                RtlAwareMessageBox.Show(window, Resources.Localization_File_ErrorText,
                    Resources.Localization_File_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}
