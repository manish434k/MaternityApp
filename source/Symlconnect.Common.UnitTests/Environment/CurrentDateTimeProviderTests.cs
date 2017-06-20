using System;
using NUnit.Framework;
using Symlconnect.Common.Environment;

namespace Symlconnect.DataModel.UnitTests.IntegrationTests
{
    [TestFixture]
    public class DataDictionarySerialization
    {
        [Test]
        public void TestCurrentDateTimeIsValid()
        {
            // Arrange
            var sut = new CurrentDateTimeProvider();

            // Act
            var result = sut.GetCurrentDateTime();

            // Assert
            Assert.IsTrue(DateTime.Now.Subtract(result).TotalSeconds < 1);
        }
    }
}