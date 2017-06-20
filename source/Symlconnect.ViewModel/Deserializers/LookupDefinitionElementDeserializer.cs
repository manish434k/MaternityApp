using System.Xml.Linq;
using Symlconnect.Common.ExtensionMethods;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.ViewModel.Deserializers
{
    public class LookupDefinitionElementDeserializer : IElementDeserializer<IFormDefinition>
    {
        private readonly IFactory<ILookupDefinition> _lookupDefinitionFactory;
        public string ElementName => "lookup";

        public LookupDefinitionElementDeserializer(IFactory<ILookupDefinition> lookupDefinitionFactory)
        {
            _lookupDefinitionFactory = lookupDefinitionFactory;
        }

        public object DeserializeFromXElement(XElement element, object parent, IFormDefinition root)
        {
            element.ValidateRequiredAttributes("id");

            var instance = _lookupDefinitionFactory.CreateInstance();
            instance.Id = element.Attribute("id").Value;
            return instance;
        }
    }
}