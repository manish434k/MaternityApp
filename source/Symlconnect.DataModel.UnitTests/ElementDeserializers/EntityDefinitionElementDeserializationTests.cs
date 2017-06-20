using System.Xml.Linq;
using NUnit.Framework;
using Symlconnect.Contracts.Serialization;
using Symlconnect.DataModel.Deserializers;
using Symlconnect.UnitTests.Framework.ElementDeserializers;

namespace Symlconnect.DataModel.UnitTests.ElementDeserializers
{
    [TestFixture]
    public class EntityDefinitionElementDeserializationTests :
        ElementDeserializationTestBase<EntityDefinition, IDataDictionary>
    {
        protected override IElementDeserializer<IDataDictionary> CreateElementDeserializerInstance()
        {
            return new EntityDefinitionElementDeserializer(FakeFactory);
        }

        [Test]
        public void DeserializeFromElement()
        {
            // Arrange
            var element = XElement.Parse("<entitydefinition/>");

            // Act
            var instance = ElementDeserializer.DeserializeFromXElement(element, null, null);

            // Assert
            Assert.IsNotNull(instance);
        }
    }
}