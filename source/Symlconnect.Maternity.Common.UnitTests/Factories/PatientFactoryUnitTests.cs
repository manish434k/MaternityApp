using NUnit.Framework;
using Symlconnect.Maternity.Common.Factories;

namespace Symlconnect.Maternity.Common.UnitTests
{
    [TestFixture]
    public class PatientFactoryUnitTests
    {
        [Test]
        public void CreateInstance()
        {
            // Arrange
            var sut = new PatientFactory();

            // Act
            var instance = sut.CreateInstance();

            // Assert
            Assert.IsInstanceOf<IPatient>(instance);
        }
    }
}