using NSW.StarCitizen.Tools.Localization;

namespace NSW.StarCitizen.Tools
{
    public static partial class Program
    {
        private static RepositoryManager? _repositoryManager;

        public static RepositoryManager RepositoryManager => _repositoryManager ??= new RepositoryManager();
    }
}