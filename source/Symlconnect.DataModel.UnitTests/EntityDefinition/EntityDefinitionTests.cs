using System.Linq;
using NUnit.Framework;
using FakeItEasy;

namespace Symlconnect.DataModel.UnitTests.IntegrationTests
{
    [TestFixture]
    public class EntityDefinitionTests
    {
        [SetUp]
        public void TestSetup()
        {
        }

        [Test]
        public void GetChildItems()
        {
            // Arrange
            var sut = new EntityDefinition();
            var propertyDefinition = A.Fake<PropertyDefinition>();
            sut.AddChildItem(propertyDefinition);

            // Act
            var childItems = sut.GetChildItems();

            // Assert
            Assert.AreEqual(1, childItems.Cast<object>().Count());
            CollectionAssert.AreEqual(new object[] {propertyDefinition}, childItems);
        }
    }
}