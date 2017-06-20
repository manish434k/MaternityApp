using System;
using System.Reflection;
using System.Xml.Linq;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.DataModel.Serializers
{
    public class ChildEntityElementSerializer : IElementSerializer<IEntity>
    {
        private readonly IValueSerializer<DateTime> _dateTimeValueSerializer;

        public ChildEntityElementSerializer(IValueSerializer<DateTime> dateTimeValueSerializer)
        {
            _dateTimeValueSerializer = dateTimeValueSerializer;
        }

        public bool IsSerializerForType(Type possibleType)
        {
            return possibleType == typeof(ChildEntity) ||
                possibleType.GetTypeInfo().IsSubclassOf(typeof(ChildEntity));
        }

        public XElement SerializeToXElement(object item, object parent, IEntity root)
        {
            if (item is ChildEntity)
            {
                var childEntity = (ChildEntity) item;
                var element = new XElement("childentity");
                element.SetAttributeValue("sessionid", childEntity.SessionId);
                element.SetAttributeValue("userid", childEntity.UserId);
                element.SetAttributeValue("createddatetime", _dateTimeValueSerializer.SerializeValue(childEntity.CreatedDateTime));
                return element;
            }

            return null;
        }
    }
}