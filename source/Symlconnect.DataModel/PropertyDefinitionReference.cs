using System;

namespace Symlconnect.DataModel
{
    /// <summary>
    ///     A Property Definition Reference is essentially a proxy for a shared Property Definition and exists as a means to
    ///     reduce duplication in the configuration model, where many Property Definitions are repeated in different
    ///     Entities.
    /// </summary>
    public class PropertyDefinitionReference : IPropertyDefinition
    {
        public IPropertyDefinition ReferencedPropertyDefinition { get; internal set; }
        public string Name { get; set; }

        public ValueKind PropertyDefinitionKind
        {
            get
            {
                return ReferencedPropertyDefinition.PropertyDefinitionKind;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void TargetToSource(IEntity entity, EntityPropertyValueChangeset changeset)
        {
            ReferencedPropertyDefinition.TargetToSource(entity, changeset);
        }

        public object SourceToTarget(IEntity entity, object value, ISessionContext sessionContext)
        {
            return ReferencedPropertyDefinition.SourceToTarget(entity, value, sessionContext);
        }
    }
}