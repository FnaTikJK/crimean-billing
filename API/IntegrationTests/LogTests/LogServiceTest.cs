using API.Infrastructure;
using API.Modules.LogsModule;
using NUnit.Framework;

namespace IntegrationTests.LogTests
{
    [TestFixture]
    public class LogsServiceTests
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
        public void WriteLog_Should_CreateLogFile_And_WriteContent()
        {
            var testDate = DateOnly.FromDateTime(DateTime.Now);
            var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", $"Log-{testDate:yyyy-MM-dd}.txt");
            var countLogsBeforeTest = logsService.ReadLog(testDate).Split('\n').Length;

            log.Info("Test log message");
            Thread.Sleep(100);
            var countLogsAfterLogWrite = logsService.ReadLog(testDate).Split('\n').Length;
            
            Assert.That(File.Exists(logFilePath), Is.True);
            Assert.That(countLogsAfterLogWrite - countLogsBeforeTest, Is.EqualTo(1));
        }
    }
}