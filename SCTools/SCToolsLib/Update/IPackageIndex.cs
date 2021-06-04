using System.Threading;
using NSW.StarCitizen.Tools.Lib.Helpers;

namespace NSW.StarCitizen.Tools.Lib.Update
{
    public interface IPackageIndex
    {
        FilesIndex CreateLocal(CancellationToken? cancellationToken = default);
        bool VerifyExternal(FilesIndex filesIndex);
    }
}
