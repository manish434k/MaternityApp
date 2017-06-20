using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Symlconnect.DataModel
{
    /// <summary>
    ///     Represents a batch of property changes to be committed.
    /// </summary>
    public class EntityPropertyValueChangeset
    {
        [ExcludeFromCodeCoverage] // Simple Property
        public ISessionContext SessionContext { get; internal set; }
        [ExcludeFromCodeCoverage] // Simple Property
        public IDataDictionary DataDictionary { get; internal set; }
        public virtual IList<EntityPropertyValueChange> Changes { get; } = new List<EntityPropertyValueChange>();
        private readonly Lazy<RuleDefinitionCollection> _invalidRuleDefinitions
            = new Lazy<RuleDefinitionCollection>(() => new RuleDefinitionCollection());
        public RuleDefinitionCollection InvalidRuleDefinitions => _invalidRuleDefinitions.Value;

        /// <summary>
        ///     Helper method to find and return the new value from a record in Changes matching the passed IEntity and
        ///     propertyName.
        /// </summary>
        /// <param name="entity">The IEntity in which the property is located.</param>
        /// <param name="propertyName">The name of the property the new value should be returned for.</param>
        /// <returns></returns>
        public object GetNewValue(IEntity entity, string propertyName)
        {
            return
                Changes.FirstOrDefault(
                    c =>
                        c.PropertyName == propertyName
                        && (string.IsNullOrEmpty(c.EntityName)
                            || c.EntityName == entity.EntityDefinition.EntityName))?.NewValue;
        }

        public bool WasStoreUpdatedForAllChanges => Changes.All(c => c.WasStoreUpdated);
    }
}