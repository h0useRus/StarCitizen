using System;
using System.Collections.Generic;
using NSW.StarCitizen.Tools.API.Universes;

namespace NSW.StarCitizen.Tools.API
{
    public interface IStarCitizenAPI : IDisposable
    {
        /// <summary>
        /// All avaibale universes
        /// </summary>
        IReadOnlyDictionary<string, IUniverse> Universes { get; }
        /// <summary>
        /// Live AKA Persistent Universe (PU)
        /// </summary>
        IUniverse? Live { get; }
        /// <summary>
        /// PTU AKA Persistent Test Universe
        /// </summary>
        IUniverse? PTU { get; }
        /// <summary>
        /// Get universe by name
        /// </summary>
        /// <param name="name">The universe name</param>
        /// <returns><c>null</c> if universe not found</returns>
        IUniverse? GetUniverse(string name);
        /// <summary>
        /// Refresh api data
        /// </summary>
        void Refresh();
    }
}
