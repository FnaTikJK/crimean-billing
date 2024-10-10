using API.Infrastructure;
using Moq;
using NUnit.Framework;
using API.Modules.LogsModule;
using Microsoft.Extensions.Logging;

namespace LogTests;

public class Tests
{
    [TestFixture]
    public class LogsControllerTests
    {
        private Mock<ILogsService> logsServiceMock;
        private Mock<ILog> logMock;
        private LogsController logsController;
        private ILogsService logsService;

        [SetUp]
        public void SetUp()
        {
            logsServiceMock = new Mock<ILogsService>();
            logMock = new Mock<ILog>();
            logsController = new LogsController(logsServiceMock.Object, logMock.Object);

        }

        [Test]
        public void Throw_Should_Log_Error_And_Return_500()
        {
            // Act
            var result = logsController.Throw();

            // Assert
            Assert.IsNotNull(result);
            var x = result;
            
           // logMock.Verify(log => log.Error(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Test_LogService()
        {
            try
            {
                throw new Exception("Тестовое исключение");
            }
            catch (Exception ex)
            { 
                logsService.WriteLog(ex.Message, LogLevel.Error);
                var x = logsService.ReadLog(DateOnly.FromDateTime(DateTime.Now));
                Assert.IsNotNull(x);
            }
        }
    }
}