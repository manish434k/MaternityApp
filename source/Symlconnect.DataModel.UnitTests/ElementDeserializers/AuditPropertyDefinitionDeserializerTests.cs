using NUnit.Framework;
using FakeItEasy;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;
using Symlconnect.DataModel.Deserializers;
using System.Xml.Linq;
using Symlconnect.Common.Serialization;
using Symlconnect.UnitTests.Framework.ElementDeserializers;

namespace Symlconnect.DataModel.UnitTests.ElementDeserializers
{
    [TestFixture]
    public class AuditPropertyDefinitionDeserializerTests :
        ElementDeserializationTestBase<AuditPropertyDefinition, IDataDictionary>
    {
        protected override IElementDeserializer<IDataDictionary> CreateElementDeserializerInstance()
        {
            return new AuditPropertyDefinitionElementDeserializer(FakeFactory, A.Fake<IFactory<PropertyDefinition>>());
        }

        [Test]
        public void GoodDeserialization()
        {
            // Arrange
            var element = XElement.Parse("<auditproperty name=\"testauditproperty\" />");

            // Act
            var instances = (DeserializedItemSet) ElementDeserializer.DeserializeFromXElement(element, null, null);

            // Assert
            Assert.IsNotNull(instances);
            Assert.AreEqual(3, instances.Count);

            Assert.IsInstanceOf<AuditPropertyDefinition>(instances[0]);
            Assert.AreEqual("testauditproperty", ((AuditPropertyDefinition) instances[0]).Name);

            Assert.IsInstanceOf<PropertyDefinition>(instances[1]);
            Assert.AreEqual("testauditpropertyAuditUserId", ((PropertyDefinition) instances[1]).Name);

            Assert.IsInstanceOf<PropertyDefinition>(instances[2]);
            Assert.AreEqual("testauditpropertyAuditDateTime", ((PropertyDefinition) instances[2]).Name);
        }

        [Test]
        public void ElementName()
        {
            // Arrange / Act / Assert
            Assert.AreEqual("auditproperty", ElementDeserializer.ElementName);
        }
    }
}