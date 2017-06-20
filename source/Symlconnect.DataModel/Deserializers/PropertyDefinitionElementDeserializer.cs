using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.DataModel.Deserializers
{
    /// <summary>
    ///     Element deserializer for PropertyDefinition Elements (&lt;property/&gt;) and Element Groups (&lt;properties/&gt;).
    /// </summary>
    public class PropertyDefinitionElementDeserializer : PropertyDefinitionElementDeserializerBase<PropertyDefinition>,
        IElementGroupDeserializer
    {
        public PropertyDefinitionElementDeserializer(IFactory<PropertyDefinition> propertyDefinitionFactory)
            : base(propertyDefinitionFactory)
        {
        }

        public override string ElementName => "property";
        public string ElementGroupName => "properties";
    }
}