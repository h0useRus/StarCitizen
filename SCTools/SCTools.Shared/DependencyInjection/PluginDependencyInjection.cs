using Microsoft.Extensions.DependencyInjection;
using NSW.StarCitizen.Tools.Plugins;

namespace NSW.StarCitizen.Tools.DependencyInjection
{
    public static class PluginDependencyInjection
    {
        public static IServiceCollection AddPlugin<TPlugin>(this IServiceCollection services) where TPlugin : class, IPlugin
            => services.AddSingleton<IPlugin, TPlugin>();
    }
}
