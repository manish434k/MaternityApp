using System.Collections;
using Symlconnect.Contracts.ObjectModel;

namespace Symlconnect.DataModel
{
    /// <summary>
    ///     A property definition that derives its value in some way that is not a direct get / set from and to the store (e.g.
    ///     a calculated property)
    /// </summary>
    public class VirtualPropertyDefinition : IPropertyDefinition, IChildItemContainer, IPropertyReferenceContainer
    {
        public string Name { get; set; }
        public ValueKind PropertyDefinitionKind { get; set; }
        public IValueProvider ValueProvider { get; set; }

        public void TargetToSource(IEntity entity, EntityPropertyValueChangeset changeset)
        {
            // Virtual properties would not generally save values (i.e. they would be read only)
            changeset.Changes.Clear();
        }

        public object SourceToTarget(IEntity entity, object value, ISessionContext sessionContext)
        {
            // Calculate the value of the property using the ValueProvider
            if (ValueProvider != null)
            {
                value = ValueProvider.ResolveValue(entity, value, sessionContext);
            }

            return value;
        }

        public bool IsPropertyReferenced(string entityName, string propertyName)
        {
            if (ValueProvider is IPropertyReferenceContainer)
            {
                return ((IPropertyReferenceContainer)ValueProvider).IsPropertyReferenced(entityName, propertyName);
            }

            return false;
        }

        #region IChildItemContainer

        public bool IsSupportedChildItem(object item)
        {
            return item is IValueProvider;
        }

        public void AddChildItem(object item)
        {
            if (item is IValueProvider)
            {
                ValueProvider = (IValueProvider) item;
            }
        }

        public IEnumerable GetChildItems()
        {
            return new[] {ValueProvider};
        }

        #endregion
    }
}