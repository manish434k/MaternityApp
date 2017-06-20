using System;
using System.Xml.Linq;
using Symlconnect.Common.ExtensionMethods;
using Symlconnect.Contracts.Factories;

namespace Symlconnect.DataModel.Deserializers
{
    public class ChildEntityCollectionPropertyDefinitionElementDeserializer :
        PropertyDefinitionElementDeserializerBase<ChildEntityCollectionPropertyDefinition>
    {
        public ChildEntityCollectionPropertyDefinitionElementDeserializer(
            IFactory<ChildEntityCollectionPropertyDefinition> propertyDefinitionFactory)
            : base(propertyDefinitionFactory)
        {
        }

        public override object DeserializeFromXElement(XElement element, object parent, IDataDictionary root)
        {
            if (root == null)
            {
                throw new InvalidOperationException(
                    "Cannot deserialize a ChildEntityCollection element with a null DataDictionary.");
            }

            element.ValidateRequiredAttributes("entityname");

            var entityName = element.Attribute("entityname").Value;
            if (!root.EntityDefinitions.Contains(entityName))
            {
                throw new InvalidOperationException(
                    $"Cannot deserialize a ChildEntityCollection as EntityDefinition {entityName} could not be found in the DataDictionary.");
            }

            var instance = (ChildEntityCollectionPropertyDefinition) base.DeserializeFromXElement(element, parent, root);
            instance.EntityDefinition = root.EntityDefinitions[entityName];

            return instance;
        }

        public override string ElementName => "childentitycollection";
    }
}