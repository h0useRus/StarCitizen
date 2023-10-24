using System;
using NSW.StarCitizen.Tools.API.Configuration;

namespace NSW.StarCitizen.Tools.API.Universes
{
    public interface IUniverse : IDisposable
    {
        UniverseType Type { get; }
        string RootFolder { get; }
        string UserFolder { get; }
        string BinFolder { get; }
        string DataFolder { get; }
        string ExecutableFile { get; }
        UserConfiguration UserConfiguration { get; }
    }
}
