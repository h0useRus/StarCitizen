using System;

namespace NSW.StarCitizen.Tools.Launcher
{
    public sealed class ProcessExitedEventArgs : EventArgs
    {
        public string ProfileName { get; }
        public int ExitCode { get; set; }
        public bool Stopped { get; set; }

        public ProcessExitedEventArgs(string profileName)
        {
            ProfileName = profileName;
        }
    }
}
