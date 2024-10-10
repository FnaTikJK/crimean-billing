using API.Infrastructure;
using API.Modules.LogsModule;
using Microsoft.Extensions.Logging;
using Log = API.Infrastructure.Log;

namespace LogServiceTests
{
    [TestFixture]
    public class LogsServiceTests
    {
        private ILog loggerMock;
        private LogsService logsService;

        [SetUp]
        public void SetUp()
        {
            var logger = new Log();

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
            var testMessage = "Test log message";
            var testDate = DateOnly.FromDateTime(DateTime.Now);
            var expectedLogFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", $"Log-{testDate:yyyy-MM-dd}.txt");

            logsService.WriteLog(testMessage, LogLevel.Information);
            
            Assert.IsTrue(File.Exists(expectedLogFilePath));
        }
    }
}
