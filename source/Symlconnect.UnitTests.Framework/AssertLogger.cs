using NUnit.Framework;
using Symlconnect.Contracts.Diagnostics;

namespace Symlconnect.UnitTests.Framework
{
    public class AssertLogger : ILogger
    {
        public void Log(LogEntry entry)
        {
            if (entry.Severity == LoggingEventType.Error || entry.Severity == LoggingEventType.Warning ||
                entry.Severity == LoggingEventType.Fatal)
            {
                Assert.Fail($"Log Message: {entry.Severity} - {entry.Message}");
            }
        }
    }
}