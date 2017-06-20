using System.Xml.Linq;
using Symlconnect.Contracts.Factories;

namespace Symlconnect.ViewModel.Deserializers
{
    public class MessageControlDefinitionDeserializer<T, TFactory> :
        BasicControlDefinitionElementDeserializer<T, TFactory>
        where T : MessageControlDefinitionBase
        where TFactory : IFactory<T>
    {
        public MessageControlDefinitionDeserializer(TFactory factory, string elementName) : base(factory, elementName)
        {
        }

        protected override void DeserializeAdditionalAttributes(T instance, XElement element, object parent,
            IFormDefinition root)
        {
            if (instance != null)
            {
                instance.ContentPropertyName = element.Attribute("content")?.Value;
                instance.Content = element.Value;
            }
        }

        protected override void CopyReferencedSharedControlProperties(T instance,
            IControlDefinition sharedControlDefinition)
        {
            base.CopyReferencedSharedControlProperties(instance, sharedControlDefinition);
            var messageControlDefinition = (T) sharedControlDefinition;
            if (string.IsNullOrWhiteSpace(instance.ContentPropertyName))
            {
                instance.ContentPropertyName = messageControlDefinition.ContentPropertyName;
            }
            if (string.IsNullOrWhiteSpace(instance.Content))
            {
                instance.Content = messageControlDefinition.Content;
            }
        }
    }
}