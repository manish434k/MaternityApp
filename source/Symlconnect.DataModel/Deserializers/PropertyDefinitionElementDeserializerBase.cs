using System;
using System.Xml.Linq;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.DataModel.Deserializers
{
    public abstract class PropertyDefinitionElementDeserializerBase<T> : IElementDeserializer<IDataDictionary>
        where T : IPropertyDefinition
    {
        private readonly IFactory<T> _propertyDefinitionFactory;

        protected PropertyDefinitionElementDeserializerBase(IFactory<T> propertyDefinitionFactory)
        {
            _propertyDefinitionFactory = propertyDefinitionFactory;
        }

        public virtual object DeserializeFromXElement(XElement element, object parent, IDataDictionary root)
        {
            var newInstance = _propertyDefinitionFactory.CreateInstance();
            newInstance.Name = (string) element.Attribute("name");
            if (!string.IsNullOrWhiteSpace(element.Attribute("kind")?.Value))
            {
                newInstance.PropertyDefinitionKind = (ValueKind) Enum.Parse(typeof(ValueKind),
                    (string) element.Attribute("kind"), true);
            }

            return newInstance;
        }

        public abstract string ElementName { get; }
    }
}