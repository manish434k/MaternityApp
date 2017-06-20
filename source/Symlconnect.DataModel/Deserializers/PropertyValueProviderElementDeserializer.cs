using System;
using System.Xml.Linq;
using Symlconnect.Contracts.Factories;
using Symlconnect.DataModel.ValueProviders;

namespace Symlconnect.DataModel.Deserializers
{
    public class PropertyValueProviderElementDeserializer : ValueProviderElementDeserializerBase<PropertyValueProvider>
    {
        public PropertyValueProviderElementDeserializer(IFactory<PropertyValueProvider> valueProviderFactory)
            : base(valueProviderFactory)
        {
        }

        public override string ElementName => "propertyvalue";

        public override object DeserializeFromXElement(XElement element, object parent, IDataDictionary root)
        {
            var newInstance = (PropertyValueProvider) base.DeserializeFromXElement(element, parent, root);

            newInstance.EntityName = element.Attribute("entityname")?.Value;

            string propertyName = element.Attribute("name")?.Value;
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new InvalidOperationException($"PropertyValueProvider element missing name attribute: {element}");
            }
            newInstance.PropertyName = propertyName;

            return newInstance;
        }
    }
}