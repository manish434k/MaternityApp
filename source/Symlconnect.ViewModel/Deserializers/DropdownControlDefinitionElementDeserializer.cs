using System.Xml.Linq;
using Symlconnect.Common.ExtensionMethods;
using Symlconnect.Contracts.Factories;

namespace Symlconnect.ViewModel.Deserializers
{
    public class DropdownControlDefinitionElementDeserializer :
        BasicControlDefinitionElementDeserializer<DropdownControlDefinition, IFactory<DropdownControlDefinition>>
    {
        public DropdownControlDefinitionElementDeserializer(IFactory<DropdownControlDefinition> factory)
            : this(factory, "dropdown")
        {
        }

        protected DropdownControlDefinitionElementDeserializer(IFactory<DropdownControlDefinition> factory,
            string elementName)
            : base(factory, elementName)
        {
        }

        public override object DeserializeFromXElement(XElement element, object parent, IFormDefinition root)
        {
            element.ValidateRequiredAttributes("lookup");

            var instance = (DropdownControlDefinition) base.DeserializeFromXElement(element, parent, root);
            instance.LookupName = element.Attribute("lookup").Value;
            return instance;
        }
    }
}