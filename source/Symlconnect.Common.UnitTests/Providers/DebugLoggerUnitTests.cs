using NUnit.Framework;
using Prism.Logging;

namespace Symlconnect.Common.UnitTests
{
    [TestFixture]
    public class DebugLoggerUnitTests
    {
        [Test]
        public void LogMessage()
        {
            // Arrange
            var sut = new DebugLogger();

            sut.Log("testmessage", Category.Debug, Priority.High);
        }
    }
}