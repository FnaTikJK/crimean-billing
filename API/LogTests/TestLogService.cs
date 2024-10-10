using API.Infrastructure;
using API.Modules.LogsModule;
using Moq;
using Serilog;
using NUnit.Framework;
using System.IO;
using Microsoft.Extensions.Logging;
using Log = Serilog.Log;

namespace LogServiceTests
{
    [TestFixture]
    public class LogsServiceTests
    {
        private Mock<ILog> loggerMock;
        private LogsService logsService;

        [SetUp]
        public void SetUp()
        {
            // Инициализация мок объекта ILog
            loggerMock = new Mock<ILog>();

            // Инициализация сервиса логирования
            logsService = new LogsService(loggerMock.Object);

            // Создаем директорию "Logs", если её нет
            var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        [Test]
        public void WriteLog_Should_CreateLogFile_And_WriteContent()
        {
            // Arrange
            var testMessage = "Test log message";
            var testDate = DateOnly.FromDateTime(DateTime.Now);
            var expectedLogFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", $"Log-{testDate:yyyy-MM-dd}.txt");

            // Act
            logsService.WriteLog(testMessage, LogLevel.Information);

            // Завершаем запись логов
            Log.CloseAndFlush();

            // Assert: убедимся, что файл создан
            Assert.IsTrue(File.Exists(expectedLogFilePath));

            // Assert: проверим, что в файле есть записанное сообщение
            var fileContent = File.ReadAllText(expectedLogFilePath);
            Assert.IsTrue(fileContent.Contains(testMessage));
        }

        [TearDown]
        public void TearDown()
        {
            // Удаляем файл логов после выполнения тестов, если он был создан
            var testDate = DateOnly.FromDateTime(DateTime.Now);
            var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", $"Log-{testDate:yyyy-MM-dd}.txt");

            if (File.Exists(logFilePath))
            {
                File.Delete(logFilePath);
            }
        }
    }
}
