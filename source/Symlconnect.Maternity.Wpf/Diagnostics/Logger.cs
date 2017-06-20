using Symlconnect.Contracts.Diagnostics;

namespace Symlconnect.Common.Diagnostics
{
    public class Logger: ILogger
    {
        private readonly log4net.ILog _loggingProvider;

        public Logger(log4net.ILog loggingProvider)
        {
            _loggingProvider = loggingProvider;
        }

        public void Log(LogEntry entry)
        {
            if (entry.Severity == LoggingEventType.Debug)
                _loggingProvider.Debug(entry.Message, entry.Exception);
            else if (entry.Severity == LoggingEventType.Information)
                _loggingProvider.Info(entry.Message, entry.Exception);
            else if (entry.Severity == LoggingEventType.Warning)
                _loggingProvider.Warn(entry.Message, entry.Exception);
            else if (entry.Severity == LoggingEventType.Error)
                _loggingProvider.Error(entry.Message, entry.Exception);
            else
                _loggingProvider.Fatal(entry.Message, entry.Exception);
        }
    }
}