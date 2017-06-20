using System.Xml.Linq;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.DataModel.Deserializers
{
    /// <summary>
    ///     Element deserializer for EntityDefinition Elements (&lt;datadictionary/&gt;).
    /// </summary>
    public class EntityDefinitionElementDeserializer : IElementDeserializer<IDataDictionary>, IElementGroupDeserializer
    {
        private readonly IFactory<EntityDefinition> _entityDefinitionFactory;

        public EntityDefinitionElementDeserializer(IFactory<EntityDefinition> entityDefinitionFactory)
        {
            _entityDefinitionFactory = entityDefinitionFactory;
        }

        public string ElementName => "entitydefinition";

        public object DeserializeFromXElement(XElement element, object parent, IDataDictionary root)
        {
            var entityDefinition = _entityDefinitionFactory.CreateInstance();
            entityDefinition.EntityName = element.Attribute("name")?.Value;
            entityDefinition.DataDictionary = root;
            return entityDefinition;
        }

        public string ElementGroupName => "entitydefinitions";
    }
}