using System;
using System.Xml.Linq;
using NUnit.Framework;
using FakeItEasy;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;
using Symlconnect.ViewModel;
using Symlconnect.UnitTests.Framework.ElementDeserializers;
using Symlconnect.ViewModel.Deserializers;

namespace Symlconnect.DataModel.UnitTests.ElementDeserializers
{
    [TestFixture]
    public class FormDefinitionElementDeserializerTests :
        ElementDeserializationTestBase<IFormDefinition, IFormDefinition>
    {
        protected override IElementDeserializer<IFormDefinition> CreateElementDeserializerInstance()
        {
            return new FormDefinitionElementDeserializer(A.Fake<IFactory<FormDefinition>>());
        }

        [Test]
        public void GoodDeserialization()
        {
            // Arrange
            var element = XElement.Parse("<form id=\"idvalue\" datadictionary=\"datadictionaryvalue\" />");

            // Act
            var instance =
                (IFormDefinition) ElementDeserializer.DeserializeFromXElement(element, null, null);

            // Assert
            Assert.IsNotNull(instance);
            Assert.AreEqual("idvalue", instance.Id);
            Assert.AreEqual("datadictionaryvalue", instance.DataDictionaryName);
        }

        [Test]
        public void MissingIdAttribute()
        {
            // Arrange
            var element = XElement.Parse("<form datadictionary=\"datadictionaryvalue\" />");

            // Act
            var instance =
                (IFormDefinition) ElementDeserializer.DeserializeFromXElement(element, null, null);

            // Assert
            Assert.IsNotNull(instance);
            Assert.IsNotNull(instance.Id);
        }

        [Test]
        public void MissingDataDictionaryAttribute()
        {
            // Arrange
            var element = XElement.Parse("<form />");

            // Act / Assert
            Assert.Throws<InvalidOperationException>(
                () => { ElementDeserializer.DeserializeFromXElement(element, null, null); });
        }

        [Test]
        public void ElementName()
        {
            // Arrange/Act/Assert
            Assert.AreEqual("form", ElementDeserializer.ElementName);
        }
    }
}