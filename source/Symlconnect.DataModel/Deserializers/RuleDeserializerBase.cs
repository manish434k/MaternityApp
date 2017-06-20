using System;
using System.Xml.Linq;
using Symlconnect.Common.ExtensionMethods;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.DataModel.Deserializers
{
    public abstract class RuleDeserializerBase<T> : IElementDeserializer<IDataDictionary>
        where T : IRuleDefinition
    {
        private readonly IFactory<T> _instanceFactory;

        protected RuleDeserializerBase(IFactory<T> instanceFactory)
        {
            _instanceFactory = instanceFactory;
        }

        public abstract string ElementName { get; }

        public object DeserializeFromXElement(XElement element, object parent, IDataDictionary root)
        {
            if (element.Attribute("ref")?.Value != null)
            {
                // Rule reference
                var refId = element.Attribute("ref").Value;
                if (!root.RuleDefinitions.Contains(refId))
                {
                    throw new InvalidOperationException($"Rule id {refId} not found.");
                }

                return root.RuleDefinitions[refId];
            }
            // Rule definition
            element.ValidateRequiredAttributes("id", "message", "severity");

            var instance = _instanceFactory.CreateInstance();
            instance.Id = element.Attribute("id").Value;
            instance.Message = element.Attribute("message").Value;
            instance.Severity =
                (RuleDefinitionSeverity) Enum.Parse(typeof(RuleDefinitionSeverity), element.Attribute("severity").Value);

            DeserializeAdditionalAttributes(instance, element, parent, root);

            return instance;
        }

        protected abstract void DeserializeAdditionalAttributes(T instance, XElement element, object parent,
            IDataDictionary root);
    }
}