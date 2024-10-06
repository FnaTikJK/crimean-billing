namespace API.Modules.CacheModule;

public class CacheModule : IModule
{
    public void RegisterModule(IServiceCollection services)
    {
        var isDebug = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") is "Development"
            or null;

        if (isDebug)
            services.AddSingleton<ICache, CacheInMemory>();
        else
            services.AddSingleton<ICache, DistributedCache>();
    }
}