using System;
using System.Globalization;
using System.Xml.Linq;
using NUnit.Framework;
using FakeItEasy;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;
using Symlconnect.DataModel.Deserializers;
using Symlconnect.UnitTests.Framework.ElementDeserializers;

namespace Symlconnect.DataModel.UnitTests.ElementDeserializers
{
    [TestFixture]
    public class ChildEntityElementDeserializerTests :
        ElementDeserializationTestBase<ChildEntity, IEntity>
    {
        protected override IElementDeserializer<IEntity> CreateElementDeserializerInstance()
        {
            var fakeValueDeserializer = A.Fake<IValueDeserializer<DateTime>>();

            A.CallTo(() => fakeValueDeserializer.DeserializeValue(A<string>.Ignored))
                .ReturnsLazily((string text) => DateTime.Parse(text, CultureInfo.InvariantCulture,
                    DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal));

            return new ChildEntityElementDeserializer(fakeValueDeserializer, A.Fake<IFactory<ChildEntity>>());
        }

        [Test]
        public void GoodDeserialization()
        {
            // Arrange
            var element =
                XElement.Parse(
                    "<childentity sessionid=\"SessionIdValue\" userid=\"UserIdValue\" createddatetime=\"2017-01-01T00:00:00.0000000Z\" />");

            // Act
            var instance =
                (ChildEntity) ElementDeserializer.DeserializeFromXElement(element, null, null);

            // Assert
            Assert.IsNotNull(instance);
            Assert.AreEqual("SessionIdValue", instance.SessionId);
            Assert.AreEqual("UserIdValue", instance.UserId);
            Assert.AreEqual(new DateTime(2017, 1, 1), instance.CreatedDateTime);
        }

        [Test]
        public void MissingSessionIdAttribute()
        {
            // Arrange
            var element =
                XElement.Parse("<childentity userid=\"UserIdValue\" createddatetime=\"2017-01-01T00:00:00.0000000Z\" />");

            // Act/Assert
            Assert.Throws<InvalidOperationException>(
                () => { ElementDeserializer.DeserializeFromXElement(element, null, null); });
        }

        [Test]
        public void MissingUserIdAttribute()
        {
            // Arrange
            var element =
                XElement.Parse(
                    "<childentity sessionid=\"SessionIdValue\" createddatetime=\"2017-01-01T00:00:00.0000000Z\" />");

            // Act/Assert
            Assert.Throws<InvalidOperationException>(
                () => { ElementDeserializer.DeserializeFromXElement(element, null, null); });
        }

        [Test]
        public void MissingCreatedDateTimeAttribute()
        {
            // Arrange
            var element = XElement.Parse("<childentity sessionid=\"SessionIdValue\" userid=\"UserIdValue\" />");

            // Act/Assert
            Assert.Throws<InvalidOperationException>(
                () => { ElementDeserializer.DeserializeFromXElement(element, null, null); });
        }
    }
}