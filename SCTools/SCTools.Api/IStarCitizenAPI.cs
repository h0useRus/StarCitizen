using System;
using NSW.StarCitizen.Tools.API.Universes;

namespace NSW.StarCitizen.Tools.API
{
    public interface IStarCitizenAPI : IDisposable
    {
        /// <summary>
        /// Live AKA Persistent Universe (PU)
        /// </summary>
        IUniverse Live { get; }
        /// <summary>
        /// PTU AKA Persistent Test Universe
        /// </summary>
        IUniverse PTU { get; }

        IUniverse GetUniverse(UniverseType type);
    }
}
