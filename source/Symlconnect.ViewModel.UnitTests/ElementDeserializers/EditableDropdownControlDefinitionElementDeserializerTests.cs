using System.Xml.Linq;
using FakeItEasy;
using NUnit.Framework;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;
using Symlconnect.ViewModel.Deserializers;

namespace Symlconnect.ViewModel.UnitTests.ElementDeserializers
{
    [TestFixture]
    public class EditableDropdownControlDefinitionElementDeserializerTests :
        ControlDefinitionElementDeserializerTestsBase<EditableDropdownControlDefinition>
    {
        public EditableDropdownControlDefinitionElementDeserializerTests() : base("editabledropdown")
        {
        }

        protected override IElementDeserializer<IFormDefinition> CreateElementDeserializerInstance()
        {
            return
                new EditableDropdownControlDefinitionElementDeserializer(
                    A.Fake<IFactory<EditableDropdownControlDefinition>>());
        }

        protected override XElement MinimumViableElement()
        {
            return XElement.Parse("<editabledropdown lookup=\"lookupvalue\" />");
        }

        [Test]
        public void AdditionalAttributes()
        {
            // Arrange
            var element = XElement.Parse("<editabledropdown lookup=\"lookupvalue\" />");

            // Act
            var instance =
                (EditableDropdownControlDefinition) ElementDeserializer.DeserializeFromXElement(element, null, null);

            // Assert
            Assert.IsNotNull(instance);
            Assert.AreEqual("lookupvalue", instance.LookupName);
        }
    }
}