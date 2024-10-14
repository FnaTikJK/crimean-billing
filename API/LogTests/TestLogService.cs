using API.Infrastructure;
using API.Modules.LogsModule;
using Microsoft.Extensions.Logging;
using Log = API.Infrastructure.Log;

namespace LogServiceTests
{
    [TestFixture]
    public class LogsServiceTests
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
        public void WriteLog_Should_CreateLogFile_And_WriteContent()
        {
            var testDate = DateOnly.FromDateTime(DateTime.Now);
            var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", $"Log-{testDate:yyyy-MM-dd}.txt");
            var countLogsBeforeTest = logsService.ReadLog(testDate).Split('\n').Length;
            
            logger.Info("Test log message");
            Thread.Sleep(100);
            var countLogsAfterLogWrite = logsService.ReadLog(testDate).Split('\n').Length;
            
            Assert.IsTrue(File.Exists(logFilePath));
            Assert.AreEqual(1, countLogsAfterLogWrite-countLogsBeforeTest);
        }
    }
}