using System;
using System.Reflection;
using System.Xml.Linq;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.DataModel.Serializers
{
    public class ChildEntityCollectionElementSerializer : IElementSerializer<IEntity>
    {
        public bool IsSerializerForType(Type possibleType)
        {
            return possibleType == typeof(ChildEntityCollection) ||
                possibleType.GetTypeInfo().IsSubclassOf(typeof(ChildEntityCollection));
        }

        public XElement SerializeToXElement(object item, object parent, IEntity root)
        {
            if (item is ChildEntityCollection)
            {
                var element = new XElement("childentities");
                element.SetAttributeValue("propertyname", ((ChildEntityCollection)item).PropertyName);
                return element;
            }

            return null;
        }
    }
}