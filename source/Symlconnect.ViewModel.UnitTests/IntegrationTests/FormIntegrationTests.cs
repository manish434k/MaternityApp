using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using FakeItEasy;
using Symlconnect.Contracts.Serialization;
using Symlconnect.ViewModel;
using Symlconnect.ViewModel.Deserializers;
using Symlconnect.Common.Factories;
using Symlconnect.Common.Serialization;
using Symlconnect.Contracts.Diagnostics;
using Symlconnect.ViewModel.Media;
using Symlconnect.ViewModel.UnitTests.Properties;

namespace Symlconnect.DataModel.UnitTests.IntegrationTests
{
    [TestFixture]
    public class FormIntegrationTests
    {
        private FormDefinitionDocumentDeserializer _sut;

        [SetUp]
        public void TestSetup()
        {
            var elementGroupDeserializers = new List<IElementGroupDeserializer>
            {
                new ControlElementGroupDeserializer(),
                new LookupDefinitionElementGroupDeserializer()
            };
            var elementDeserializers = new List<IElementDeserializer<IFormDefinition>>
            {
                new BasicControlDefinitionElementDeserializer
                    <CheckboxControlDefinition, SimpleFactory<CheckboxControlDefinition>>(
                        new SimpleFactory<CheckboxControlDefinition>(), "checkbox"),
                new BasicControlDefinitionElementDeserializer
                    <DateBoxControlDefinition, SimpleFactory<DateBoxControlDefinition>>(
                        new SimpleFactory<DateBoxControlDefinition>(), "datebox"),
                new DropdownControlDefinitionElementDeserializer(new SimpleFactory<DropdownControlDefinition>()),
                new EditableDropdownControlDefinitionElementDeserializer(
                    new SimpleFactory<EditableDropdownControlDefinition>()),
                new FormDefinitionElementDeserializer(new SimpleFactory<FormDefinition>()),
                new FormSectionDefinitionElementDeserializer(new SimpleFactory<FormSectionDefinition>()),
                new LookupDefinitionElementDeserializer(new SimpleFactory<LookupDefinition>()),
                new LookupEntryElementDeserializer(new SimpleFactory<LookupEntry>()),
                new StackControlDefinitionElementDeserializer(new SimpleFactory<StackControlDefinition>(), new CommonValueDeserializers()),
                new BasicControlDefinitionElementDeserializer
                    <TextAreaControlDefinition, SimpleFactory<TextAreaControlDefinition>>(
                        new SimpleFactory<TextAreaControlDefinition>(), "textarea"),
                new BasicControlDefinitionElementDeserializer
                    <TextBlockControlDefinition, SimpleFactory<TextBlockControlDefinition>>(
                        new SimpleFactory<TextBlockControlDefinition>(), "textblock"),
                new BasicControlDefinitionElementDeserializer
                    <TextBoxControlDefinition, SimpleFactory<TextBoxControlDefinition>>(
                        new SimpleFactory<TextBoxControlDefinition>(), "textbox"),
                new BasicControlDefinitionElementDeserializer
                    <YesNoControlDefinition, SimpleFactory<YesNoControlDefinition>>(
                        new SimpleFactory<YesNoControlDefinition>(), "yesno")
            };

            _sut = new FormDefinitionDocumentDeserializer(elementDeserializers, elementGroupDeserializers,
                A.Fake<ILogger>());
        }

        [Test]
        public void TestFullDeserialization()
        {
            // Arrange/Act
            var formDefinition = _sut.DeserializeFromXDocument(XDocument.Parse(Resources.basicform));

            // Assert
            Assert.IsNotNull(formDefinition);
            Assert.AreEqual("datadictionaryvalue", formDefinition.DataDictionaryName);
            ValidateControls(formDefinition.SharedControlDefinitions,
                new[]
                {
                    "checkboxcontrol", "dateboxcontrol", "dropdowncontrol", "editabledropdowncontrol", "stackcontrol",
                    "textareacontrol", "textblockcontrol", "textboxcontrol", "yesnocontrol"
                });

            var dropdownControl = (DropdownControlDefinition)formDefinition.SharedControlDefinitions["dropdowncontrol"];
            Assert.IsNotNull(dropdownControl.LookupName);

            var editableDropdownControl =
                (DropdownControlDefinition)formDefinition.SharedControlDefinitions["editabledropdowncontrol"];
            Assert.IsNotNull(editableDropdownControl.LookupName);

            var stackControl = (StackControlDefinition)formDefinition.SharedControlDefinitions["stackcontrol"];
            Assert.AreEqual(2, stackControl.ChildControlDefinitions.Count);

            Assert.AreEqual(2, formDefinition.SectionDefinitions.Count);

            // Root section
            var firstRootSection = formDefinition.SectionDefinitions[0];
            Assert.AreEqual("rootsectionid1", firstRootSection.Id);
            Assert.AreEqual(Color.FromHexString("#111111"), firstRootSection.BackgroundColor);
            Assert.AreEqual(Color.FromHexString("#222222"), firstRootSection.ForegroundColor);
            Assert.AreEqual("rootsectionid1title", firstRootSection.Title);

            // Nested section
            var childSection = firstRootSection.ChildSectionDefinitions.First();
            Assert.AreEqual("childsectionid1", childSection.Id);

            // Lookup Definition
            var lookupDefinition = formDefinition.LookupDefinitions.First();
            Assert.AreEqual("lookup1", lookupDefinition.Id);
            Assert.AreEqual(2,lookupDefinition.LookupEntries.Count);
            var lookupEntry = lookupDefinition.LookupEntries.Skip(1).First();
            Assert.AreEqual("entryvalue2", lookupEntry.Value);
            Assert.AreEqual("entrycaption2", lookupEntry.Caption);
        }

        private static void ValidateControls(ControlDefinitionCollection controlDefinitions, string[] controlIds)
        {
            foreach (string controlId in controlIds)
            {
                Assert.IsTrue(controlDefinitions.Contains(controlId), $"Control id {controlId} not found");
                var control = controlDefinitions[controlId];
                Assert.AreEqual(controlId + "caption", control.Caption);
                Assert.AreEqual(controlId + "visible", control.IsVisiblePropertyName);
                Assert.AreEqual(controlId + "value", control.ValuePropertyName);
            }
        }
    }
}