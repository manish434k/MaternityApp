using System;
using System.Xml.Linq;
using NUnit.Framework;
using FakeItEasy;
using Symlconnect.Contracts.Serialization;
using Symlconnect.DataModel.Deserializers;
using Symlconnect.UnitTests.Framework.ElementDeserializers;

namespace Symlconnect.DataModel.UnitTests.ElementDeserializers
{
    [TestFixture]
    public class PropertyDefinitionReferenceElementDeserializerTests :
        ElementDeserializationTestBase<PropertyDefinitionReference, IDataDictionary>
    {
        private PropertyDefinitionCollection _propertyDefinitions;

        protected override IElementDeserializer<IDataDictionary> CreateElementDeserializerInstance()
        {
            return new PropertyDefinitionReferenceElementDeserializer(FakeFactory);
        }

        [Test]
        public void GoodDeserialization()
        {
            // Arrange
            var element = XElement.Parse("<propertyref name=\"propertyrefname\" refname=\"sharedpropertyid\" />");
            var fakeDataDictionary = A.Fake<IDataDictionary>();
            _propertyDefinitions = new PropertyDefinitionCollection();
            A.CallTo(() => fakeDataDictionary.PropertyDefinitions).Returns(_propertyDefinitions);
            var fakeSharedPropertyDefinition = A.Fake<IPropertyDefinition>();
            A.CallTo(() => fakeSharedPropertyDefinition.Name).Returns("sharedpropertyid");
            A.CallTo(() => fakeSharedPropertyDefinition.PropertyDefinitionKind).Returns(ValueKind.DateTime);
            _propertyDefinitions.Add(fakeSharedPropertyDefinition);

            // Act
            var instance =
                (PropertyDefinitionReference)
                ElementDeserializer.DeserializeFromXElement(element, null, fakeDataDictionary);

            // Assert
            Assert.IsNotNull(instance);
            Assert.AreEqual("propertyrefname", instance.Name);
            Assert.AreEqual(ValueKind.DateTime, instance.PropertyDefinitionKind);
        }

        [Test]
        public void NullElement()
        {
            // Arrange / Act / Asssert
            Assert.Throws<InvalidOperationException>(
                () => { ElementDeserializer.DeserializeFromXElement(null, null, null); });
        }

        [Test]
        public void MissingRefName()
        {
            var element = XElement.Parse("<propertyref name=\"propertyrefname\" />");

            // Arrange / Act / Asssert
            Assert.Throws<InvalidOperationException>(
                () => { ElementDeserializer.DeserializeFromXElement(element, null, null); });
        }

        [Test]
        public void MissingName()
        {
            var element = XElement.Parse("<propertyref refname=\"sharedpropertyid\" />");

            // Arrange / Act / Asssert
            Assert.Throws<InvalidOperationException>(
                () => { ElementDeserializer.DeserializeFromXElement(element, null, null); });
        }

        [Test]
        public void MissingReferencedPropertyDefinition()
        {
            // Arrange
            var element = XElement.Parse("<propertyref name=\"propertyrefname\" refname=\"sharedpropertyid\" />");
            var fakeDataDictionary = A.Fake<IDataDictionary>();

            // Act / Asssert
            Assert.Throws<InvalidOperationException>(
                () => { ElementDeserializer.DeserializeFromXElement(element, null, fakeDataDictionary); });
        }
    }
}