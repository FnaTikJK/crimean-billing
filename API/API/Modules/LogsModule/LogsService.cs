using System.Text;
using API.Infrastructure;

namespace API.Modules.LogsModule;

public interface ILogsService
{
    public void WriteInfoLog(string log);
    public void WriteErrorLog(string log);

    public string ReadLog(DateOnly date);
}

public class LogsService : ILogsService
{
    private readonly ILog logger;

    public LogsService(ILog logger)
    {
        this.logger = logger;
    }

    public void WriteInfoLog(string message)
    {
        logger.Info(message);
    }

    public void WriteErrorLog(string message)
    {
        logger.Info(message);
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
                if (line.Contains(formatDate))
                {
                    sb.AppendLine(line);
                }
            }
        }

        return sb.ToString();
    }
}