using System.Xml.Linq;
using NUnit.Framework;
using Symlconnect.UnitTests.Framework.ElementDeserializers;

namespace Symlconnect.ViewModel.UnitTests.ElementDeserializers
{
    public abstract class ControlDefinitionElementDeserializerTestsBase<TControlDefinition> :
        ElementDeserializationTestBase<IControlDefinition, IFormDefinition>
        where TControlDefinition : IControlDefinition
    {
        private readonly string _elementName;

        protected ControlDefinitionElementDeserializerTestsBase(string elementName)
        {
            _elementName = elementName;
        }

        [Test]
        public void GoodDeserializationStandardAttributes()
        {
            // Arrange
            var element =
                XElement.Parse(
                    "<control id=\"idvalue\" caption=\"captionvalue\" value=\"valuevalue\" visible=\"visiblevalue\" />");
            element.Add(MinimumViableElement().Attributes());

            // Act
            var instance =
                (TControlDefinition) ElementDeserializer.DeserializeFromXElement(element, null, null);

            // Assert
            Assert.IsNotNull(instance);
            Assert.AreEqual("idvalue", instance.Id);
            Assert.AreEqual("captionvalue", instance.Caption);
            Assert.AreEqual("valuevalue", instance.ValuePropertyName);
            Assert.AreEqual("visiblevalue", instance.IsVisiblePropertyName);
        }

        protected virtual XElement MinimumViableElement()
        {
            return XElement.Parse("<control />");
        }

        [Test]
        public void DefaultValuesOfMissingAttributes()
        {
            // Arrange
            var element = MinimumViableElement();

            // Act
            var instance =
                (TControlDefinition) ElementDeserializer.DeserializeFromXElement(element, null, null);

            // Assert
            Assert.IsNotNull(instance);
            Assert.IsNotNull(instance.Id);
            Assert.IsNull(instance.Caption);
            Assert.IsNull(instance.ValuePropertyName);
            Assert.IsNull(instance.IsVisiblePropertyName);
        }

        [Test]
        public void ElementName()
        {
            // Arrange/Act/Assert
            Assert.AreEqual(_elementName, ElementDeserializer.ElementName);
        }
    }
}