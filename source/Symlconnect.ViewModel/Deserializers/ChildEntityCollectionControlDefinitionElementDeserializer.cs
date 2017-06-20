using System.Xml.Linq;
using Symlconnect.Common.ExtensionMethods;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.ViewModel.Deserializers
{
    public class ChildEntityCollectionControlDefinitionElementDeserializer :
        BasicControlDefinitionElementDeserializer
        <ChildEntityCollectionControlDefinition, IFactory<ChildEntityCollectionControlDefinition>>
    {
        private readonly IValueDeserializer<bool> _booleanValueDeserializer;

        public ChildEntityCollectionControlDefinitionElementDeserializer(
            IFactory<ChildEntityCollectionControlDefinition> factory,
            IValueDeserializer<bool> booleanValueDeserializer) : base(factory, "childentities")
        {
            _booleanValueDeserializer = booleanValueDeserializer;
        }

        protected override void DeserializeAdditionalAttributes(ChildEntityCollectionControlDefinition instance,
            XElement element, object parent,
            IFormDefinition root)
        {
            element.ValidateRequiredAttributes("form", "propertyname");

            instance.PropertyName = element.Attribute("propertyname").Value;
            instance.FormDefinitionName = element.Attribute("form").Value;
            instance.IsAddAllowed = _booleanValueDeserializer.DeserializeValue(element.Attribute("isaddallowed")?.Value);
        }
    }
}