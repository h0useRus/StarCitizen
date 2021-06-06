using System.Threading;
using System.Threading.Tasks;

namespace NSW.StarCitizen.Tools.Lib.Update
{
    public interface IPackageIndex
    {
        Task<FilesIndex> CreateLocalAsync(CancellationToken? cancellationToken = default);
        bool VerifyExternal(FilesIndex filesIndex);
    }
}
