using System.Xml.Linq;
using NUnit.Framework;
using FakeItEasy;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;
using Symlconnect.UnitTests.Framework.ElementDeserializers;
using Symlconnect.ViewModel;
using Symlconnect.ViewModel.Deserializers;
using Symlconnect.ViewModel.Media;

namespace Symlconnect.DataModel.UnitTests.ElementDeserializers
{
    [TestFixture]
    public class FormSectionDefinitionElementDeserializerTests :
        ElementDeserializationTestBase<FormSectionDefinition, IFormDefinition>
    {
        protected override IElementDeserializer<IFormDefinition> CreateElementDeserializerInstance()
        {
            return new FormSectionDefinitionElementDeserializer(A.Fake<IFactory<FormSectionDefinition>>());
        }

        [Test]
        public void GoodDeserialization()
        {
            // Arrange
            var element =
                XElement.Parse(
                    "<section id=\"idvalue\" backcolor=\"#555555\" forecolor=\"#222222\" title=\"titlevalue\" />");

            // Act
            var instance =
                (FormSectionDefinition) ElementDeserializer.DeserializeFromXElement(element, null, null);

            // Assert
            Assert.IsNotNull(instance);
            Assert.AreEqual("idvalue", instance.Id);
            Assert.AreEqual("titlevalue", instance.Title);
            Assert.AreEqual(Color.FromHexString("#555555"), instance.BackgroundColor);
            Assert.AreEqual(Color.FromHexString("#222222"), instance.ForegroundColor);
            Assert.AreEqual("idvalue", instance.Id);
        }

        [Test]
        public void AttributeDefaults()
        {
            // Arrange
            var element = XElement.Parse("<section />");

            // Act
            var instance =
                (FormSectionDefinition) ElementDeserializer.DeserializeFromXElement(element, null, null);

            // Assert
            Assert.IsNotNull(instance);
            Assert.IsNotNull(instance.Id);
        }

        [Test]
        public void ElementName()
        {
            // Arrange/Act/Assert
            Assert.AreEqual("section", ElementDeserializer.ElementName);
        }
    }
}