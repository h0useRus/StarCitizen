using NSW.StarCitizen.Tools.Lib.Update;

namespace NSW.StarCitizen.Tools.Settings
{
    public static class TimePresets
    {
        private readonly static int[] _gitGubRefreshTimes = new int[]
        {
            5,
            10,
            15,
            30,
            60,
        };

        private readonly static int[] _giteeRefreshTimes = new int[]
        {
            60,
            120,
            180,
            240,
            300,
        };

        private readonly static int[] _folderRefreshTimes = new int[]
        {
            2,
            5,
            10,
            15,
            30,
            60,
        };

        public static int[] GetRefreshTimePresets(UpdateRepositoryType repositoryType) => repositoryType switch
        {
            UpdateRepositoryType.GitHub => _gitGubRefreshTimes,
            UpdateRepositoryType.Gitee => _giteeRefreshTimes,
            UpdateRepositoryType.Folder => _folderRefreshTimes,
            _ => _gitGubRefreshTimes,
        };
    }
}
