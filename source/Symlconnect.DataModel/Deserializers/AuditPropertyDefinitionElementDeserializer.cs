using System.Xml.Linq;
using Symlconnect.Common.Serialization;
using Symlconnect.Contracts.Factories;

namespace Symlconnect.DataModel.Deserializers
{
    public class AuditPropertyDefinitionElementDeserializer :
        PropertyDefinitionElementDeserializerBase<AuditPropertyDefinition>
    {
        private readonly IFactory<PropertyDefinition> _propertyDefinitionFactory;

        public AuditPropertyDefinitionElementDeserializer(
            IFactory<AuditPropertyDefinition> auditPropertyDefinitionFactory,
            IFactory<PropertyDefinition> propertyDefinitionFactory)
            : base(auditPropertyDefinitionFactory)
        {
            _propertyDefinitionFactory = propertyDefinitionFactory;
        }

        public override object DeserializeFromXElement(XElement element, object parent, IDataDictionary root)
        {
            var propertyDefinitions = new DeserializedItemSet();

            // The property that actually records if this audit has been satisfied or not
            var valuePropertyDefinition = (AuditPropertyDefinition) base.DeserializeFromXElement(element, parent, root);
            propertyDefinitions.Add(valuePropertyDefinition);

            // .. and 2 additional related properties to store the UserId and ChangeDateTime
            var userIdPropertyDefinition = _propertyDefinitionFactory.CreateInstance();
            userIdPropertyDefinition.Name = $"{valuePropertyDefinition.Name}AuditUserId";
            userIdPropertyDefinition.PropertyDefinitionKind = ValueKind.Text;
            valuePropertyDefinition.UserIdPropertyDefinition = userIdPropertyDefinition;
            propertyDefinitions.Add(userIdPropertyDefinition);

            var changeDateTimePropertyDefinition = _propertyDefinitionFactory.CreateInstance();
            changeDateTimePropertyDefinition.Name = $"{valuePropertyDefinition.Name}AuditDateTime";
            changeDateTimePropertyDefinition.PropertyDefinitionKind = ValueKind.DateTime;
            valuePropertyDefinition.ChangeDateTimePropertyDefinition = changeDateTimePropertyDefinition;
            propertyDefinitions.Add(changeDateTimePropertyDefinition);

            return propertyDefinitions;
        }

        public override string ElementName => "auditproperty";
    }
}