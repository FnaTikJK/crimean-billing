using System.Text;
using API.Infrastructure;

namespace API.Modules.LogsModule;

public interface ILogsService
{
    public string ReadLog(DateOnly date);
}

public class LogsService : ILogsService
{
    private readonly ILog log;

    public LogsService(ILog logger)
    {
        this.log = logger;
    }

    public string ReadLog(DateOnly date)
    {
        var formatDate = date.ToString("yyyy-MM-dd");
        var sb = new StringBuilder();
        var logFilePath = $"Logs/log-{formatDate}.txt";

        if (File.Exists(logFilePath))
        {
            using var stream = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(stream);
            while (reader.ReadLine() is { } line)
            {
                sb.AppendLine(line);
            }
        }

        return sb.ToString();
    }
}