using System.Threading;
using System.Threading.Tasks;

namespace NSW.StarCitizen.Tools.Repositories
{
    public interface IPackageIndex
    {
        Task<FilesIndex> CreateLocalAsync(CancellationToken cancellationToken);
        bool VerifyExternal(FilesIndex filesIndex);
    }
}
