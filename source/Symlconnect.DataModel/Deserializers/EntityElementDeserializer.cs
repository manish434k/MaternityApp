using System;
using System.Xml.Linq;
using Symlconnect.Common.ExtensionMethods;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.DataModel.Deserializers
{
    /// <summary>
    ///     Deserializes an Entity from an XElement.
    /// </summary>
    public class EntityElementDeserializer : IElementDeserializer<IEntity>
    {
        private readonly IFactory<Entity> _entityFactory;
        private readonly IDataDictionaryLocator _dataDictionaryLocator;
        private readonly IValueDeserializer<DateTime> _dateTimeDeserializer;

        public EntityElementDeserializer(IFactory<Entity> entityFactory, IDataDictionaryLocator dataDictionaryLocator,
            IValueDeserializer<DateTime> dateTimeDeserializer)
        {
            _entityFactory = entityFactory;
            _dataDictionaryLocator = dataDictionaryLocator;
            _dateTimeDeserializer = dateTimeDeserializer;
        }

        public string ElementName => "entity";

        public object DeserializeFromXElement(XElement element, object parent, IEntity root)
        {
            element.ValidateRequiredAttributes("datadictionary", "entityname", "id", "createddatetime", "createdbyuserid", "createdbyuserdisplayname");

            var dictionaryName = element.Attribute("datadictionary").Value;
            var dataDictionary = _dataDictionaryLocator.GetDataDictionary(dictionaryName);
            if (dataDictionary == null)
            {
                throw new InvalidOperationException(
                    $"Could not deserialize an Entity as the Data Dictionary {dictionaryName} could not be located.");
            }

            var entityName = element.Attribute("entityname").Value;
            if (!dataDictionary.EntityDefinitions.Contains(entityName))
            {
                throw new InvalidOperationException(
                    $"Could not deserialize an Entity as an Entity Named {entityName} could not be found in the Data Dictionary.");
            }

            var newEntity = _entityFactory.CreateInstance();
            newEntity.Id = element.Attribute("id").Value;
            newEntity.CreatedDateTime = _dateTimeDeserializer.DeserializeValue(element.Attribute("createddatetime").Value);
            newEntity.CreatedByUserDisplayName = element.Attribute("createdbyuserdisplayname").Value;
            newEntity.CreatedByUserId = element.Attribute("createdbyuserid").Value;
            newEntity.EntityDefinition = dataDictionary.EntityDefinitions[entityName];
            return newEntity;
        }
    }
}