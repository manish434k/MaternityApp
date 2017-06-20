using NUnit.Framework;
using Symlconnect.Contracts.Factories;

namespace Symlconnect.DataModel.UnitTests.IntegrationTests
{
    public abstract class FactoryTestBase<T>
    {
        protected abstract IFactory<T> CreateFactoryInstance();

        [Test]
        public void CreateInstance()
        {
            // Arrange
            var sut = CreateFactoryInstance();

            // Act
            var instance = sut.CreateInstance();

            // Assert
            Assert.IsInstanceOf<T>(instance);
        }
    }
}