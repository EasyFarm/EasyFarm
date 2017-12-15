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
