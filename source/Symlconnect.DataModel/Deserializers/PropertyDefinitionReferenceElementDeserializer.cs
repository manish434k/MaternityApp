using System;
using System.Xml.Linq;
using Symlconnect.Common.ExtensionMethods;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.DataModel.Deserializers
{
    public class PropertyDefinitionReferenceElementDeserializer : IElementDeserializer<IDataDictionary>
    {
        private readonly IFactory<PropertyDefinitionReference> _propertyDefinitionReferenceFactory;

        public PropertyDefinitionReferenceElementDeserializer(
            IFactory<PropertyDefinitionReference> propertyDefinitionReferenceFactory)
        {
            _propertyDefinitionReferenceFactory = propertyDefinitionReferenceFactory;
        }

        public string ElementName => "propertyref";

        public object DeserializeFromXElement(XElement element, object parent, IDataDictionary root)
        {
            if (root == null)
            {
                throw new InvalidOperationException(
                    $"Attempted to deserialize a Property Definition without a root Data Dictionary: {element}");
            }

            element.ValidateRequiredAttributes("refname", "name");

            var referencedPropertyName = element.Attribute("refname").Value;
            var name = element.Attribute("name").Value;


            if (!root.PropertyDefinitions.Contains(referencedPropertyName))
            {
                throw new InvalidOperationException(
                    $"Could not find referenced Property Definition: {referencedPropertyName}");
            }

            var propertyDefinitionReference = _propertyDefinitionReferenceFactory.CreateInstance();
            propertyDefinitionReference.Name = name;
            propertyDefinitionReference.ReferencedPropertyDefinition = root.PropertyDefinitions[referencedPropertyName];

            return propertyDefinitionReference;
        }
    }
}