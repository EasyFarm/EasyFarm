using System;
using EasyFarm.Logging;
using Xunit;

namespace EasyFarm.Tests.Logging
{
    public class LogEntryTests
    {
        public class IncludeExceptionInMessage
        {
            [Fact]
            public void ShouldKeepUserMessageInFront()
            {
                var exception = FindExceptionWithStackTrace("exception");
                var sut = CreateSut("test", exception);

                var result = sut.IncludeExceptionInMessage();

                AssertUserMessageAtStartOfLogLine(sut, result);
            }

            [Fact]
            public void ShouldIncludeExceptionInMessage()
            {
                var exception = FindExceptionWithStackTrace("exception");
                var sut = CreateSut("test", exception);

                var result = sut.IncludeExceptionInMessage();

                Assert.Contains(sut.Exception.ToString(), result.Message);
            }

            [Fact]
            public void DoesNotPrintNewLinesWithNullException()
            {
                var exception = null as Exception;
                var sut = CreateSut("test", exception);

                var result = sut.IncludeExceptionInMessage();

                Assert.DoesNotContain(Environment.NewLine, result.Message);
            }

            private void AssertUserMessageAtStartOfLogLine(
                LogEntry original, 
                LogEntry formmatted)
            {
                var startOfMessageEntry = 
                    formmatted.Message.IndexOf(
                        original.Message, 
                        StringComparison.Ordinal);

                Assert.Equal(0, startOfMessageEntry);
            }

            private static LogEntry CreateSut(string message, Exception exception)
            {
                var sut = new LogEntry(LoggingEventType.Debug, message, exception);
                return sut;
            }

            private static Exception FindExceptionWithStackTrace(string message)
            {
                return Record.ExceptionAsync(() =>
                {
                    throw new Exception(message);
                })?.GetAwaiter().GetResult();
            }
        }
    }
}
