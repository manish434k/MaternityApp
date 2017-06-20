using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using Symlconnect.Contracts.Environment;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.ObjectModel;

namespace Symlconnect.DataModel
{
    /// <summary>
    ///     Stores a historic set of property values for a given property.
    /// </summary>
    /// <remarks>
    ///     There are zero or one values per SessionId. Values are ordered logically by ChangeDateTime descending but may be in
    ///     the collection
    ///     in any order. Create and use a SessionContext together with the RetrieveValue to get the value as at a specific
    ///     Date and Time.
    /// </remarks>
    public class EntityPropertyValueCollection : KeyedCollection<string, EntityPropertyValue>, IChildItemContainer
    {
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;
        private readonly IFactory<EntityPropertyValue> _entityPropertyValueFactory;

        public virtual string PropertyName { get; set; }

        public EntityPropertyValueCollection(ICurrentDateTimeProvider currentDateTimeProvider,
            IFactory<EntityPropertyValue> entityPropertyValueFactory)
        {
            _currentDateTimeProvider = currentDateTimeProvider;
            _entityPropertyValueFactory = entityPropertyValueFactory;
        }

        protected override string GetKeyForItem(EntityPropertyValue item)
        {
            return item.SessionId;
        }

        public object RetrieveValue(ISessionContext sessionContext)
        {
            // Looks for a value for this session and if none found, finds the most recently saved value
            if (Contains(sessionContext.SessionId))
            {
                return this[sessionContext.SessionId].Value;
            }
            var previousEntityValue = GetPreviousEntityValue(sessionContext);
            return previousEntityValue?.Value;
        }

        public bool StoreValue(object value, ISessionContext sessionContext)
        {
            var previousEntityValue = GetPreviousEntityValue(sessionContext);

            if (Contains(sessionContext.SessionId))
            {
                // Already have a value for this session
                var entityPropertyValue = this[sessionContext.SessionId];
                if (entityPropertyValue.Value != value)
                {
                    // Only store if changed since last value stored for this session
                    if (previousEntityValue != null && value == previousEntityValue.Value)
                    {
                        // Value is now the same as in the previous session. Delete our value (i.e. we haven't actually changed it)
                        Remove(sessionContext.SessionId);
                        // Return true as the value has changed
                        return true;
                    }
                    else
                    {
                        entityPropertyValue.Value = value;
                        entityPropertyValue.ChangeDateTime = _currentDateTimeProvider.GetCurrentDateTime();
                        return true;
                    }
                }
            }
            else
            {
                // No existing value for this session. Store only if the value is different to the last session
                if (previousEntityValue?.Value != value)
                {
                    var newEntityPropertyValue = _entityPropertyValueFactory.CreateInstance();
                    newEntityPropertyValue.ChangeDateTime = _currentDateTimeProvider.GetCurrentDateTime();
                    newEntityPropertyValue.SessionId = sessionContext.SessionId;
                    newEntityPropertyValue.UserId = sessionContext.SessionUser.UserId;
                    newEntityPropertyValue.Value = value;
                    Add(newEntityPropertyValue);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Returns the most recent EntityPropertyValue that isn't for the passed SessionContext.
        /// </summary>
        private EntityPropertyValue GetPreviousEntityValue(ISessionContext sessionContext)
        {
            return this.OrderByDescending(epv => epv.ChangeDateTime)
                .FirstOrDefault(
                    epv =>
                        epv.ChangeDateTime <= sessionContext.SessionDateTime &&
                        epv.SessionId != sessionContext.SessionId);
        }

        #region IChildItemContainer

        public bool IsSupportedChildItem(object item)
        {
            return item is EntityPropertyValue;
        }

        public void AddChildItem(object item)
        {
            if (item is EntityPropertyValue)
            {
                Add((EntityPropertyValue) item);
            }
        }

        public IEnumerable GetChildItems()
        {
            return this;
        }

        #endregion
    }
}