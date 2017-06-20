using System;
using System.Reflection;
using System.Xml.Linq;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.DataModel.Serializers
{
    public class EntityElementSerializer : IElementSerializer<IEntity>
    {
        private readonly IValueSerializer<DateTime> _dateTimeSerializer;

        public EntityElementSerializer(IValueSerializer<DateTime> dateTimeSerializer)
        {
            _dateTimeSerializer = dateTimeSerializer;
        }

        public bool IsSerializerForType(Type possibleType)
        {
            return typeof(IEntity).GetTypeInfo().IsAssignableFrom(possibleType.GetTypeInfo());
        }

        public XElement SerializeToXElement(object item, object parent, IEntity root)
        {
            if (item is IEntity)
            {
                var entity = (IEntity)item;
                var element = new XElement("entity");
                element.SetAttributeValue("id", entity.Id);
                element.SetAttributeValue("datadictionary", entity.EntityDefinition.DataDictionary.Name);
                element.SetAttributeValue("entityname", entity.EntityDefinition.EntityName);
                element.SetAttributeValue("createddatetime",_dateTimeSerializer.SerializeValue(entity.CreatedDateTime));
                element.SetAttributeValue("createdbyuserid", entity.CreatedByUserId);
                element.SetAttributeValue("createdbyuserdisplayname", entity.CreatedByUserDisplayName);
                return element;
            }

            return null;
        }
    }
}