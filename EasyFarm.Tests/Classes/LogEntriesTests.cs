using System;
using System.Linq;
using System.Windows.Threading;
using EasyFarm.Classes;
using Xunit;

namespace EasyFarm.Tests.Classes
{
    public class LogEntriesTests
    {
        // ReSharper disable once InconsistentNaming
        private static readonly DateTime Jan_1st_At_08_00_36 = new DateTime(2000, 1, 1, 08, 00, 36);

        [Fact]
        public void LogEntriesUseCorrectFormat()
        {
            // Fixture setup
            var dateTime = Jan_1st_At_08_00_36;
            var sut = CreateSut();

            // Exercise system
            sut.Write(dateTime, "test");

            // Verify outcome
            VerifyLogMessageEquals(sut, "08:00:36 test");

            // Teardown
        }

        private static void VerifyLogMessageEquals(
            LogEntries logEntries, 
            string logMessage)
        {
            var result = logEntries.LoggedItems.FirstOrDefault();
            Assert.NotNull(result);
            Assert.Equal(logMessage, result);
        }

        private static LogEntries CreateSut()
        {
            return new LogEntries
            {
                DispatcherFactory = () => Dispatcher.CurrentDispatcher
            };
        }
    }    
}
