using System;
using System.Xml.Linq;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.DataModel.Deserializers
{
    public class DataDictionaryElementDeserializer : IElementDeserializer<IDataDictionary>
    {
        private readonly IFactory<DataDictionary> _dataDictionaryFactory;

        public DataDictionaryElementDeserializer(IFactory<DataDictionary> dataDictionaryFactory)
        {
            _dataDictionaryFactory = dataDictionaryFactory;
        }

        public string ElementName => "datadictionary";

        public object DeserializeFromXElement(XElement element, object parent, IDataDictionary root)
        {
            var newInstance = _dataDictionaryFactory.CreateInstance();

            string name = element.Attribute("name")?.Value;
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidOperationException($"DataDictionary element missing name attribute: {element}");
            }

            newInstance.Name = name;

            return newInstance;
        }
    }
}