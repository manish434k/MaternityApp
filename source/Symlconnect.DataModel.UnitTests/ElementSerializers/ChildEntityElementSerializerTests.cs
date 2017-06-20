using System;
using System.Xml.Linq;
using FakeItEasy;
using NUnit.Framework;
using Symlconnect.Common.Serialization;
using Symlconnect.DataModel.Serializers;

namespace Symlconnect.DataModel.UnitTests.ElementSerializers
{
    [TestFixture]
    public class ChildEntityElementSerializerTests
    {

        [Test]
        public void GoodSerialization()
        {
            // Arrange
            var instance = new ChildEntity
            {
                SessionId = "SessionIdValue",
                UserId = "UserIdValue",
                CreatedDateTime = new DateTime(2017, 1, 1)
            };

            var sut = new ChildEntityElementSerializer(new CommonValueSerializers());

            // Act
            var element = sut.SerializeToXElement(instance, null, null);

            // Assert
            var expectedElement = XElement.Parse("<childentity sessionid=\"SessionIdValue\" userid=\"UserIdValue\" createddatetime=\"2017-01-01T00:00:00.0000000Z\" />");
            Assert.IsTrue(XNode.DeepEquals(expectedElement, element), $"Expected {expectedElement} was {element}");
        }

        [Test]
        public void InvalidSerialization()
        {
            // Arrange
            var sut = new ChildEntityElementSerializer(new CommonValueSerializers());

            // Act
            var element = sut.SerializeToXElement(A.Fake<object>(), null, null);

            // Assert
            Assert.IsNull(element);
        }

        [Test]
        public void GoodSerializerType()
        {
            // Arrange
            var sut = new ChildEntityElementSerializer(new CommonValueSerializers());

            // Act
            var result = sut.IsSerializerForType(typeof(ChildEntity));

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void BadSerializerType()
        {
            // Arrange
            var sut = new ChildEntityElementSerializer(new CommonValueSerializers());

            // Act
            var result = sut.IsSerializerForType(typeof(object));

            // Assert
            Assert.IsFalse(result);
        }
    }
}