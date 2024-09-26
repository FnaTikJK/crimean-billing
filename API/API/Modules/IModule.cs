namespace API.Modules;

public interface IModule
{
    void RegisterModule(IServiceCollection services);
}

public interface IHubModule : IModule
{
    void ConfigureHubs(WebApplication app);
}

public static class ModuleExtensions
{
    private static List<IModule> modules;
    private static List<IModule> Modules => modules ??= DiscoverModules().ToList();

    public static void RegisterModules(this IServiceCollection services)
    {
        foreach (var module in Modules)
        {
            module.RegisterModule(services);
        }
    }

    public static void ConfigureHubs(this WebApplication app)
    {
        foreach (var module in Modules.Where(m => m is IHubModule).Cast<IHubModule>())
        {
            module.ConfigureHubs(app);
        }
    }

    private static IEnumerable<IModule> DiscoverModules()
    {
        return typeof(IModule).Assembly
            .GetTypes()
            .Where(p => p.IsClass && p.IsAssignableTo(typeof(IModule)))
            .Select(Activator.CreateInstance)
            .Cast<IModule>();
    }
}