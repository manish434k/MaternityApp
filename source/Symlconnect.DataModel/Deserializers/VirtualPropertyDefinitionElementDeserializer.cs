using Symlconnect.Contracts.Factories;

namespace Symlconnect.DataModel.Deserializers
{
    public class VirtualPropertyDefinitionElementDeserializer :
        PropertyDefinitionElementDeserializerBase<VirtualPropertyDefinition>
    {
        public VirtualPropertyDefinitionElementDeserializer(
            IFactory<VirtualPropertyDefinition> propertyDefinitionFactory)
            : base(propertyDefinitionFactory)
        {
        }

        public override string ElementName => "virtualproperty";
    }
}