using System;
using System.Collections.Generic;
using Symlconnect.Common.Environment;
using Symlconnect.Common.Factories;
using Symlconnect.Common.Serialization;
using Symlconnect.Contracts.Serialization;
using Symlconnect.DataModel;
using Symlconnect.DataModel.Deserializers;
using Symlconnect.DataModel.Factories;
using Symlconnect.DataModel.Rules;
using Symlconnect.ViewModel;
using Symlconnect.ViewModel.Deserializers;
using Symlconnect.ViewModel.Factories;
using Symlconnect.ViewModel.ViewModels;

namespace Symlconnect.Maternity.Common.Container
{
    public static class DesignTimeConfiguration
    {
        public static IEnumerable<IElementDeserializer<IFormDefinition>> GetDefaultFormDefinitionElementDeserializers()
        {
            return new List<IElementDeserializer<IFormDefinition>>
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
                new StackControlDefinitionElementDeserializer(new SimpleFactory<StackControlDefinition>(),
                    new CommonValueDeserializers()),
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
                        new SimpleFactory<YesNoControlDefinition>(), "yesno"),
                new BasicControlDefinitionElementDeserializer
                    <AlertControlDefinition, SimpleFactory<AlertControlDefinition>>(
                        new SimpleFactory<AlertControlDefinition>(), "alert"),
                new BasicControlDefinitionElementDeserializer
                    <AuditControlDefinition, SimpleFactory<AuditControlDefinition>>(
                        new SimpleFactory<AuditControlDefinition>(), "audit"),
                new ChildEntityCollectionControlDefinitionElementDeserializer(
                    new SimpleFactory<ChildEntityCollectionControlDefinition>(), new CommonValueDeserializers())
            };
        }

        public static IEnumerable<IElementGroupDeserializer> GetDefaultFormDefinitionElementGroupDeserializers()
        {
            return new List<IElementGroupDeserializer>
            {
                new ControlElementGroupDeserializer(),
                new LookupDefinitionElementGroupDeserializer()
            };
        }

        public static IEnumerable<IElementDeserializer<IDataDictionary>> GetDefaultDataDictionaryElementDeserializers()
        {
            return new List<IElementDeserializer<IDataDictionary>>
            {
                new AuditPropertyDefinitionElementDeserializer(
                    new AuditPropertyDefinitionFactory(new CurrentDateTimeProvider()), new PropertyDefinitionFactory()),
                new ChildEntityCollectionPropertyDefinitionElementDeserializer(
                    new ChildEntityCollectionPropertyDefinitionFactory()),
                new DataDictionaryElementDeserializer(new DataDictionaryFactory()),
                new EntityDefinitionElementDeserializer(new EntityDefinitionFactory()),
                new PropertyDefinitionElementDeserializer(new PropertyDefinitionFactory()),
                new PropertyDefinitionReferenceElementDeserializer(new PropertyDefinitionReferenceFactory()),
                new PropertyValueProviderElementDeserializer(new PropertyValueProviderFactory()),
                new AgeValueProviderElementDeserializer(new AgeValueProviderFactory(new CurrentDateTimeProvider())),
                new VirtualPropertyDefinitionElementDeserializer(new VirtualPropertyDefinitionFactory()),
                new RegExRuleElementDeserializer(new SimpleFactory<RegExRuleDefinition>())
            };
        }

        public static IEnumerable<IElementGroupDeserializer> GetDefaultDataDictionaryElementGroupDeserializers()
        {
            return new List<IElementGroupDeserializer>
            {
                new PropertyDefinitionElementDeserializer(new PropertyDefinitionFactory()),
                new EntityDefinitionElementDeserializer(new EntityDefinitionFactory()),
                new RuleElementGroupDeserializer()
            };
        }

        public static IEnumerable<IControlDefinitionViewModelFactory> GetDefaultControlDefinitionViewModelFactories()
        {
            var factories = new List<IControlDefinitionViewModelFactory>
            {
                new ControlDefinitionViewModelFactory<CheckboxControlDefinitionViewModel, CheckboxControlDefinition>(),
                new ControlDefinitionViewModelFactory<DateBoxControlDefinitionViewModel, DateBoxControlDefinition>(),
                new ControlDefinitionViewModelFactory<DropdownControlDefinitionViewModel, DropdownControlDefinition>(),
                new ControlDefinitionViewModelFactory
                    <EditableDropdownControlDefinitionViewModel, EditableDropdownControlDefinition>(),
                new ControlDefinitionViewModelFactory<TextAreaControlDefinitionViewModel, TextAreaControlDefinition>(),
                new ControlDefinitionViewModelFactory<TextBlockControlDefinitionViewModel, TextBlockControlDefinition>(),
                new ControlDefinitionViewModelFactory<TextBoxControlDefinitionViewModel, TextBoxControlDefinition>(),
                new ControlDefinitionViewModelFactory<YesNoControlDefinitionViewModel, YesNoControlDefinition>(),
                new ControlDefinitionViewModelFactory<StackControlDefinitionViewModel, StackControlDefinition>(),
                new ControlDefinitionViewModelFactory<AlertControlDefinitionViewModel, AlertControlDefinition>()
            };

            return factories;
        }

        public static IPatient CreateDummyPatient()
        {
            return new Patient
            {
                DateOfBirth = new DateTime(1995, 3, 1),
                Id = "PatientId",
                Name = "Jane Doe",
                PatientNumber = "1234567"
            };
        }

        public static IEntity CreateDummyPatientEntity(IDataDictionary dataDictionary, string entityname,
            IPatient patient)
        {
            // Load a dummy maternity record
            var entity =
                new Entity(
                    new EntityPropertyValueCollectionFactory(new CurrentDateTimeProvider(),
                        new EntityPropertyValueFactory()), new ChildEntityCollectionFactory(), new ChildEntityFactory(),
                    new CurrentDateTimeProvider())
                {
                    EntityDefinition = dataDictionary.EntityDefinitions[entityname],
                    CreatedDateTime = DateTime.Now,
                    CreatedByUserDisplayName = "Midwife A",
                    CreatedByUserId = "MidwifeA"
                };
            return entity;
        }
    }
}