using System;

namespace Symlconnect.DataModel
{
    public class ChildEntityCollectionPropertyDefinition : IChildEntityCollectionPropertyDefinition
    {
        public string Name { get; set; }

        public ValueKind PropertyDefinitionKind
        {
            get
            {
                return ValueKind.ChildEntityCollection;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IEntityDefinition EntityDefinition { get; set; }

        public void TargetToSource(IEntity entity, EntityPropertyValueChangeset changeset)
        {
            // No property value implementation for child entity collections
            throw new NotImplementedException();
        }

        public object SourceToTarget(IEntity entity, object value, ISessionContext sessionContext)
        {
            // No property value implementation for child entity collections
            throw new NotImplementedException();
        }
    }
}