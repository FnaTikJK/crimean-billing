using System.Globalization;
using System.Text;
using API.Infrastructure;

namespace API.Modules.LogsModule;

public interface ILogsService
{
    public void WriteLog(string log, LogLevel level);

    public string ReadLog(DateOnly date);
}

public class LogsService : ILogsService
{
    private readonly ILog logger;
    public LogsService(ILog logger)
    {
        this.logger = logger;
    }
    public void WriteLog(string message, LogLevel level)
    {
        switch (level)
        {
            case LogLevel.Information:
                logger.Info(message);
                break;
            case LogLevel.Error:
                logger.Error(message);
                break;
        }
    }

    public string ReadLog(DateOnly date)
    {
        var filterDate = date.ToString(CultureInfo.InvariantCulture);
        var sb = new StringBuilder();
        var logFilePath = $"Logs/log-{filterDate}.txt";
        
        if(File.Exists(logFilePath))
            foreach (var line in File.ReadLines(logFilePath))
            {
                if (line.Contains(filterDate))
                {
                    sb.Append(line + '\n');
                    break;
                }
            }

        return sb.ToString();
    }
}