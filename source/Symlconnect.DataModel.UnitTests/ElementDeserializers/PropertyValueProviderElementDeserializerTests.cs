using System;
using System.Xml.Linq;
using NUnit.Framework;
using FakeItEasy;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;
using Symlconnect.DataModel.Deserializers;
using Symlconnect.DataModel.ValueProviders;
using Symlconnect.UnitTests.Framework.ElementDeserializers;

namespace Symlconnect.DataModel.UnitTests.ElementDeserializers
{
    [TestFixture]
    public class PropertyValueProviderElementDeserializerTests :
        ElementDeserializationTestBase<PropertyValueProvider, DataDictionary>
    {
        protected override IElementDeserializer<DataDictionary> CreateElementDeserializerInstance()
        {
            return new PropertyValueProviderElementDeserializer(A.Fake<IFactory<PropertyValueProvider>>());
        }

        [Test]
        public void GoodDeserialization()
        {
            // Arrange
            var element =
                XElement.Parse("<propertyvalue name=\"propertyname\" entityname=\"entityname\" kind=\"DateTime\" />");

            // Act
            var propertyValueProvider =
                (PropertyValueProvider) ElementDeserializer.DeserializeFromXElement(element, null, null);

            // Assert
            Assert.IsNotNull(propertyValueProvider);
            Assert.AreEqual("entityname", propertyValueProvider.EntityName);
            Assert.AreEqual("propertyname", propertyValueProvider.PropertyName);
            Assert.AreEqual(ValueKind.DateTime, propertyValueProvider.ValueProviderKind);
        }

        [Test]
        public void MissingValueKind()
        {
            // Arrange
            var element =
                XElement.Parse("<propertyvalue name=\"propertyname\" entityname=\"entityname\" />");

            // Act
            var propertyValueProvider =
                (PropertyValueProvider)ElementDeserializer.DeserializeFromXElement(element, null, null);

            // Assert
            Assert.IsNotNull(propertyValueProvider);
            Assert.AreEqual("entityname", propertyValueProvider.EntityName);
            Assert.AreEqual("propertyname", propertyValueProvider.PropertyName);
            Assert.AreEqual(ValueKind.Unknown, propertyValueProvider.ValueProviderKind);
        }

        [Test]
        public void WithoutEntityNameAttribute()
        {
            // Arrange
            var element = XElement.Parse("<propertyvalue name=\"propertyname\" kind=\"DateTime\" />");

            // Act
            var propertyValueProvider =
                (PropertyValueProvider) ElementDeserializer.DeserializeFromXElement(element, null, null);

            // Assert
            Assert.IsNotNull(propertyValueProvider);
            Assert.AreEqual(null, propertyValueProvider.EntityName);
            Assert.AreEqual("propertyname", propertyValueProvider.PropertyName);
            Assert.AreEqual(ValueKind.DateTime, propertyValueProvider.ValueProviderKind);
        }

        [Test]
        public void MissingNameAttribute()
        {
            // Arrange
            var element = XElement.Parse("<propertyvalue kind=\"DateTime\" />");

            // Act / Assert
            Assert.Throws<InvalidOperationException>(
                () => { ElementDeserializer.DeserializeFromXElement(element, null, null); });
        }

        [Test]
        public void MissingKindAttribute()
        {
            // Arrange
            var element = XElement.Parse("<propertyvalue name=\"propertyname\" />");

            // Act
            var propertyValueProvider =
                (PropertyValueProvider) ElementDeserializer.DeserializeFromXElement(element, null, null);

            // Assert
            Assert.IsNotNull(propertyValueProvider);
            Assert.AreEqual(null, propertyValueProvider.EntityName);
            Assert.AreEqual("propertyname", propertyValueProvider.PropertyName);
        }
    }
}