using System.Xml.Linq;
using NUnit.Framework;
using Symlconnect.Maternity.Common.Container;
using Symlconnect.Maternity.Common.UnitTests.Properties;
using Symlconnect.UnitTests.Framework;
using Symlconnect.ViewModel.Deserializers;

namespace Symlconnect.Maternity.Common.UnitTests
{
    [TestFixture]
    public class FormDefinitionUnitTests
    {
        [Test]
        public void LoadLiveFormDefinition()
        {
            var document = XDocument.Parse(Resources.maternity_formdefinition);
            var elementDeserializers = DesignTimeConfiguration.GetDefaultFormDefinitionElementDeserializers();
            var elementGroupDeserializers = DesignTimeConfiguration.GetDefaultFormDefinitionElementGroupDeserializers();
            var deserializer = new FormDefinitionDocumentDeserializer(elementDeserializers, elementGroupDeserializers,
                new AssertLogger());

            var formDefinition = deserializer.DeserializeFromXDocument(document);

            // Assert
            Assert.IsNotNull(formDefinition);

            CollectionAssert.IsNotEmpty(formDefinition.SharedControlDefinitions);

            Assert.AreEqual(12, formDefinition.SharedControlDefinitions.Count);
            Assert.AreEqual(3, formDefinition.LookupDefinitions.Count);
            Assert.AreEqual(7, formDefinition.SectionDefinitions.Count);
        }
    }
}