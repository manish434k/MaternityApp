using System;
using System.Xml.Linq;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.ViewModel.Deserializers
{
    public class BasicControlDefinitionElementDeserializer<T, TFactory> : IElementDeserializer<IFormDefinition>
        where T : IControlDefinition
        where TFactory : IFactory<T>
    {
        private readonly TFactory _factory;

        public BasicControlDefinitionElementDeserializer(TFactory factory, string elementName)
        {
            _factory = factory;
            ElementName = elementName;
        }

        public string ElementName { get; }

        public virtual object DeserializeFromXElement(XElement element, object parent, IFormDefinition root)
        {
            var instance = _factory.CreateInstance();
            instance.Caption = element.Attribute("caption")?.Value;
            instance.Id = element.Attribute("id") != null ? element.Attribute("id").Value : Guid.NewGuid().ToString();
            instance.IsVisiblePropertyName = element.Attribute("visible")?.Value;
            instance.ValuePropertyName = element.Attribute("value")?.Value;
            instance.Margin = element.Attribute("margin")?.Value;
            instance.Width = element.Attribute("width")?.Value;

            DeserializeAdditionalAttributes(instance, element, parent, root);

            var refValue = element.Attribute("ref")?.Value;
            if (!string.IsNullOrWhiteSpace(refValue))
            {
                if (root.SharedControlDefinitions.Contains(refValue))
                {
                    var refControl = root.SharedControlDefinitions[refValue];
                    CopyReferencedSharedControlProperties(instance, refControl);
                }
            }
            return instance;
        }

        protected virtual void DeserializeAdditionalAttributes(T instance, XElement element, object parent, IFormDefinition root)
        {
            
        }

        protected virtual void CopyReferencedSharedControlProperties(T instance,
            IControlDefinition sharedControlDefinition)
        {
            if (string.IsNullOrWhiteSpace(instance.Caption))
            {
                instance.Caption = sharedControlDefinition.Caption;
            }
            if (string.IsNullOrWhiteSpace(instance.IsVisiblePropertyName))
            {
                instance.IsVisiblePropertyName = sharedControlDefinition.IsVisiblePropertyName;
            }
            if (string.IsNullOrWhiteSpace(instance.ValuePropertyName))
            {
                instance.ValuePropertyName = sharedControlDefinition.ValuePropertyName;
            }
            if (string.IsNullOrWhiteSpace(instance.Margin))
            {
                instance.Margin = sharedControlDefinition.Margin;
            }
        }
    }
}