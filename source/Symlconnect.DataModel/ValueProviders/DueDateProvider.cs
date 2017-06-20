using System;
using Symlconnect.Contracts.Environment;
using Symlconnect.Common;

namespace Symlconnect.DataModel.ValueProviders
{
    public class DueDateProvider : IValueProvider, IPropertyReferenceContainer
    {
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

        public DueDateProvider(ICurrentDateTimeProvider currentDateTimeProvider)
        {
            _currentDateTimeProvider = currentDateTimeProvider;
        }

        public string EntityName { get; internal set; }
        public string DueDatePropertyName { get; internal set; }

        public ValueKind ValueProviderKind
        {
            get
            {
                return ValueKind.DateTime;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

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
                if (entity.EntityDefinition.PropertyDefinitions.Contains(DueDatePropertyName))
                {
                    return DateTimeHelpers.CalculateDueDate((DateTime)entity.GetValue(DueDatePropertyName, sessionContext));
                        
                }
            }

            return null;
        }

        public bool IsPropertyReferenced(string entityName, string propertyName)
        {
            return PropertyDefinition.IsPropertyMatch(entityName, propertyName, DueDatePropertyName);
        }
    }
}
