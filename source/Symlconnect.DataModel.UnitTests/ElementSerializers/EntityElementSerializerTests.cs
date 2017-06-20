using System;
using System.Xml.Linq;
using FakeItEasy;
using NUnit.Framework;
using Symlconnect.Common.Serialization;
using Symlconnect.DataModel.Serializers;

namespace Symlconnect.DataModel.UnitTests.ElementSerializers
{
    [TestFixture]
    public class EntityElementSerializerTests
    {
        [Test]
        public void GoodSerialization()
        {
            // Arrange
            var fakeEntityDefinition = A.Fake<IEntityDefinition>();
            A.CallTo(() => fakeEntityDefinition.EntityName).Returns("EntityNameValue");
            A.CallTo(() => fakeEntityDefinition.DataDictionary.Name).Returns("DataDictionaryValue");

            var instance = A.Fake<IEntity>();
            A.CallTo(() => instance.Id).Returns("EntityIdValue");
            A.CallTo(() => instance.CreatedDateTime).Returns(new DateTime(2017, 1, 2));
            A.CallTo(() => instance.CreatedByUserId).Returns("CreatedByUserIdValue");
            A.CallTo(() => instance.CreatedByUserDisplayName).Returns("CreatedByUserDisplayNameValue");

            A.CallTo(() => instance.EntityDefinition).Returns(fakeEntityDefinition);

            var sut = new EntityElementSerializer(new CommonValueSerializers());

            // Act
            var element = sut.SerializeToXElement(instance, null, null);

            // Assert
            var expectedElement =
                XElement.Parse(
                    "<entity id=\"EntityIdValue\" datadictionary=\"DataDictionaryValue\" entityname=\"EntityNameValue\" createddatetime=\"2017-01-02T00:00:00.0000000Z\" createdbyuserid=\"CreatedByUserIdValue\" createdbyuserdisplayname=\"CreatedByUserDisplayNameValue\" />");
            Assert.IsTrue(XNode.DeepEquals(expectedElement, element), $"Expected {expectedElement} was {element}");
        }

        [Test]
        public void InvalidSerialization()
        {
            // Arrange
            var sut = new EntityElementSerializer(new CommonValueSerializers());

            // Act
            var element = sut.SerializeToXElement(A.Fake<object>(), null, null);

            // Assert
            Assert.IsNull(element);
        }

        [Test]
        public void GoodSerializerType()
        {
            // Arrange
            var sut = new EntityElementSerializer(new CommonValueSerializers());

            // Act
            var result = sut.IsSerializerForType(typeof(Entity));

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void BadSerializerType()
        {
            // Arrange
            var sut = new EntityElementSerializer(new CommonValueSerializers());

            // Act
            var result = sut.IsSerializerForType(typeof(object));

            // Assert
            Assert.IsFalse(result);
        }
    }
}