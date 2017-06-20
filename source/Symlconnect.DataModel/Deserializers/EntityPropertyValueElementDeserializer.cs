using System;
using System.Xml.Linq;
using Symlconnect.Common.ExtensionMethods;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.DataModel.Deserializers
{
    public class EntityPropertyValueElementDeserializer : IElementDeserializer<IEntity>
    {
        private readonly IFactory<EntityPropertyValue> _entityPropertyValueFactory;
        private readonly IValueDeserializer<bool> _booleanValueDeserializer;
        private readonly IValueDeserializer<DateTime> _dateValueDeserializer;
        private readonly IValueDeserializer<double> _doubleValueDeserializer;
        private readonly IValueDeserializer<long> _longValueDeserializer;

        public EntityPropertyValueElementDeserializer(IFactory<EntityPropertyValue> entityPropertyValueFactory,
            IValueDeserializer<bool> booleanValueDeserializer,
            IValueDeserializer<DateTime> dateValueDeserializer,
            IValueDeserializer<double> doubleValueDeserializer,
            IValueDeserializer<long> longValueDeserializer)
        {
            _entityPropertyValueFactory = entityPropertyValueFactory;
            _booleanValueDeserializer = booleanValueDeserializer;
            _dateValueDeserializer = dateValueDeserializer;
            _doubleValueDeserializer = doubleValueDeserializer;
            _longValueDeserializer = longValueDeserializer;
        }

        public string ElementName => "value";

        public object DeserializeFromXElement(XElement element, object parent, IEntity root)
        {
            element.ValidateRequiredAttributes("sessionid", "userid", "changedatetime");

            var entityPropertyValue = _entityPropertyValueFactory.CreateInstance();
            entityPropertyValue.SessionId = element.Attribute("sessionid").Value;
            entityPropertyValue.UserId = element.Attribute("userid").Value;
            entityPropertyValue.ChangeDateTime =
                _dateValueDeserializer.DeserializeValue(element.Attribute("changedatetime").Value);

            if (!string.IsNullOrEmpty(element.Value))
            {
                var valueKind = element.Attribute("valuekind")?.Value;
                var value = element.Value;

                if (string.IsNullOrEmpty(valueKind))
                {
                    // Text value
                    entityPropertyValue.Value = value;
                }
                else
                {
                    switch (valueKind)
                    {
                        case "bool":
                            entityPropertyValue.Value = _booleanValueDeserializer.DeserializeValue(value);
                            break;
                        case "datetime":
                            entityPropertyValue.Value = _dateValueDeserializer.DeserializeValue(value);
                            break;
                        case "double":
                            entityPropertyValue.Value = _doubleValueDeserializer.DeserializeValue(value);
                            break;
                        case "long":
                            entityPropertyValue.Value = _longValueDeserializer.DeserializeValue(value);
                            break;
                        default:
                            throw new InvalidOperationException($"Unrecognized valuekind of {valueKind} encountered.");
                    }
                }
            }

            return entityPropertyValue;
        }
    }
}