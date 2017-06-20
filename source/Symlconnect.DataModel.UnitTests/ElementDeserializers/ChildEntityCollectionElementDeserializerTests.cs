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
    public class ChildEntityCollectionElementDeserializerTests :
        ElementDeserializationTestBase<ChildEntityCollection, IEntity>
    {
        protected override IElementDeserializer<IEntity> CreateElementDeserializerInstance()
        {
            return new ChildEntityCollectionElementDeserializer(A.Fake<IFactory<ChildEntityCollection>>());
        }

        [Test]
        public void GoodDeserialization()
        {
            // Arrange
            var element = XElement.Parse("<childentities propertyname=\"PropertyNameValue\" />");

            // Act
            var instance =
                (ChildEntityCollection) ElementDeserializer.DeserializeFromXElement(element, null, null);

            // Assert
            Assert.IsNotNull(instance);
            Assert.AreEqual("PropertyNameValue", instance.PropertyName);
        }

        [Test]
        public void MissingPropertyNameAttribute()
        {
            // Arrange
            var element = XElement.Parse("<childentities />");

            // Act / Assert
            Assert.Throws<InvalidOperationException>(
                () => { ElementDeserializer.DeserializeFromXElement(element, null, null); });
        }
    }
}