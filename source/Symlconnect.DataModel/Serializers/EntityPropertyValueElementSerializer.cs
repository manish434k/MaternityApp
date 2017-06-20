using System;
using System.Reflection;
using System.Xml.Linq;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.DataModel.Serializers
{
    public class EntityPropertyValueElementSerializer : IElementSerializer<IEntity>
    {
        private readonly IValueSerializer<bool> _booleanValueSerializer;
        private readonly IValueSerializer<DateTime> _dateValueSerializer;
        private readonly IValueSerializer<double> _doubleValueSerializer;
        private readonly IValueSerializer<long> _longValueSerializer;

        public EntityPropertyValueElementSerializer(IValueSerializer<bool> booleanValueSerializer,
            IValueSerializer<DateTime> dateValueSerializer,
            IValueSerializer<double> doubleValueSerializer,
            IValueSerializer<long> longValueSerializer)
        {
            _booleanValueSerializer = booleanValueSerializer;
            _dateValueSerializer = dateValueSerializer;
            _doubleValueSerializer = doubleValueSerializer;
            _longValueSerializer = longValueSerializer;
        }

        public bool IsSerializerForType(Type possibleType)
        {
            return possibleType == typeof(EntityPropertyValue) ||
                   possibleType.GetTypeInfo().IsSubclassOf(typeof(EntityPropertyValue));
        }

        public XElement SerializeToXElement(object item, object parent, IEntity root)
        {
            if (item is EntityPropertyValue)
            {
                var entityPropertyValue = (EntityPropertyValue) item;

                var element = new XElement("value");
                element.SetAttributeValue("sessionid", entityPropertyValue.SessionId);
                element.SetAttributeValue("userid", entityPropertyValue.UserId);
                element.SetAttributeValue("changedatetime",
                    _dateValueSerializer.SerializeValue(entityPropertyValue.ChangeDateTime));

                // Serialize value according to the type of the value
                if (entityPropertyValue.Value != null)
                {
                    if (entityPropertyValue.Value is string)
                    {
                        element.Value = (string) entityPropertyValue.Value;
                    }
                    else if (entityPropertyValue.Value is bool)
                    {
                        element.SetAttributeValue("valuekind", "bool");
                        element.Value = _booleanValueSerializer.SerializeValue((bool) entityPropertyValue.Value);
                    }
                    else if (entityPropertyValue.Value is DateTime)
                    {
                        element.SetAttributeValue("valuekind", "datetime");
                        element.Value = _dateValueSerializer.SerializeValue((DateTime) entityPropertyValue.Value);
                    }
                    else if (entityPropertyValue.Value is double || entityPropertyValue.Value is decimal)
                    {
                        element.SetAttributeValue("valuekind", "double");
                        element.Value =
                            _doubleValueSerializer.SerializeValue(Convert.ToDouble(entityPropertyValue.Value));
                    }
                    else if (entityPropertyValue.Value is long || entityPropertyValue.Value is int)
                    {
                        element.SetAttributeValue("valuekind", "long");
                        element.Value = _longValueSerializer.SerializeValue(Convert.ToInt64(entityPropertyValue.Value));
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            $"Unrecognized Type {entityPropertyValue.Value.GetType().Name} cannot be serialized.");
                    }
                }

                return element;
            }

            return null;
        }
    }
}