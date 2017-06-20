using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Symlconnect.Contracts.Environment;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.ObjectModel;

namespace Symlconnect.DataModel
{
    /// <summary>
    ///     Represents a single instance of an Entity (i.e. a Patient, or a Consultation).
    /// </summary>
    public class Entity : IEntity, IChildItemContainer
    {
        private readonly IFactory<EntityPropertyValueCollection> _entityPropertyValueCollectionFactory;
        private readonly IFactory<ChildEntityCollection> _childEntityCollectionFactory;
        private readonly IFactory<ChildEntity> _childEntityFactory;
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

        private readonly Lazy<Dictionary<string, EntityPropertyValueCollection>> _propertyValues
            =
            new Lazy<Dictionary<string, EntityPropertyValueCollection>>(
                () => new Dictionary<string, EntityPropertyValueCollection>());

        private readonly Lazy<Dictionary<string, ChildEntityCollection>> _childEntities
            =
            new Lazy<Dictionary<string, ChildEntityCollection>>(
                () => new Dictionary<string, ChildEntityCollection>());

        public Entity(IFactory<EntityPropertyValueCollection> entityPropertyValueCollectionFactory,
            IFactory<ChildEntityCollection> childEntityCollectionFactory,
            IFactory<ChildEntity> childEntityFactory,
            ICurrentDateTimeProvider currentDateTimeProvider)
        {
            _entityPropertyValueCollectionFactory = entityPropertyValueCollectionFactory;
            _childEntityCollectionFactory = childEntityCollectionFactory;
            _childEntityFactory = childEntityFactory;
            _currentDateTimeProvider = currentDateTimeProvider;
        }

        private string _id;

        public string Id
        {
            get { return _id ?? (_id = Guid.NewGuid().ToString()); }
            set { _id = value; }
        }

        public string CreatedByUserId { get; set; }
        public string CreatedByUserDisplayName { get; set; }
        public DateTime CreatedDateTime { get; set; }

        internal Dictionary<string, EntityPropertyValueCollection> PropertyValues => _propertyValues.Value;
        internal Dictionary<string, ChildEntityCollection> ChildEntities => _childEntities.Value;

        private EntityPropertyValueCollection GetEntityPropertyValueCollection(string propertyName)
        {
            if (PropertyValues.ContainsKey(propertyName.ToUpperInvariant()))
            {
                return PropertyValues[propertyName.ToUpperInvariant()];
            }
            var newPropertyValues = _entityPropertyValueCollectionFactory.CreateInstance();
            newPropertyValues.PropertyName = propertyName;
            PropertyValues.Add(propertyName.ToUpperInvariant(), newPropertyValues);
            return newPropertyValues;
        }

        private void OnEntityPropertyChanged(string propertyName)
        {
            EntityPropertyChanged?.Invoke(this, new EntityPropertyChangedEventArgs(this, propertyName));
        }

        public IEntityDefinition EntityDefinition { get; set; }

        public EntitySetValueResult SetValue(string propertyName, object value, ISessionContext sessionContext)
        {
            if (EntityDefinition.PropertyDefinitions.Contains(propertyName))
            {
                var propertyDefinition = EntityDefinition.PropertyDefinitions[propertyName];

                var changeset = new EntityPropertyValueChangeset
                {
                    SessionContext = sessionContext,
                    DataDictionary = EntityDefinition.DataDictionary
                };

                changeset.Changes.Add(new EntityPropertyValueChange
                {
                    EntityName = EntityDefinition.EntityName,
                    PropertyName = propertyName,
                    NewValue = value
                });

                propertyDefinition.TargetToSource(this, changeset);

                if (changeset.InvalidRuleDefinitions.Any(rule => rule.Severity == RuleDefinitionSeverity.Error))
                {
                    // Critical validation rules broken. Do not save
                    return new EntitySetValueResult
                    {
                        IsSuccess = false,
                        InvalidRuleDefinitions = changeset.InvalidRuleDefinitions
                    };
                }

                bool allChangesStoredOk = true;
                var changedPropertyNames = new Dictionary<string, EntityPropertyValueChange>();
                foreach (var change in changeset.Changes)
                {
                    bool result = GetEntityPropertyValueCollection(change.PropertyName)
                        .StoreValue(change.NewValue, sessionContext);
                    change.WasStoreUpdated = result;
                    if (result)
                    {
                        changedPropertyNames.Add($"{change.EntityName}.{change.PropertyName}", change);
                    }
                    allChangesStoredOk &= result;
                }

                foreach (
                    var possiblePropertyDefinition in
                    EntityDefinition.PropertyDefinitions.OfType<IPropertyReferenceContainer>())
                {
                    foreach (var change in changedPropertyNames.Values.ToList())
                    {
                        if (possiblePropertyDefinition.IsPropertyReferenced(change.EntityName, change.PropertyName))
                        {
                            changedPropertyNames.Add(
                                $"{change.EntityName}.{((IPropertyDefinition) possiblePropertyDefinition).Name}",
                                new EntityPropertyValueChange
                                {
                                    EntityName = EntityDefinition.EntityName,
                                    PropertyName = ((IPropertyDefinition) possiblePropertyDefinition).Name
                                });
                        }
                    }
                }
                // TODO: Should repeat the process above until no new properties are revealed - 
                //  to discover any secondary properties affectected

                foreach (
                    var change in changedPropertyNames.Values.Where(c => c.EntityName == EntityDefinition.EntityName))
                {
                    OnEntityPropertyChanged(change.PropertyName);
                }

                // TODO: Raise changes in related entities?

                return new EntitySetValueResult
                {
                    IsSuccess = true,
                    ChangeSet = changeset,
                    InvalidRuleDefinitions = changeset.InvalidRuleDefinitions
                };
            }
            throw new InvalidOperationException(
                $"Property {propertyName} could not be found in the EntityDefinition");
        }

        public object GetValue(string propertyName, ISessionContext sessionContext)
        {
            if (EntityDefinition.PropertyDefinitions.Contains(propertyName))
            {
                var propertyDefinition = EntityDefinition.PropertyDefinitions[propertyName];
                var sourceValue = GetEntityPropertyValueCollection(propertyName).RetrieveValue(sessionContext);
                return propertyDefinition.SourceToTarget(this, sourceValue, sessionContext);
            }
            throw new InvalidOperationException($"Property {propertyName} could not be found in the EntityDefinition");
        }

        public bool AddChildEntity(string childEntityPropertyName, IEntity childEntity, ISessionContext sessionContext)
        {
            var childEntityCollectionPropertyDefinition =
                GetChildEntityCollectionPropertyDefinition(childEntityPropertyName);

            if (childEntityCollectionPropertyDefinition == null)
            {
                throw new InvalidOperationException(
                    $"Could not find the Child Entity property {childEntityPropertyName}");
            }

            // Validate the entity type
            if (childEntity.EntityDefinition.EntityName !=
                childEntityCollectionPropertyDefinition.EntityDefinition.EntityName)
            {
                throw new InvalidOperationException(
                    $"Child Entity was not expected EntityName. Expected {childEntityCollectionPropertyDefinition.EntityDefinition.EntityName} but was {childEntity.EntityDefinition.EntityName}");
            }

            var newChildEntity = _childEntityFactory.CreateInstance();
            newChildEntity.SessionId = sessionContext.SessionId;
            newChildEntity.UserId = sessionContext.SessionUser.UserId;
            newChildEntity.CreatedDateTime = _currentDateTimeProvider.GetCurrentDateTime();
            newChildEntity.Entity = childEntity;

            var collection = GetChildEntityCollection(childEntityPropertyName);
            collection.Add(newChildEntity);

            return true;
        }

        private ChildEntityCollection GetChildEntityCollection(string childEntityPropertyName)
        {
            var collection = !ChildEntities.ContainsKey(childEntityPropertyName)
                ? CreateChildEntityCollection(childEntityPropertyName)
                : ChildEntities[childEntityPropertyName];

            return collection;
        }

        private ChildEntityCollection CreateChildEntityCollection(string childEntityPropertyName)
        {
            var collection = _childEntityCollectionFactory.CreateInstance();
            collection.PropertyName = childEntityPropertyName;
            ChildEntities.Add(childEntityPropertyName, collection);
            return collection;
        }

        public IEnumerable<IEntity> GetChildEntities(string childEntityPropertyName, ISessionContext sessionContext)
        {
            if (ChildEntities.ContainsKey(childEntityPropertyName))
            {
                return ChildEntities[childEntityPropertyName].GetChildItems(sessionContext).Select(ci => ci.Entity);
            }

            return Enumerable.Empty<IEntity>();
        }

        private IChildEntityCollectionPropertyDefinition GetChildEntityCollectionPropertyDefinition(
            string childEntityPropertyName)
        {
            if (EntityDefinition.PropertyDefinitions.Contains(childEntityPropertyName))
            {
                return
                    EntityDefinition.PropertyDefinitions[childEntityPropertyName] as
                        IChildEntityCollectionPropertyDefinition;
            }

            return null;
        }

        public IEntity ResolveRelatedEntity(string entityName)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<EntityPropertyChangedEventArgs> EntityPropertyChanged;

        #region IChildItemContainer

        bool IChildItemContainer.IsSupportedChildItem(object item)
        {
            return item is EntityPropertyValueCollection || item is ChildEntityCollection;
        }

        void IChildItemContainer.AddChildItem(object item)
        {
            if (item is EntityPropertyValueCollection)
            {
                var entityPropertyValues = (EntityPropertyValueCollection) item;
                if (!PropertyValues.ContainsKey(entityPropertyValues.PropertyName.ToUpperInvariant()))
                {
                    PropertyValues.Add(entityPropertyValues.PropertyName.ToUpperInvariant(), entityPropertyValues);
                }
                else
                {
                    throw new InvalidOperationException(
                        $"Property {entityPropertyValues.PropertyName} passed to AddChildItem already exists on Entity {EntityDefinition?.EntityName}");
                }
            }
            else if (item is ChildEntityCollection)
            {
                var collection = (ChildEntityCollection) item;
                if (ChildEntities.ContainsKey(collection.PropertyName))
                {
                    // Already exists
                    throw new InvalidOperationException(
                        $"A Child Entity collection for property name {collection.PropertyName} already exists.");
                }
                ChildEntities.Add(collection.PropertyName, collection);
            }
        }

        IEnumerable IChildItemContainer.GetChildItems()
        {
            return PropertyValues.Values.Concat<object>(ChildEntities.Values);
        }

        #endregion
    }
}