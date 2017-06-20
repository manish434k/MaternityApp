using System.Xml.Linq;
using NUnit.Framework;
using Symlconnect.Contracts.Serialization;
using Symlconnect.DataModel.Deserializers;
using Symlconnect.UnitTests.Framework.ElementDeserializers;

namespace Symlconnect.DataModel.UnitTests.ElementDeserializers
{
    [TestFixture]
    public class PropertyDefinitionElementDeserializerTests :
        ElementDeserializationTestBase<PropertyDefinition, IDataDictionary>
    {
        protected override IElementDeserializer<IDataDictionary> CreateElementDeserializerInstance()
        {
            return new PropertyDefinitionElementDeserializer(FakeFactory);
        }

        [Test]
        public void GoodDeserialization()
        {
            // Arrange
            var element = XElement.Parse("<property name=\"testpropertyid\" kind=\"text\"/>");

            // Act
            var instance = (PropertyDefinition) ElementDeserializer.DeserializeFromXElement(element, null, null);

            // Assert
            Assert.IsNotNull(instance);
            Assert.AreEqual("testpropertyid", instance.Name);
            Assert.AreEqual(ValueKind.Text, instance.PropertyDefinitionKind);
        }
    }
}