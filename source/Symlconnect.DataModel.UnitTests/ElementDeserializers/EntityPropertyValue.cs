using System;
using System.Xml.Linq;
using NUnit.Framework;
using FakeItEasy;
using Symlconnect.Common.Serialization;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;
using Symlconnect.DataModel.Deserializers;
using Symlconnect.UnitTests.Framework.ElementDeserializers;

namespace Symlconnect.DataModel.UnitTests.ElementDeserializers
{
    [TestFixture]
    public class EntityPropertyValueElementDeserializerTests :
        ElementDeserializationTestBase<EntityPropertyValue, IEntity>
    {
        protected override IElementDeserializer<IEntity> CreateElementDeserializerInstance()
        {
            var commonValueDeserializers = new CommonValueDeserializers();
            return new EntityPropertyValueElementDeserializer(A.Fake<IFactory<EntityPropertyValue>>(),
                commonValueDeserializers, commonValueDeserializers, commonValueDeserializers, commonValueDeserializers);
        }

        [Test]
        public void GoodDeserialization()
        {
            // Arrange
            var element =
                XElement.Parse(
                    "<value sessionid=\"SessionIdValue\" userid=\"UserIdValue\" changedatetime=\"2017-01-01T00:00:00.0000000Z\" />");

            // Act
            var instance =
                (EntityPropertyValue) ElementDeserializer.DeserializeFromXElement(element, null, null);

            // Assert
            Assert.IsNotNull(instance);
            Assert.AreEqual("SessionIdValue", instance.SessionId);
            Assert.AreEqual("UserIdValue", instance.UserId);
            Assert.AreEqual(new DateTime(2017, 1, 1), instance.ChangeDateTime);
        }

        [Test]
        public void SpecificValueKind()
        {
            // Arrange
            var element =
                XElement.Parse(
                    "<value sessionid=\"SessionIdValue\" userid=\"UserIdValue\" changedatetime=\"2017-01-01T00:00:00.0000000Z\" valuekind=\"datetime\">2017-01-02T00:00:00.0000000Z</value>");

            // Act
            var instance =
                (EntityPropertyValue) ElementDeserializer.DeserializeFromXElement(element, null, null);

            // Assert
            Assert.IsNotNull(instance);
            Assert.AreEqual("SessionIdValue", instance.SessionId);
            Assert.AreEqual("UserIdValue", instance.UserId);
            Assert.AreEqual(new DateTime(2017, 1, 1), instance.ChangeDateTime);
            Assert.AreEqual(new DateTime(2017, 1, 2), instance.Value);
        }

        [Test]
        public void InvalidValueKind()
        {
            // Arrange
            var element =
                XElement.Parse(
                    "<value sessionid=\"SessionIdValue\" userid=\"UserIdValue\" changedatetime=\"2017-01-01T00:00:00.0000000Z\" valuekind=\"unknown\">2017-01-02T00:00:00.0000000Z</value>");

            // Act / Assert
            Assert.Throws<InvalidOperationException>(
                () => { ElementDeserializer.DeserializeFromXElement(element, null, null); });
        }

        [Test]
        public void MissingSessionIdAttribute()
        {
            // Arrange
            var element =
                XElement.Parse("<value userid=\"UserIdValue\" changedatetime=\"2017-01-01T00:00:00.0000000Z\" />");

            // Act / Assert
            Assert.Throws<InvalidOperationException>(
                () => { ElementDeserializer.DeserializeFromXElement(element, null, null); });
        }

        [Test]
        public void MissingUserIdAttribute()
        {
            // Arrange
            var element =
                XElement.Parse("<value sessionid=\"SessionIdValue\" changedatetime=\"2017-01-01T00:00:00.0000000Z\" />");

            // Act / Assert
            Assert.Throws<InvalidOperationException>(
                () => { ElementDeserializer.DeserializeFromXElement(element, null, null); });
        }

        [Test]
        public void MissingChangeDateTimeAttribute()
        {
            // Arrange
            var element = XElement.Parse("<value sessionid=\"SessionIdValue\" userid=\"UserIdValue\" />");

            // Act / Assert
            Assert.Throws<InvalidOperationException>(
                () => { ElementDeserializer.DeserializeFromXElement(element, null, null); });
        }
    }
}