using API.Infrastructure;
using API.Modules.LogsModule;
using NUnit.Framework;

namespace IntegrationTests.LogTests;

[TestFixture]
public class LogsServiceMultithreadingTests
{
    private ILog log;
    private LogsService logsService;

    [SetUp]
    public void SetUp()
    {
        log = new Log();
        logsService = new LogsService(log);
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
        for (int i = 1; i <= 20; i++)
        {
            var loggerIndex = i;
            tasks.Add(Task.Run(() => { log.Info($"Log from Logger {loggerIndex}"); }));
        }

        await Task.WhenAll(tasks);

        Thread.Sleep(100);
        var countLogsAfterLogWrite = logsService.ReadLog(testDate).Split('\n').Length;
        Assert.That(File.Exists(logFilePath), Is.True);
        Assert.That(countLogsAfterLogWrite - countLogsBeforeTest, Is.EqualTo(20));
    }
}