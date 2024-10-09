using System.Globalization;
using System.Text;
using API.Infrastructure;
using Serilog;
using LoggerExtensions = Microsoft.Extensions.Logging.LoggerExtensions;

namespace API.Modules.LogsModule;

public interface ILogsService
{
    public void WriteLog(string log, LogLevel level);

    public string ReadLog(DateOnly date);
}

public class LogsService : ILogsService
{
    private readonly string path = "API/Logs";
    private readonly ILog logger;

    public LogsService(string path, ILog logger)
    {
        this.path = path;
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
        var logFilePath = $"{path}/log-{filterDate}.txt";
        
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