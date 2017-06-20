using System;
using Symlconnect.Common;
using Symlconnect.Contracts.Environment;

namespace Symlconnect.DataModel.ValueProviders
{
    public class AgeValueProvider : IValueProvider, IPropertyReferenceContainer
    {
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

        public AgeValueProvider(ICurrentDateTimeProvider currentDateTimeProvider)
        {
            _currentDateTimeProvider = currentDateTimeProvider;
        }

        public string EntityName { get; internal set; }
        public string DateOfBirthPropertyName { get; internal set; }
        public string ReferenceDatePropertyName { get; internal set; }

        public ValueKind ValueProviderKind
        {
            get
            {
                return ValueKind.Integer;
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
                if (entity.EntityDefinition.PropertyDefinitions.Contains(DateOfBirthPropertyName))
                {
                    var dateOfBirth = entity.GetValue(DateOfBirthPropertyName, sessionContext);
                    if (dateOfBirth is DateTime)
                    {
                        object referenceDate = null;
                        if (!string.IsNullOrWhiteSpace(ReferenceDatePropertyName))
                        {
                            if (entity.EntityDefinition.PropertyDefinitions.Contains(ReferenceDatePropertyName))
                            {
                                referenceDate = entity.GetValue(ReferenceDatePropertyName, sessionContext);
                            }
                        }
                        else
                        {
                            referenceDate = _currentDateTimeProvider.GetCurrentDateTime();
                        }

                        if (referenceDate is DateTime)
                        {
                            return DateTimeHelpers.CalculateAge((DateTime)dateOfBirth, (DateTime)referenceDate);
                        }
                    }
                }
            }

            return null;
        }

        public bool IsPropertyReferenced(string entityName, string propertyName)
        {
            return PropertyDefinition.IsPropertyMatch(entityName, propertyName, DateOfBirthPropertyName)
                   || PropertyDefinition.IsPropertyMatch(entityName, propertyName, ReferenceDatePropertyName);
        }
    }
}