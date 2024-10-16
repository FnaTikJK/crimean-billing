using Serilog;

namespace API.Infrastructure;

public interface ILog
{
    void Error(string message);
    void Info(string message);
}

public class Log : ILog
{
    private readonly Serilog.ILogger serilog;

    public Log()
    {
        var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", $"Log-{DateTime.UtcNow:yyyy-MM-dd}.txt");
        serilog = new LoggerConfiguration()
            .WriteTo.Async(a => a.File(logFilePath, shared: true))
            .CreateBootstrapLogger();
    }

    public void Error(string message)
    {
        serilog.Error($"{DateTime.UtcNow}: {message}");
    }

    public void Info(string message)
    {
        serilog.Information($"{DateTime.UtcNow}: {message}");
    }
}