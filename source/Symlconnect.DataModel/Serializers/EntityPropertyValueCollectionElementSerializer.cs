using System;
using System.Reflection;
using System.Xml.Linq;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.DataModel.Serializers
{
    public class EntityPropertyValueCollectionElementSerializer : IElementSerializer<IEntity>
    {
        public bool IsSerializerForType(Type possibleType)
        {
            return possibleType == typeof(EntityPropertyValueCollection) ||
                   possibleType.GetTypeInfo().IsSubclassOf(typeof(EntityPropertyValueCollection));
        }

        public XElement SerializeToXElement(object item, object parent, IEntity root)
        {
            if (item is EntityPropertyValueCollection)
            {
                var element = new XElement("values");
                element.SetAttributeValue("propertyname", ((EntityPropertyValueCollection) item).PropertyName);
                return element;
            }

            return null;
        }
    }
}