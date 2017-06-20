using Symlconnect.Contracts.Diagnostics;

namespace Symlconnect.Common.Diagnostics
{
    /// <summary>
    ///     Used for design time operations where we can't surface warnings.
    /// </summary>
    public class NullLogger : ILogger
    {
        public void Log(LogEntry entry)
        {
        }
    }
}