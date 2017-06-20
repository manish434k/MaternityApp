using System;
using Symlconnect.Contracts.Environment;

namespace Symlconnect.DataModel
{
    /// <summary>
    ///     An Audit Property is a specialised boolean property used for Audit, where setting the value to
    ///     true automatically records the date/time and user id in secondary properties.
    /// </summary>
    public class AuditPropertyDefinition : PropertyDefinition
    {
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;
        public IPropertyDefinition UserIdPropertyDefinition { get; set; }
        public IPropertyDefinition ChangeDateTimePropertyDefinition { get; set; }

        public AuditPropertyDefinition(ICurrentDateTimeProvider currentDateTimeProvider)
        {
            _currentDateTimeProvider = currentDateTimeProvider;
        }

        public override ValueKind PropertyDefinitionKind
        {
            get { return ValueKind.Boolean; }
            set { throw new NotImplementedException(); }
        }

        public override void TargetToSource(IEntity entity, EntityPropertyValueChangeset changeset)
        {
            if ((bool) changeset.GetNewValue(entity, Name))
            {
                // Set to true, also record the UserName and ChangeDateTime
                changeset.Changes.Add(new EntityPropertyValueChange
                {
                    EntityName = entity.EntityDefinition.EntityName,
                    PropertyName = UserIdPropertyDefinition.Name,
                    NewValue = changeset.SessionContext.SessionUser.UserId
                });
                changeset.Changes.Add(new EntityPropertyValueChange
                {
                    EntityName = entity.EntityDefinition.EntityName,
                    PropertyName = ChangeDateTimePropertyDefinition.Name,
                    NewValue = _currentDateTimeProvider.GetCurrentDateTime()
                });
            }
            else
            {
                // Set to false, remove the audit
                changeset.Changes.Add(new EntityPropertyValueChange
                {
                    EntityName = entity.EntityDefinition.EntityName,
                    PropertyName = UserIdPropertyDefinition.Name,
                    NewValue = null
                });
                changeset.Changes.Add(new EntityPropertyValueChange
                {
                    EntityName = entity.EntityDefinition.EntityName,
                    PropertyName = ChangeDateTimePropertyDefinition.Name,
                    NewValue = null
                });
            }
        }
    }
}