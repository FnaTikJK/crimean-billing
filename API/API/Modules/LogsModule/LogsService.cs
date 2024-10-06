using System.Globalization;
using System.Text;
using Serilog;

namespace API.Modules.LogsModule;

public interface ILogsService
{
    public void WriteLog(string log);

    public string ReadLog(DateOnly date);
}

public class LogsService : ILogsService
{
    private readonly string _path; 
    private readonly Serilog.ILogger _logger;

    public LogsService(string path)
    {
        _path = path;
        _logger = new LoggerConfiguration()
            .WriteTo.File(path)
            .CreateLogger();
    }
    public void WriteLog(string log)
    {
        _logger.Error(log);
    }

    public string ReadLog(DateOnly date)
    {
        var filterDate = date.ToString(CultureInfo.InvariantCulture);
        var sb = new StringBuilder();
        
        foreach (var line in File.ReadLines(_path))
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