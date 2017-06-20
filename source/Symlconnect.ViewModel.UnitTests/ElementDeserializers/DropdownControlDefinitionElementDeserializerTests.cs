using System.Xml.Linq;
using NUnit.Framework;
using FakeItEasy;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;
using Symlconnect.ViewModel.Deserializers;

namespace Symlconnect.ViewModel.UnitTests.ElementDeserializers
{
    [TestFixture]
    public class DropdownControlDefinitionElementDeserializerTests :
        ControlDefinitionElementDeserializerTestsBase<DropdownControlDefinition>
    {
        public DropdownControlDefinitionElementDeserializerTests() : base("dropdown")
        {
        }

        protected override IElementDeserializer<IFormDefinition> CreateElementDeserializerInstance()
        {
            return new DropdownControlDefinitionElementDeserializer(A.Fake<IFactory<DropdownControlDefinition>>());
        }

        protected override XElement MinimumViableElement()
        {
            return XElement.Parse("<dropdown lookup=\"lookupvalue\" />");
        }

        [Test]
        public void AdditionalAttributes()
        {
            // Arrange
            var element = XElement.Parse("<dropdown lookup=\"lookupvalue\" />");

            // Act
            var instance =
                (DropdownControlDefinition) ElementDeserializer.DeserializeFromXElement(element, null, null);

            // Assert
            Assert.IsNotNull(instance);
            Assert.AreEqual("lookupvalue", instance.LookupName);
        }
    }
}