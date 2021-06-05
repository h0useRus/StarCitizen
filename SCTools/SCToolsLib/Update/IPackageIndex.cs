using System.Threading;

namespace NSW.StarCitizen.Tools.Lib.Update
{
    public interface IPackageIndex
    {
        FilesIndex CreateLocal(CancellationToken? cancellationToken = default);
        bool VerifyExternal(FilesIndex filesIndex);
    }
}
