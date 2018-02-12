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
using NLog;

namespace EasyFarm.Logging
{
    public class NLogTextFileLogger : ILogger
    {
        private readonly NLog.Logger _logger = LogManager.GetCurrentClassLogger();

        public void Log(LogEntry logEntry)
        {
            var formattedLogEntry = logEntry.IncludeExceptionInMessage();
            var message = formattedLogEntry.Message;
            var exception = formattedLogEntry.Exception;

            switch (logEntry.Severity)
            {
                case LoggingEventType.Debug:
                    _logger.Debug(exception, message);
                    break;
                case LoggingEventType.Information:
                    _logger.Info(exception, message);
                    break;
                case LoggingEventType.Warning:
                    _logger.Warn(exception, message);
                    break;
                case LoggingEventType.Error:
                    _logger.Error(exception, message);
                    break;
                case LoggingEventType.Fatal:
                    _logger.Fatal(exception, message);
                    break;
                default:
                    _logger.Info(exception, message);
                    break;
            }
        }
    }
}