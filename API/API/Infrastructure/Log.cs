using API.Modules.LogsModule;
using Serilog;

namespace API.Infrastructure;

public interface ILog
{
    void Error(string message);
    void Info(string message);
}

public class Log : ILog
{
    private readonly Serilog.ILogger logger = new LoggerConfiguration()
        .WriteTo.File($"logs/log-{DateOnly.FromDateTime(DateTime.Now)}.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();

    public void Error(string message)
    {
        logger.Error($"{DateTime.UtcNow}: {message}", LogLevel.Error);
    }
    
    public void Info(string message)
    {
       logger.Information($"{DateTime.UtcNow}: {message}", LogLevel.Information);
    }
}