using System;
using System.Xml.Linq;
using Symlconnect.Common.ExtensionMethods;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.ViewModel.Deserializers
{
    public class FormDefinitionElementDeserializer : IElementDeserializer<IFormDefinition>
    {
        private readonly IFactory<IFormDefinition> _factory;

        public FormDefinitionElementDeserializer(IFactory<IFormDefinition> factory)
        {
            _factory = factory;
        }

        public string ElementName => "form";

        public object DeserializeFromXElement(XElement element, object parent, IFormDefinition root)
        {
            element.ValidateRequiredAttributes("datadictionary");

            var instance = _factory.CreateInstance();
            instance.Id = element.Attribute("id") != null ? element.Attribute("id").Value : Guid.NewGuid().ToString();
            instance.DataDictionaryName = element.Attribute("datadictionary").Value;
            return instance;
        }
    }
}