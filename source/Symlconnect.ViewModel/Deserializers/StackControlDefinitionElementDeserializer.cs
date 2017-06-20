using System.Xml.Linq;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.ViewModel.Deserializers
{
    public class StackControlDefinitionElementDeserializer :
        BasicControlDefinitionElementDeserializer<StackControlDefinition, IFactory<StackControlDefinition>>
    {
        private readonly IValueDeserializer<bool> _booleanValueDeserializer;

        public StackControlDefinitionElementDeserializer(IFactory<StackControlDefinition> factory,
            IValueDeserializer<bool> booleanValueDeserializer) : base(factory, "stack")
        {
            _booleanValueDeserializer = booleanValueDeserializer;
        }

        protected override void DeserializeAdditionalAttributes(StackControlDefinition instance, XElement element,
            object parent,
            IFormDefinition root)
        {
            instance.IsHorizontal = _booleanValueDeserializer.DeserializeValue(element.Attribute("ishorizontal")?.Value);
        }
    }
}