using NUnit.Framework;
using FakeItEasy;

namespace Symlconnect.DataModel.UnitTests.IntegrationTests
{
    [TestFixture]
    public class DataDictionaryTests
    {
        [Test]
        public void AddChildItems()
        {
            // Arrange
            var propertyDefinition = A.Fake<IPropertyDefinition>();
            var entityDefinition = A.Fake<IEntityDefinition>();
            var sut = new DataDictionary();

            // Act
            sut.AddChildItem(propertyDefinition);
            sut.AddChildItem(entityDefinition);

            // Assert
            CollectionAssert.AreEqual(new object[] { entityDefinition, propertyDefinition }, sut.GetChildItems());
        }
    }
}