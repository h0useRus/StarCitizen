using Microsoft.Extensions.DependencyInjection;

namespace NSW.StarCitizen.Tools.API.DependencyInjection
{
    public static class StarCitizenApiDependencyInjection
    {
        public static IServiceCollection AddStarCitizenAPI(this IServiceCollection services)
            => services.AddSingleton<IStarCitizenAPI, StarCitizenAPI>();
    }
}
