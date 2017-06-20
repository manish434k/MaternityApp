using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.Unity;
using Symlconnect.Common.Environment;
using Symlconnect.Common.Factories;
using Symlconnect.Common.Serialization;
using Symlconnect.Contracts.Environment;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;
using Symlconnect.DataModel;
using Symlconnect.DataModel.Deserializers;
using Symlconnect.DataModel.Factories;
using Symlconnect.DataModel.Rules;
using Symlconnect.DataModel.ValueProviders;
using Symlconnect.Maternity.Common.Factories;
using Symlconnect.ViewModel;
using Symlconnect.ViewModel.Deserializers;
using Symlconnect.ViewModel.Factories;
using Symlconnect.ViewModel.ViewModels;

namespace Symlconnect.Maternity.Common.Container
{
    [ExcludeFromCodeCoverage]
    public static class ContainerConfiguration
    {
        public static void RegisterCommonTypes(IUnityContainer container)
        {
            container.RegisterType<ICurrentDateTimeProvider, CurrentDateTimeProvider>();
        }

        /// <summary>
        ///     Register the DataModel types used by Maternity.
        /// </summary>
        public static void RegisterDataModelTypes(IUnityContainer container)
        {
            container.RegisterType<IFactory<AuditPropertyDefinition>, AuditPropertyDefinitionFactory>();
            container.RegisterType<IFactory<ChildEntityCollection>, ChildEntityCollectionFactory>();
            container
                .RegisterType
                <IFactory<ChildEntityCollectionPropertyDefinition>, ChildEntityCollectionPropertyDefinitionFactory>();
            container.RegisterType<IFactory<ChildEntity>, ChildEntityFactory>();
            container.RegisterType<IFactory<DataDictionary>, DataDictionaryFactory>();
            container.RegisterType<IFactory<EntityDefinition>, EntityDefinitionFactory>();
            container.RegisterType<IFactory<Entity>, EntityFactory>();
            container.RegisterType<IFactory<EntityPropertyValueCollection>, EntityPropertyValueCollectionFactory>();
            container.RegisterType<IFactory<EntityPropertyValue>, EntityPropertyValueFactory>();
            container.RegisterType<IFactory<PropertyDefinition>, PropertyDefinitionFactory>();
            container.RegisterType<IFactory<PropertyDefinitionReference>, PropertyDefinitionReferenceFactory>();
            container.RegisterType<IFactory<PropertyValueProvider>, PropertyValueProviderFactory>();
            container.RegisterType<IFactory<AgeValueProvider>, AgeValueProviderFactory>();
            container.RegisterType<IFactory<VirtualPropertyDefinition>, VirtualPropertyDefinitionFactory>();

            // Entity document deserializer
            container.RegisterType<IDocumentDeserializer<IEntity>, EntityDocumentDeSerializer>();
            // Entity element deserializers
            container.RegisterType<IElementDeserializer<IEntity>, ChildEntityCollectionElementDeserializer>(
                "childentitycollection");
            container.RegisterType<IElementDeserializer<IEntity>, ChildEntityElementDeserializer>("childentity");
            container.RegisterType<IElementDeserializer<IEntity>, EntityElementDeserializer>("entity");
            container.RegisterType<IElementDeserializer<IEntity>, EntityPropertyValueElementDeserializer>(
                "entitypropertyvalue");
            container.RegisterType<IElementDeserializer<IEntity>, EntityPropertyValueCollectionElementDeserializer>(
                "entitypropertyvaluecollection");
            container.RegisterType<IEnumerable<IElementDeserializer<IEntity>>, IElementDeserializer<IEntity>[]>();
            container.RegisterType<IEnumerable<IElementGroupDeserializer>, IElementGroupDeserializer[]>();

            // DataDictionary document deserializer
            container.RegisterType<IDocumentDeserializer<IDataDictionary>, DataDictionaryDocumentDeserializer>();
            // DataDictionary element deserializers
            container.RegisterType<IElementDeserializer<IDataDictionary>, AuditPropertyDefinitionElementDeserializer>(
                "auditpropertydefinition");
            container
                .RegisterType
                <IElementDeserializer<IDataDictionary>, ChildEntityCollectionPropertyDefinitionElementDeserializer>(
                    "childentitypropertydefinitioncollection");
            container.RegisterType<IElementDeserializer<IDataDictionary>, DataDictionaryElementDeserializer>(
                "datadictionary");
            container.RegisterType<IElementDeserializer<IDataDictionary>, EntityDefinitionElementDeserializer>(
                "entitydefinition");
            container.RegisterType<IElementDeserializer<IDataDictionary>, PropertyDefinitionElementDeserializer>(
                "propertydefinition");
            container
                .RegisterType<IElementDeserializer<IDataDictionary>, PropertyDefinitionReferenceElementDeserializer>(
                    "propertydefinitionreference");
            container.RegisterType<IElementDeserializer<IDataDictionary>, PropertyValueProviderElementDeserializer>(
                "propertyvalueprovider");
            container.RegisterType<IElementDeserializer<IDataDictionary>, AgeValueProviderElementDeserializer>(
                "agevalueprovider");
            container.RegisterType<IElementDeserializer<IDataDictionary>, VirtualPropertyDefinitionElementDeserializer>(
                "virtualpropertydef");
            container.RegisterType<IElementGroupDeserializer, PropertyDefinitionElementDeserializer>("properties");
            container.RegisterType<IElementGroupDeserializer, EntityDefinitionElementDeserializer>("entities");
            container.RegisterType<IElementGroupDeserializer, RuleElementGroupDeserializer>("rules");
            container
                .RegisterType
                <IEnumerable<IElementDeserializer<IDataDictionary>>, IElementDeserializer<IDataDictionary>[]>();

            container.RegisterType<IFactory<RegExRuleDefinition>, SimpleFactory<RegExRuleDefinition>>();

            container.RegisterType<IValueDeserializer<bool>, CommonValueDeserializers>();
            container.RegisterType<IValueDeserializer<double>, CommonValueDeserializers>();
            container.RegisterType<IValueDeserializer<long>, CommonValueDeserializers>();
            container.RegisterType<IValueDeserializer<DateTime>, CommonValueDeserializers>();

            container.RegisterType<IElementDeserializer<IDataDictionary>, RegExRuleElementDeserializer>("regexrule");
        }

        /// <summary>
        ///     Register the ViewModel types used by Maternity.
        /// </summary>
        public static void RegisterViewModelTypes(IUnityContainer container)
        {
            container
                .RegisterType
                <IElementDeserializer<IFormDefinition>,
                    BasicControlDefinitionElementDeserializer
                    <CheckboxControlDefinition, IFactory<CheckboxControlDefinition>>>("checkbox",
                    new InjectionConstructor(new SimpleFactory<CheckboxControlDefinition>(), "checkbox"));
            container.RegisterType<IFactory<CheckboxControlDefinition>, SimpleFactory<CheckboxControlDefinition>>();

            container
                .RegisterType
                <IElementDeserializer<IFormDefinition>,
                    BasicControlDefinitionElementDeserializer
                    <DateBoxControlDefinition, SimpleFactory<DateBoxControlDefinition>>>("datebox",
                    new InjectionConstructor(new SimpleFactory<DateBoxControlDefinition>(), "datebox"));
            container.RegisterType<IFactory<DateBoxControlDefinition>, SimpleFactory<DateBoxControlDefinition>>();

            container.RegisterType<IElementDeserializer<IFormDefinition>,
                DropdownControlDefinitionElementDeserializer>("dropdown");
            container.RegisterType<IFactory<DropdownControlDefinition>, SimpleFactory<DropdownControlDefinition>>();

            container.RegisterType<IElementDeserializer<IFormDefinition>,
                EditableDropdownControlDefinitionElementDeserializer>("editabledropdown");
            container
                .RegisterType
                <IFactory<EditableDropdownControlDefinition>, SimpleFactory<EditableDropdownControlDefinition>>();

            container
                .RegisterType
                <IElementDeserializer<IFormDefinition>,
                    StackControlDefinitionElementDeserializer>("stack");
            container.RegisterType<IFactory<StackControlDefinition>, SimpleFactory<StackControlDefinition>>();

            container
                .RegisterType
                <IElementDeserializer<IFormDefinition>,
                    BasicControlDefinitionElementDeserializer
                    <TextAreaControlDefinition, IFactory<TextAreaControlDefinition>>>("textarea",
                    new InjectionConstructor(new SimpleFactory<TextAreaControlDefinition>(), "textarea"));
            container.RegisterType<IFactory<TextAreaControlDefinition>, SimpleFactory<TextAreaControlDefinition>>();

            container
                .RegisterType
                <IElementDeserializer<IFormDefinition>,
                    BasicControlDefinitionElementDeserializer
                    <TextBoxControlDefinition, IFactory<TextBoxControlDefinition>>>("textbox",
                    new InjectionConstructor(new SimpleFactory<TextBoxControlDefinition>(), "textbox"));
            container.RegisterType<IFactory<TextBoxControlDefinition>, SimpleFactory<TextBoxControlDefinition>>();
            container.RegisterType<IFactory<TextBoxControlDefinition>, SimpleFactory<TextBoxControlDefinition>>();

            container
                .RegisterType
                <IElementDeserializer<IFormDefinition>,
                    BasicControlDefinitionElementDeserializer
                    <TextBlockControlDefinition, IFactory<TextBlockControlDefinition>>>("textblock",
                    new InjectionConstructor(new SimpleFactory<TextBlockControlDefinition>(), "textblock"));
            container.RegisterType<IFactory<TextBlockControlDefinition>, SimpleFactory<TextBlockControlDefinition>>();

            container
                .RegisterType
                <IElementDeserializer<IFormDefinition>,
                    BasicControlDefinitionElementDeserializer<YesNoControlDefinition, IFactory<YesNoControlDefinition>>>
                ("yesno",
                    new InjectionConstructor(new SimpleFactory<YesNoControlDefinition>(), "yesno"));
            container.RegisterType<IFactory<YesNoControlDefinition>, SimpleFactory<YesNoControlDefinition>>();

            container
                .RegisterType
                <IElementDeserializer<IFormDefinition>,
                    MessageControlDefinitionDeserializer<AlertControlDefinition, IFactory<AlertControlDefinition>>>
                ("alert",
                    new InjectionConstructor(new SimpleFactory<AlertControlDefinition>(), "alert"));
            container.RegisterType<IFactory<AlertControlDefinition>, SimpleFactory<AlertControlDefinition>>();

            container
                .RegisterType
                <IElementDeserializer<IFormDefinition>,
                    MessageControlDefinitionDeserializer<AuditControlDefinition, IFactory<AuditControlDefinition>>>
                ("audit",
                    new InjectionConstructor(new SimpleFactory<AuditControlDefinition>(), "audit"));
            container.RegisterType<IFactory<AuditControlDefinition>, SimpleFactory<AuditControlDefinition>>();

            container.RegisterType<IElementDeserializer<IFormDefinition>,
                ChildEntityCollectionControlDefinitionElementDeserializer>("childentities");
            container
                .RegisterType<IFactory<ChildEntityCollectionControlDefinition>, SimpleFactory<ChildEntityCollectionControlDefinition>>();

            container.RegisterType<IElementDeserializer<IFormDefinition>, FormSectionDefinitionElementDeserializer>(
                "formsection");
            container.RegisterType<IFactory<IFormSectionDefinition>, SimpleFactory<FormSectionDefinition>>();
            container.RegisterType<IElementDeserializer<IFormDefinition>, FormDefinitionElementDeserializer>("form");
            container.RegisterType<IFactory<IFormDefinition>, SimpleFactory<FormDefinition>>();
            container.RegisterType<IElementDeserializer<IFormDefinition>, LookupDefinitionElementDeserializer>(
                "lookupdefinition");
            container.RegisterType<IFactory<ILookupDefinition>, SimpleFactory<LookupDefinition>>();
            container.RegisterType<IElementDeserializer<IFormDefinition>, LookupEntryElementDeserializer>(
                "lookupentry");
            container.RegisterType<IFactory<ILookupEntry>, SimpleFactory<LookupEntry>>();

            container.RegisterType<IElementGroupDeserializer, ControlElementGroupDeserializer>("controls");
            container.RegisterType<IElementGroupDeserializer, LookupDefinitionElementGroupDeserializer>("lookups");

            container
                .RegisterType
                <IEnumerable<IElementDeserializer<IFormDefinition>>, IElementDeserializer<IFormDefinition>[]>();

            // ControlDefinition ViewModel Factories
            container
                .RegisterType
                <IControlDefinitionViewModelFactory,
                    ControlDefinitionViewModelFactory<CheckboxControlDefinitionViewModel, CheckboxControlDefinition>>(
                    "checkbox");
            container
                .RegisterType
                <IControlDefinitionViewModelFactory,
                    ControlDefinitionViewModelFactory<DateBoxControlDefinitionViewModel, DateBoxControlDefinition>>(
                    "datebox");
            container
                .RegisterType
                <IControlDefinitionViewModelFactory,
                    ControlDefinitionViewModelFactory<DropdownControlDefinitionViewModel, DropdownControlDefinition>>(
                    "dropdown");
            container
                .RegisterType
                <IControlDefinitionViewModelFactory, ControlDefinitionViewModelFactory
                    <EditableDropdownControlDefinitionViewModel, EditableDropdownControlDefinition>>("editabledropdown");
            container
                .RegisterType
                <IControlDefinitionViewModelFactory,
                    ControlDefinitionViewModelFactory<StackControlDefinitionViewModel, StackControlDefinition>>("stack");
            container
                .RegisterType
                <IControlDefinitionViewModelFactory,
                    ControlDefinitionViewModelFactory<TextAreaControlDefinitionViewModel, TextAreaControlDefinition>>(
                    "textarea");
            container
                .RegisterType
                <IControlDefinitionViewModelFactory,
                    ControlDefinitionViewModelFactory<TextBlockControlDefinitionViewModel, TextBlockControlDefinition>>(
                    "textblock");
            container
                .RegisterType
                <IControlDefinitionViewModelFactory,
                    ControlDefinitionViewModelFactory<TextBoxControlDefinitionViewModel, TextBoxControlDefinition>>(
                    "textbox");
            container
                .RegisterType
                <IControlDefinitionViewModelFactory,
                    ControlDefinitionViewModelFactory<YesNoControlDefinitionViewModel, YesNoControlDefinition>>("yesno");
            container
                .RegisterType
                <IControlDefinitionViewModelFactory,
                    ControlDefinitionViewModelFactory<AlertControlDefinitionViewModel, AlertControlDefinition>>("alert");
            container
                .RegisterType
                <IControlDefinitionViewModelFactory,
                    ControlDefinitionViewModelFactory<AuditControlDefinitionViewModel, AuditControlDefinition>>("audit");
            container
                .RegisterType
                <IControlDefinitionViewModelFactory,
                    ChildEntityCollectionControlDefinitionViewModelFactory>("childentities");

            container
                .RegisterType
                <IEnumerable<IControlDefinitionViewModelFactory>, IControlDefinitionViewModelFactory[]>();

            // FormDefinition Document Serializer
            container.RegisterType<IDocumentDeserializer<IFormDefinition>, FormDefinitionDocumentDeserializer>();

            // FormContext
            container.RegisterType<IFactory<IFormContext>, SimpleFactory<FormContext>>();

            // Form and Section ViewModel Factories
            container.RegisterType<IFactory<FormViewModel>, FormViewModelFactory>();
            container.RegisterType<IFactory<FormSectionViewModel>, FormSectionViewModelFactory>();

            container.RegisterType<IControlDefinitionViewModelFactoryLocator, ControlDefinitionViewModelFactoryLocator>();
        }

        public static void RegisterMaternityTypes(IUnityContainer container)
        {
            container.RegisterType<IFactory<IPatient>, PatientFactory>();
        }
    }
}