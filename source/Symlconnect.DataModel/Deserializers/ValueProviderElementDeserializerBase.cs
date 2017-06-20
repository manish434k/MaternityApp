using System;
using System.Xml.Linq;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.DataModel.Deserializers
{
    public abstract class ValueProviderElementDeserializerBase<T> : IElementDeserializer<IDataDictionary>
        where T : IValueProvider
    {
        private readonly IFactory<T> _valueProviderFactory;

        protected ValueProviderElementDeserializerBase(IFactory<T> valueProviderFactory)
        {
            _valueProviderFactory = valueProviderFactory;
        }

        public abstract string ElementName { get; }

        public virtual object DeserializeFromXElement(XElement element, object parent, IDataDictionary root)
        {
            var newInstance = _valueProviderFactory.CreateInstance();
            if (!string.IsNullOrWhiteSpace(element.Attribute("kind")?.Value))
            {
                newInstance.ValueProviderKind = (ValueKind) Enum.Parse(typeof(ValueKind),
                    element.Attribute("kind").Value, true);
            }

            return newInstance;
        }
    }
}