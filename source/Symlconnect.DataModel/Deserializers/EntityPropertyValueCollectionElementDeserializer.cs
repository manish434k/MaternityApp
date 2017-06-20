using System;
using System.Xml.Linq;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.DataModel.Deserializers
{
    public class EntityPropertyValueCollectionElementDeserializer : IElementDeserializer<IEntity>
    {
        private readonly IFactory<EntityPropertyValueCollection> _entityPropertyValueCollectionFactory;

        public EntityPropertyValueCollectionElementDeserializer(
            IFactory<EntityPropertyValueCollection> entityPropertyValueCollectionFactory)
        {
            _entityPropertyValueCollectionFactory = entityPropertyValueCollectionFactory;
        }

        public string ElementName => "values";

        public object DeserializeFromXElement(XElement element, object parent, IEntity root)
        {
            var collection = _entityPropertyValueCollectionFactory.CreateInstance();

            string propertyName = element.Attribute("propertyname")?.Value;
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new InvalidOperationException(
                    $"EntityPropertyValue element missing propertyname attribute: {element}");
            }

            collection.PropertyName = propertyName;

            return collection;
        }
    }
}