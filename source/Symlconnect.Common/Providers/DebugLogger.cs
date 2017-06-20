using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Prism.Logging;

namespace Symlconnect.Common.Providers
{
    [ExcludeFromCodeCoverage]
    public class DebugLogger : ILoggerFacade
    {
        public void Log(string message, Category category, Priority priority)
        {
            Debug.WriteLine($"{category}-{priority} : {message}");
        }
    }
}