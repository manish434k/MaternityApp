﻿using System;
using System.Diagnostics.CodeAnalysis;

namespace Symlconnect.Contracts.Diagnostics
{
    [ExcludeFromCodeCoverage]
    public static class LoggerExtensions
    {
        public static void Log(this ILogger logger, string message)
        {
            logger.Log(new LogEntry(LoggingEventType.Information, message));
        }

        public static void Log(this ILogger logger, Exception exception)
        {
            logger.Log(new LogEntry(LoggingEventType.Error, exception.Message, exception));
        }

        public static void Log(this ILogger logger, LoggingEventType eventType, string message)
        {
            logger.Log(new LogEntry(eventType, message));
        }
    }
}