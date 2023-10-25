using System;
using NSW.StarCitizen.Tools.API.Storage;

namespace NSW.StarCitizen.Tools.API.Universes
{
    public interface IUniverse : IDisposable
    {
        string Name { get; }
        IFiles Files { get; }
    }
}
