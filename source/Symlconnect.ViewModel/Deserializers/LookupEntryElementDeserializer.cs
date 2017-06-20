using System.Xml.Linq;
using Symlconnect.Common.ExtensionMethods;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.ViewModel.Deserializers
{
    public class LookupEntryElementDeserializer : IElementDeserializer<IFormDefinition>
    {
        private readonly IFactory<ILookupEntry> _lookupEntryFactory;
        public string ElementName => "entry";

        public LookupEntryElementDeserializer(IFactory<ILookupEntry> lookupEntryFactory)
        {
            _lookupEntryFactory = lookupEntryFactory;
        }

        public object DeserializeFromXElement(XElement element, object parent, IFormDefinition root)
        {
            element.ValidateRequiredAttributes("value");

            var instance = _lookupEntryFactory.CreateInstance();
            instance.Value = element.Attribute("value").Value;
            instance.Caption = element.Attribute("caption")?.Value;
            if (string.IsNullOrWhiteSpace(instance.Caption))
            {
                instance.Caption = instance.Value;
            }

            return instance;
        }
    }
}