using System;
using System.Xml.Linq;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;
using Symlconnect.ViewModel.Media;

namespace Symlconnect.ViewModel.Deserializers
{
    public class FormSectionDefinitionElementDeserializer : IElementDeserializer<IFormDefinition>
    {
        private readonly IFactory<IFormSectionDefinition> _factory;

        public FormSectionDefinitionElementDeserializer(IFactory<IFormSectionDefinition> factory)
        {
            _factory = factory;
        }

        public string ElementName => "section";

        public object DeserializeFromXElement(XElement element, object parent, IFormDefinition root)
        {
            var instance = _factory.CreateInstance();
            if (element.Attribute("backcolor") != null)
            {
                instance.BackgroundColor = Color.FromHexString(element.Attribute("backcolor").Value);
            }
            if (element.Attribute("forecolor") != null)
            {
                instance.ForegroundColor = Color.FromHexString(element.Attribute("forecolor").Value);
            }
            instance.Id = element.Attribute("id") != null ? element.Attribute("id").Value : Guid.NewGuid().ToString();
            instance.Title = element.Attribute("title")?.Value;
            return instance;
        }
    }
}