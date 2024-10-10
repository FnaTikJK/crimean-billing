using API.Infrastructure.Config;

namespace API.Modules.CacheModule;

public class CacheModule : IModule
{
    public void RegisterModule(IServiceCollection services)
    {
        var isCacheInMemory = Config.IsDebug || true;

        if (isCacheInMemory)
            services.AddSingleton<ICache, CacheInMemory>();
        else
            services.AddSingleton<ICache, DistributedCache>();
    }
}