using System;
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
    public class EntityPropertyValueCollectionElementDeserializerTests :
        ElementDeserializationTestBase<EntityPropertyValueCollection, IEntity>
    {
        protected override IElementDeserializer<IEntity> CreateElementDeserializerInstance()
        {
            return
                new EntityPropertyValueCollectionElementDeserializer(A.Fake<IFactory<EntityPropertyValueCollection>>());
        }

        [Test]
        public void GoodDeserialization()
        {
            // Arrange
            var element = XElement.Parse("<element propertyname=\"propertynamevalue\" />");

            // Act
            var instance =
                (EntityPropertyValueCollection) ElementDeserializer.DeserializeFromXElement(element, null, null);

            // Assert
            Assert.IsNotNull(instance);
            Assert.AreEqual("propertynamevalue", instance.PropertyName);
        }

        [Test]
        public void MissingPropertyNameAttribute()
        {
            // Arrange
            var element = XElement.Parse("<element />");

            // Act / Assert
            Assert.Throws<InvalidOperationException>(
                () => { ElementDeserializer.DeserializeFromXElement(element, null, null); });
        }
    }
}