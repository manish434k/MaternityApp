using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using FakeItEasy;
using Symlconnect.Contracts.Diagnostics;
using Symlconnect.Contracts.Serialization;
using Symlconnect.DataModel.Deserializers;
using Symlconnect.DataModel.Factories;
using Symlconnect.DataModel.UnitTests.Properties;
using Symlconnect.DataModel.ValueProviders;

namespace Symlconnect.DataModel.UnitTests.IntegrationTests
{
    /// <summary>
    ///     Integration test for DataDictionary document deserialization
    /// </summary>
    [TestFixture]
    public class DataDictionaryDeserializationTests
    {
        private ILogger _fakeLogger;

        [SetUp]
        public void TestSetup()
        {
            _fakeLogger = A.Fake<ILogger>();
        }

        [Test]
        public void DeserializationFromDataDictionaryXmlDocument()
        {
            // Arrange
            var entityDefinitionFactory = new EntityDefinitionFactory();

            var propertyDefinitionElementDeserializer =
                new PropertyDefinitionElementDeserializer(new PropertyDefinitionFactory());

            var entityDefinitionElementDeserializer = new EntityDefinitionElementDeserializer(entityDefinitionFactory);

            var elementDeserializers = new List<IElementDeserializer<IDataDictionary>>
            {
                new DataDictionaryElementDeserializer(new DataDictionaryFactory()),
                entityDefinitionElementDeserializer,
                propertyDefinitionElementDeserializer,
                new VirtualPropertyDefinitionElementDeserializer(new VirtualPropertyDefinitionFactory()),
                new PropertyDefinitionReferenceElementDeserializer(new PropertyDefinitionReferenceFactory()),
                new PropertyValueProviderElementDeserializer(new PropertyValueProviderFactory())
            };

            var elementGroupDeserializers = new List<IElementGroupDeserializer>
            {
                entityDefinitionElementDeserializer,
                propertyDefinitionElementDeserializer
            };

            var sut = new DataDictionaryDocumentDeserializer(elementDeserializers, elementGroupDeserializers,
                _fakeLogger);

            // Act
            var dataDictionary = sut.DeserializeFromXDocument(XDocument.Parse(Resources.DataDictionaryXml));

            // Assert

            Assert.AreEqual("datadictionaryname", dataDictionary.Name);

            // Shared PropertyDefinitions
            Assert.AreEqual(5, dataDictionary.PropertyDefinitions.Count);
            Assert.AreEqual(ValueKind.Text, dataDictionary.PropertyDefinitions[0].PropertyDefinitionKind);
            Assert.AreEqual(ValueKind.Numeric, dataDictionary.PropertyDefinitions[1].PropertyDefinitionKind);
            Assert.AreEqual(ValueKind.Integer, dataDictionary.PropertyDefinitions[2].PropertyDefinitionKind);
            Assert.AreEqual(ValueKind.Boolean, dataDictionary.PropertyDefinitions[3].PropertyDefinitionKind);
            Assert.AreEqual(ValueKind.DateTime,
                dataDictionary.PropertyDefinitions[4].PropertyDefinitionKind);

            // Entities
            Assert.AreEqual(1, dataDictionary.EntityDefinitions.Count);

            // First Entity Properties
            var entityDefinition = dataDictionary.EntityDefinitions.First();
            Assert.AreEqual(7, entityDefinition.PropertyDefinitions.Count);

            Assert.AreEqual(ValueKind.Text, entityDefinition.PropertyDefinitions[0].PropertyDefinitionKind);
            Assert.AreEqual(ValueKind.Numeric,
                entityDefinition.PropertyDefinitions[1].PropertyDefinitionKind);
            Assert.AreEqual(ValueKind.Integer,
                entityDefinition.PropertyDefinitions[2].PropertyDefinitionKind);
            Assert.AreEqual(ValueKind.Boolean,
                entityDefinition.PropertyDefinitions[3].PropertyDefinitionKind);
            Assert.AreEqual(ValueKind.DateTime,
                entityDefinition.PropertyDefinitions[4].PropertyDefinitionKind);

            // 6th property is a reference to a shared property
            Assert.AreEqual(ValueKind.Text, entityDefinition.PropertyDefinitions[5].PropertyDefinitionKind);

            // 7th property is a virtual property
            Assert.IsInstanceOf<VirtualPropertyDefinition>(entityDefinition.PropertyDefinitions[6]);
            var virtualProperty = (VirtualPropertyDefinition) entityDefinition.PropertyDefinitions[6];
            Assert.IsInstanceOf<PropertyValueProvider>(virtualProperty.ValueProvider);
        }
    }
}