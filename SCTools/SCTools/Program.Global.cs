using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NSW.StarCitizen.Tools.Global;

namespace NSW.StarCitizen.Tools
{
    public static partial class Program
    {
        public static GameInfo? CurrentGame { get; set; }

        public static string Name { get; } = Assembly.GetExecutingAssembly().GetName().Name;

        public static Version Version { get; } = Assembly.GetExecutingAssembly().GetName().Version;

        public static IEnumerable<GameInfo> GetGameModes(string gameFolder)
        {
            var result = new List<GameInfo>();
            if (Directory.Exists(gameFolder))
            {
                foreach (GameMode mode in Enum.GetValues(typeof(GameMode)))
                {
                    GameInfo? gameInfo = GameInfo.Create(mode, gameFolder);
                    if (gameInfo != null)
                        result.Add(gameInfo);
                }
            }
            return result;
        }
    }
}