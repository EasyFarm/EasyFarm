// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013-2017 Mykezero
// 
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////
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
