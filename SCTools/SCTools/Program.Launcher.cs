using NSW.StarCitizen.Tools.Launcher;

namespace NSW.StarCitizen.Tools
{
    public static partial class Program
    {
        private static GameProcessesManager? _processManager;

        public static GameProcessesManager ProcessManager => _processManager ??= new GameProcessesManager();
    }
}
