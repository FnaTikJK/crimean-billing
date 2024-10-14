using API.Infrastructure;
using API.Modules.LogsModule;
using Serilog;
using Log = API.Infrastructure.Log;

public class LogsServiceMultithreadingTests
{
    private ILog logger;
    private LogsService logsService;

    [SetUp]
    public void SetUp()
    {
        logger = new Log();
        logsService = new LogsService(logger);
        var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }
    }

    [Test]
    public async Task MultipleLoggers_Should_WriteToSameLogFile_WithoutErrors_WithFourLoggers()
    {
        var testDate = DateOnly.FromDateTime(DateTime.Now);
        var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", $"Log-{testDate:yyyy-MM-dd}.txt");
        var countLogsBeforeTest = logsService.ReadLog(testDate).Split('\n').Length;
        var tasks = new List<Task>();
        for (int i = 1; i <= 4; i++)
        {
            var loggerIndex = i;
            tasks.Add(Task.Run(() => { logger.Info($"Log from Logger {loggerIndex}"); }));
        }

        await Task.WhenAll(tasks);

        Thread.Sleep(100);
        var countLogsAfterLogWrite = logsService.ReadLog(testDate).Split('\n').Length;
        Assert.IsTrue(File.Exists(logFilePath));
        Assert.AreEqual(4, countLogsAfterLogWrite - countLogsBeforeTest);
    }
}