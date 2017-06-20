namespace Symlconnect.DataModel.ValueProviders
{
    /// <summary>
    ///     Obtains a value from a property either in the same or a related Entity.
    /// </summary>
    public class PropertyValueProvider : IValueProvider, IPropertyReferenceContainer
    {
        public string EntityName { get; internal set; }
        public string PropertyName { get; internal set; }
        public ValueKind ValueProviderKind { get; set; }

        public object ResolveValue(IEntity entity, object baseValue, ISessionContext sessionContext)
        {
            var sourceEntity = entity;
            if (!string.IsNullOrWhiteSpace(EntityName) && entity.EntityDefinition.EntityName != EntityName)
            {
                // Property is in a different Entity - resolve
                sourceEntity = entity.ResolveRelatedEntity(EntityName);
            }

            if (sourceEntity != null)
            {
                if (entity.EntityDefinition.PropertyDefinitions.Contains(PropertyName))
                {
                    return entity.GetValue(PropertyName, sessionContext);
                }
            }

            return null;
        }

        public bool IsPropertyReferenced(string entityName, string propertyName)
        {
            return PropertyDefinition.IsPropertyMatch(entityName, propertyName, PropertyName);
        }
    }
}