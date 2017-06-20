using System.Xml.Linq;
using FakeItEasy;
using NUnit.Framework;
using Symlconnect.DataModel.Serializers;

namespace Symlconnect.DataModel.UnitTests.ElementSerializers
{
    [TestFixture]
    public class ChildEntityCollectionSerializerTests
    {
        [Test]
        public void GoodSerialization()
        {
            // Arrange
            var collection = new ChildEntityCollection {PropertyName = "ChildEntityCollectionPropertyName"};

            var sut = new ChildEntityCollectionElementSerializer();

            // Act
            var element = sut.SerializeToXElement(collection, null, null);

            // Assert
            var expectedElement = XElement.Parse("<childentities propertyname=\"ChildEntityCollectionPropertyName\"/>");
            Assert.IsTrue(XNode.DeepEquals(expectedElement, element), $"Expected {expectedElement} was {element}");
        }

        [Test]
        public void InvalidSerialization()
        {
            // Arrange
            var sut = new ChildEntityCollectionElementSerializer();

            // Act
            var element = sut.SerializeToXElement(A.Fake<object>(), null, null);

            // Assert
            Assert.IsNull(element);
        }

        [Test]
        public void GoodSerializerType()
        {
            // Arrange
            var sut = new ChildEntityCollectionElementSerializer();

            // Act
            var result = sut.IsSerializerForType(typeof(ChildEntityCollection));

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void BadSerializerType()
        {
            // Arrange
            var sut = new ChildEntityCollectionElementSerializer();

            // Act
            var result = sut.IsSerializerForType(typeof(object));

            // Assert
            Assert.IsFalse(result);
        }
    }
}